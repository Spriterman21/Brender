using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    [Serializable()]
    public class Camera : Component
    {
        public Camera()
        {
        }

        public static float HWratio = 9f / 20f; // ratio of the height and width of a character in console

        public float viewingAngle; // literally FOV
        public CharInfo[,] image;
        private Vector3[] axes = new Vector3[3]
        {
            Vector3.Right(),
            Vector3.Forward(),
            Vector3.Up()
        };

    #region 3D points to Canvas
    public float[] PointToCanvas(Vector3 point)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            float canvasDistance = width / (2f * MathF.Tan(viewingAngle));

            Tuple<Vector3, float> shadowOperations = CamPlanePointShadow(point);
            Vector3 shadow = shadowOperations.Item1;

            float normalDistance = shadowOperations.Item2;
            if (normalDistance <= 0)
            {
                return null;
            }

            float horizontalDistance = ShadowCamHorizontal(shadow);
            float verticalDistance = ShadowCamVertical(shadow);

            float[] canvasPoint = new float[2];
            canvasPoint[0] = -(horizontalDistance * canvasDistance) / (normalDistance) + width / 2f;
            canvasPoint[1] = -HWratio * (verticalDistance * canvasDistance) / (normalDistance) + height / 2f;

            return canvasPoint;
        }

        /// <summary>
        /// Finds the intersect (shadow) of the camera plane and a normal line going through desired point + returns the signed distance from shadow to point
        /// </summary>
        /// <returns></returns>
        Tuple<Vector3, float> CamPlanePointShadow(Vector3 point)
        {
            float d = axes[1] * _object.globalPosition; // fourth variable of the plane equation
            float t = (d - (axes[1] * point)) / (axes[1].Length() * axes[1].Length()); // linear multiple of axes[1] / starting at point, ending at shadow

            Vector3 shadow = point + (t * axes[1]);
            Vector3 shadowToPoint = -t * axes[1];

            float distance = -MathF.Sign(t) * shadowToPoint.Length();

            return new Tuple<Vector3, float>(shadow, distance);
        }

        /// <summary>
        /// returns the signed distance between shadow and line defined by globalPosition and a directional vector axes[2]
        /// </summary>
        /// <returns></returns>
        float ShadowCamHorizontal(Vector3 shadow)
        {
            Vector3 shadowToCam = _object.globalPosition - shadow;

            float horizontalDistance = (shadowToCam % axes[2]).Length() / axes[2].Length();

            return -MathF.Sign(shadowToCam * axes[0]) * horizontalDistance;
        }


        /// <summary>
        /// returns the signed distance between shadow and line defined by globalPosition and a directional vector axes[0]
        /// </summary>
        /// <returns></returns>
        float ShadowCamVertical(Vector3 shadow)
        {
            Vector3 shadowToCam = _object.globalPosition - shadow;

            float verticalDistance = (shadowToCam % axes[0]).Length() / axes[0].Length();

            return -MathF.Sign(shadowToCam * axes[2]) * verticalDistance;
        }
        #endregion

    #region Polygon manipulation
        public void PolygonSetuper(Polygon polygon)
        {
            //Console.WriteLine("polygon");
            Vector3 holder = Vector3.Zero();
            for (int i = 0; i < polygon.realPoints.Length; i++)
            {
                polygon.canvasPoints[i] = PointToCanvas(polygon.realPoints[i]); // finds the points on the canvas
                polygon.camDistance[i] = (_object.globalPosition - polygon.realPoints[i]).Length();
                holder += polygon.realPoints[i]; // for finding the average distance to that polygon
            }

            polygon.distance = (_object.globalPosition - (holder / polygon.realPoints.Length)).Length();
        }

        public bool BackFaceCulling(Polygon polygon)
        {
            Vector3 normal = (polygon.realPoints[1] - polygon.realPoints[0]) % (polygon.realPoints[2] - polygon.realPoints[0]);

            return polygon.draw = (polygon.realPoints[0] - _object.globalPosition) * normal <= 0;
        }

        /// <summary>
        /// For ordering polygons for render from the furthest to closest
        /// </summary>
        /// <param name="polygons"></param>
        public void BubbleSort(Polygon[] polygons)
        {
            for (int i = polygons.Length - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (polygons[j].distance <= polygons[j + 1].distance)
                    {
                        if (polygons[j].distance == polygons[j + 1].distance)
                        {
                            if (polygons[j].drawPreference <= polygons[j + 1].drawPreference)
                            {
                                continue;
                            }
                        }
                        Polygon holder = polygons[j];
                        polygons[j] = polygons[j + 1];
                        polygons[j + 1] = holder;
                    }
                }
            }
        }


        #endregion

        public void PaintOver(CharInfo[,] addCanvas)
        {
            for (int i = 0; i < addCanvas.GetLength(1); i++)
            {
                for (int j = 0; j < addCanvas.GetLength(0); j++)
                {
                    try
                    {
                        if (addCanvas[j, i] != HolderClass.background)
                        {
                            image[j, i] = addCanvas[j, i];
                        } // there seems to be a problem here sometimes
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public override void Update()
        {
            image = new CharInfo[HolderClass.x = Console.BufferWidth, HolderClass.y = Console.BufferHeight];
            Polygon[] polygons = HolderClass.polygons.ToArray(); // load all polygons to render
            
            axes = new Vector3[3] // the axes of the camera centered inertial frame of reference
            {
                Quaternion.RotatePoint(_object.globalRotation, Vector3.Right()),
                Quaternion.RotatePoint(_object.globalRotation, Vector3.Forward()),
                Quaternion.RotatePoint(_object.globalRotation, Vector3.Up())
            };

            UpdateWindow();

            if (polygons.Length == 0)
            {
                return;
            }

            // goes through polygons, prepares them for rendering
            foreach (Polygon polygon in polygons)
            {
                if (polygon.applyBackFaceCulling)
                {
                    if (BackFaceCulling(polygon))
                    {
                        PolygonSetuper(polygon);
                    }
                }
                else
                {
                    PolygonSetuper(polygon); 
                    polygon.draw = true;
                }
            }

            // sorts polygons acording to Painter algorithm
            BubbleSort(polygons);

            // renders polygons onto the newly created canvas
            for (int i = 0; i < polygons.Length; i++)
            {
                if (polygons[i].draw)
                {
                    CharInfo[,] addCanvas = LineDrawer.DrawPolygon(polygons[i], Console.BufferWidth, Console.BufferHeight);

                    PaintOver(addCanvas);
                }
            }

            #region may try multithreading later
            //System.Threading.Tasks.Parallel.For(0, polygons.Length, i =>
            //{
            //    if (polygons[i].draw)
            //    {
            //        CharInfo[,] addCanvas = LineDrawer.DrawPolygon(polygons[i], Console.BufferWidth, Console.BufferHeight);

            //        PaintOver(addCanvas);
            //    }
            //});
            #endregion
        }

        /// <summary>
        /// Handles the size of console buffer to allow for different screen size
        /// Inspired by https://stackoverflow.com/questions/46862027/manipulating-net-system-console-window-size-causes-an-argumentoutofboundsexcept/46863855
        /// (but really just inspired, that code looked ugly and kept crashing in my situations, so some major modifications were made)
        /// </summary>
        private void UpdateWindow()
        {
            // screen sizes from last frame
            int oldX = HolderClass.x;
            int oldY = HolderClass.y;

            // screen sizes for following frame to be applied to the console
            int newX = Console.WindowWidth;
            int newY = Console.WindowHeight;

            try
            {
                if (newX <= oldX)
                {
                    Console.WindowWidth = newX;
                    Console.BufferWidth = newX;
                }
                else
                {
                    Console.WindowWidth = newX;
                    Console.BufferWidth = newX;
                }

                if (newY <= oldY)
                {
                    Console.WindowHeight = newY;
                    Console.BufferHeight = newY;
                }
                else
                {
                    Console.BufferHeight = newY;
                    Console.WindowHeight = newY;
                }
            }
            catch (Exception)
            {
                // not gonna lie, I have no idea why these exeptions exist because I don't understand buffers and stuff so well
                // aaaaanyway, this little catch seems to solve all my problems
            }
        }

        #region Menu creation
        static readonly string[] names = new string[]
        {
            "Name",
            "FOV"
        };

        public override void StartOwnMenu()
        {
            List<object> optionFns = new List<object>();
            Ref<float> fov = new Ref<float>(viewingAngle);

            optionFns.Add(name);
            optionFns.Add(fov);

            ListMenu<object> menu = new ListMenu<object>(name, names, optionFns);
            menu.EngageMenu();

            viewingAngle = fov.value;
        }
        #endregion
    }
}
