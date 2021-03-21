using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Runtime.InteropServices;

// https://sketchfab.com/3d-models/doom-e1m1-hangar-map-2148fb6a3fe7454b901fcea67d70b318  doom

namespace Brender_0_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            EngagingBaseMenu();

            HolderClass.x = Console.BufferWidth;
            HolderClass.y = Console.BufferHeight;
            
            SetScreenColorsApp.SetColor((ConsoleColor)1, 100, 180, 50); // grass
            SetScreenColorsApp.SetColor((ConsoleColor)2, 135, 90, 25); // dirt
            SetScreenColorsApp.SetColor((ConsoleColor)3, 135, 100, 45); // outer tree
            SetScreenColorsApp.SetColor((ConsoleColor)4, 220, 175, 90); // inner tree
            SetScreenColorsApp.SetColor((ConsoleColor)5, 60, 120, 25); // leaves
            SetScreenColorsApp.SetColor((ConsoleColor)6, 125, 125, 125); // stone
            SetScreenColorsApp.SetColor((ConsoleColor)7, 70, 255, 40); // creeper

            HolderClass.background = new CharInfo('\0', 0, 0);
            CharInfo grass = new CharInfo(' ', 0, 1);
            CharInfo dirt = new CharInfo(' ', 0, 2);
            CharInfo stump = new CharInfo(' ', 0, 3);
            CharInfo wood = new CharInfo(' ', 0, 4);
            CharInfo leave = new CharInfo(' ', 0, 5);
            CharInfo stone = new CharInfo(' ', 0, 6);
            CharInfo creeperSkin = new CharInfo(' ', 0, 7);
            CharInfo black = new CharInfo(' ', 0, 0);
            CharInfo outline = new CharInfo('X', 15, 0);

            Camera cam = new Camera();
            Controls camCont = new Controls();

            // defining objects ///////////////
            Object camera = new Object(new Component[] { cam, camCont });

            #region blocks
            // cube ///////////////////////////
            Object gb0 = new Object("gb0", new Vector3(1, 0, 0));
            Object gb1 = new Object("gb1", new Vector3(-1, 0, 0));
            Object gb2 = new Object("gb2", new Vector3(0, 1, 0));
            Object gb3 = new Object("gb3", new Vector3(0, -1, 0));
            Object log = new Object("log", new Vector3(0, 0, 1));
            Object sb = new Object("sb", new Vector3(0, 0, -1));
            Object leaves0 = new Object("leaves0", new Vector3(0, 0, 3));
            Object leaves1 = new Object("leaves1", new Vector3(1, 0, 2));
            Object leaves2 = new Object("leaves2", new Vector3(-1, 0, 2));
            Object leaves3 = new Object("leaves3", new Vector3(0, 1, 2));
            Object leaves4 = new Object("leaves4", new Vector3(0, -1, 2));


            CharInfo[] grassSides = new CharInfo[] { dirt, dirt, dirt, dirt, dirt, grass };
            CharInfo[] stoneSides = new CharInfo[] { stone, stone, stone, stone, stone, stone };
            CharInfo[] logSides = new CharInfo[] { wood, stump, stump, stump, stump, wood };
            CharInfo[] leaveSides = new CharInfo[] { leave, leave, leave, leave, leave, leave };

            gb0.Cube(Vector3.FullOne(), grassSides, outline);
            gb1.Cube(Vector3.FullOne(), grassSides, outline);
            gb2.Cube(Vector3.FullOne(), grassSides, outline);
            gb3.Cube(Vector3.FullOne(), grassSides, outline);
            log.Cube(Vector3.FullOne(), logSides, outline);
            leaves0.Cube(Vector3.FullOne(), leaveSides, outline);
            leaves1.Cube(Vector3.FullOne(), leaveSides, outline);
            leaves2.Cube(Vector3.FullOne(), leaveSides, outline);
            leaves3.Cube(Vector3.FullOne(), leaveSides, outline);
            leaves4.Cube(Vector3.FullOne(), leaveSides, outline);
            sb.Cube(Vector3.FullOne(), stoneSides, outline);

            Object gb4 = HolderClass.DeepClone(gb0);
            Object gb5 = HolderClass.DeepClone(gb0);
            Object gb6 = HolderClass.DeepClone(gb0);
            Object sb2 = HolderClass.DeepClone(sb);

            gb4.position = new Vector3(2, 0, 0);
            gb4.moved = true;
            gb5.position = new Vector3(2, 1, 0);
            gb5.moved = true;
            gb6.position = new Vector3(3, 1, 0);
            gb6.moved = true;
            sb2.position = new Vector3(1, 0, -1);
            sb2.moved = true;

            #endregion

            #region creeper
            //            CharInfo[] creeperSides = new CharInfo[] { creeperSkin, creeperSkin, creeperSkin, creeperSkin, creeperSkin, creeperSkin };


            //            // HEAD ///////////////////////////////////////////////////////
            //            Object head = new Object("head", new Vector3(0, 0, 1.375f));
            //            head.Cube(new Vector3(0.5f, 0.5f, 0.5f), creeperSides, outline);
            //            Mesh headMesh = (Mesh)head.components[0];

            //            Vector3[] square = new Vector3[]
            //            {
            //                new Vector3(-1, 0, -1),
            //                new Vector3(1, 0, -1),
            //                new Vector3(1, 0, 1),
            //                new Vector3(-1, 0, 1)
            //            };

            //            int[][] endpoints = new int[4][]
            //{
            //                new int[]{ 0, 1},
            //                new int[]{ 1, 2},
            //                new int[]{ 2, 3},
            //                new int[]{ 3, 0}
            //};

            //            Edge[] edges = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                edges[j] = new Edge();
            //                edges[j].pixel = creeperSkin;
            //                edges[j].points = endpoints[j];
            //            }
            //            Edge[] blackedges = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                blackedges[j] = new Edge();
            //                blackedges[j].pixel = black;
            //                blackedges[j].points = endpoints[j];
            //            }
            //            Edge[] le = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                le[j] = new Edge();
            //                le[j].pixel = creeperSkin;
            //                le[j].points = endpoints[j];
            //            }
            //            le[3].pixel = outline;
            //            Edge[] ue = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                ue[j] = new Edge();
            //                ue[j].pixel = creeperSkin;
            //                ue[j].points = endpoints[j];
            //            }
            //            ue[2].pixel = outline;
            //            Edge[] re = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                re[j] = new Edge();
            //                re[j].pixel = creeperSkin;
            //                re[j].points = endpoints[j];
            //            }
            //            re[1].pixel = outline;
            //            Edge[] de = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                de[j] = new Edge();
            //                de[j].pixel = creeperSkin;
            //                de[j].points = endpoints[j];
            //            }
            //            de[0].pixel = outline;
            //            Edge[] lue = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                lue[j] = new Edge();
            //                lue[j].pixel = creeperSkin;
            //                lue[j].points = endpoints[j];
            //            }
            //            lue[3].pixel = outline;
            //            lue[2].pixel = outline;
            //            Edge[] ure = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                ure[j] = new Edge();
            //                ure[j].pixel = creeperSkin;
            //                ure[j].points = endpoints[j];
            //            }
            //            ure[2].pixel = outline;
            //            ure[1].pixel = outline;
            //            Edge[] rde = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                rde[j] = new Edge();
            //                rde[j].pixel = creeperSkin;
            //                rde[j].points = endpoints[j];
            //            }
            //            rde[1].pixel = outline;
            //            rde[0].pixel = outline;
            //            Edge[] dle = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                dle[j] = new Edge();
            //                dle[j].pixel = creeperSkin;
            //                dle[j].points = endpoints[j];
            //            }
            //            dle[0].pixel = outline;
            //            dle[3].pixel = outline;
            //            Edge[] fuckme = new Edge[4];
            //            for (int j = 0; j < 4; j++)
            //            {
            //                fuckme[j] = new Edge();
            //                fuckme[j].pixel = black;
            //                fuckme[j].points = endpoints[j];
            //            }
            //            fuckme[0].pixel = outline;

            //            Polygon[] creeperFace = new Polygon[64];
            //            string face = "" +
            //                "00000000" +
            //                "00000000" +
            //                "01100110" +
            //                "01100110" +
            //                "00011000" +
            //                "00111100" +
            //                "00111100" +
            //                "00100100";
            //            for (int i = 0; i < 8; i++)
            //            {
            //                for (int j = 0; j < 8; j++)
            //                {
            //                    Vector3[] pixel = new Vector3[4];
            //                    for (int k = 0; k < 4; k++)
            //                    {
            //                        pixel[k] = square[k] / 32 + new Vector3(1f / 16f * i - 3.5f / 16f, -0.25f, -1f / 16f * j + 3.5f / 16f);
            //                    }

            //                    Edge[] e = new Edge[0];
            //                    if (i == 0)
            //                    {
            //                        e = le;
            //                        if (j == 0)
            //                        {
            //                            e = lue;
            //                        }
            //                    }
            //                    else if (i == 7)
            //                    {
            //                        e = re;
            //                        if (j == 7)
            //                        {
            //                            e = rde;
            //                        }
            //                    }
            //                    else if (j == 0)
            //                    {
            //                        e = ue;
            //                        if (i == 7)
            //                        {
            //                            e = ure;
            //                        }
            //                    }
            //                    else if (j == 7)
            //                    {
            //                        e = de;
            //                        if (i == 0)
            //                        {
            //                            e = dle;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        e = face[j * 8 + i] == '0' ? edges : blackedges;
            //                    }

            //                    if ((i == 2 || i == 5) && j == 7)
            //                    {
            //                        e = fuckme;
            //                    }

            //                    creeperFace[j * 8 + i] = new Polygon(pixel, creeperSkin, face[j * 8 + i] == '0' ? creeperSkin : black);
            //                }
            //            }

            //            Polygon[] newMesh = new Polygon[5 + 64];
            //            for (int i = 0, j = 0; i < 6; i++)
            //            {
            //                if (i != 1)
            //                {
            //                    newMesh[j] = headMesh.polygons[i];
            //                    j++;
            //                }
            //            }
            //            for (int i = 5; i < 69; i++)
            //            {
            //                newMesh[i] = creeperFace[i - 5];
            //            }
            //            headMesh.polygons = newMesh;

            //            // BODY /////////////////////////////////////////////////////////
            //            Object body = new Object("body", new Vector3(0, 0, 0.75f));
            //            body.Cube(new Vector3(0.5f, 0.25f, 0.75f), creeperSides, outline);

            //            // LEG //////////////////////////////////////////////////////////
            //            Object leg0 = new Object("leg", new Vector3(0.125f, -0.25f, 0.1875f));
            //            leg0.Cube(new Vector3(0.25f, 0.375f, 0.25f), creeperSides, outline);
            //            /*Vector3[] legpixel0 = new Vector3[4];
            //            for (int i = 0; i < 4; i++)
            //            {
            //                legpixel0[i] = square[i] * 0.03125f + new Vector3(-0.09375f, -0.126f, -0.15625f);
            //            }
            //            Vector3[] legpixel1 = new Vector3[4];
            //            for (int i = 0; i < 4; i++)
            //            {
            //                legpixel1[i] = square[i] * 0.03125f + new Vector3(-0.03125f, -0.126f, -0.09375f);
            //            }
            //            Vector3[] legpixel2 = new Vector3[4];
            //            for (int i = 0; i < 4; i++)
            //            {
            //                legpixel2[i] = square[i] * 0.03125f + new Vector3(0.03125f, -0.126f, -0.15625f);
            //            }
            //            Vector3[] legpixel3 = new Vector3[4];
            //            for (int i = 0; i < 4; i++)
            //            {
            //                legpixel3[i] = square[i] * 0.03125f + new Vector3(0.09375f, -0.125f, -0.09375f);
            //            }*/

            //            /*Mesh legMesh = (Mesh)leg0.components[0];
            //            int lmPol = legMesh.polygons.Length;
            //            Array.Resize(ref legMesh.polygons, lmPol + 4);

            //            legMesh.polygons[lmPol] = new Polygon(legpixel0, edges, black, 1);
            //            legMesh.polygons[lmPol + 1] = new Polygon(legpixel1, edges, black, 1);
            //            legMesh.polygons[lmPol + 2] = new Polygon(legpixel2, edges, black, 1);
            //            legMesh.polygons[lmPol + 3] = new Polygon(legpixel3, edges, black, 1);*/

            //            Object leg1 = HolderClass.DeepClone(leg0);
            //            Object leg2 = HolderClass.DeepClone(leg0);
            //            Object leg3 = HolderClass.DeepClone(leg0);

            //            leg0.moved = true;
            //            leg1.moved = true;
            //            leg2.moved = true;
            //            leg3.moved = true;

            //            leg0.position = new Vector3(0.125f, -0.25f, 0.1875f);
            //            leg1.position = new Vector3(-0.125f, -0.25f, 0.1875f);
            //            leg2.position = new Vector3(0.125f, 0.25f, 0.1875f);
            //            leg3.position = new Vector3(-0.125f, 0.25f, 0.1875f);

            //            // Parent Object //////////////////////////////////////////////
            //            Object Creeper = new Object("Creeper", new Vector3(3, 1, 0.5f));
            //            Creeper.children = new Object[]
            //            {
            //                head,
            //                body,
            //                leg0,
            //                leg1,
            //                leg2,
            //                leg3
            //            };

            #endregion

            #region camera
            // camera //////////////////////////
            camera.Name = "camera";
            camera.position = new Vector3(0, -10, 0);
            camera.Rotation = Vector3.Zero();
            camera.scale = Vector3.FullOne();

            cam.viewingAngle = 0.5f;
            cam._object = camera;
            camCont._object = camera;
            #endregion

            List<string> linesList = new List<string>();
            using (StreamReader sr = new StreamReader(@"..\..\..\doom_E1M1.obj"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    linesList.Add(line);
                }
            }

            Object teapot = ObjImporter.Import(@"..\..\..\doom_E1M1.obj");
            teapot.scale = new Vector3(0.5f, 0.5f, 0.5f) * 2;
            Scene scene = new Scene(new Object[] { gb0, gb1, gb2, gb3, gb4, gb5, gb6, log, leaves0, leaves1, leaves2, leaves3, leaves4, sb, sb2/*, Creeper,/* teapot*/, camera });
            scene.mainCamera = cam;
            HolderClass.sw.Start();


            ///////////////////////////////////////////////////////////////////////////////////////////
            // Main Process ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
            Console.Clear();
            while (true)
            {
                System.Diagnostics.Debug.WriteLine("Started frame");
                HolderClass.sw.Restart();

                ConsoleKeyInfo key = HolderClass.key = new ConsoleKeyInfo();
                bool gotKey = false;

                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    gotKey = true;
                }

                if (gotKey)
                {
                    HolderClass.key = key;
                    if (HolderClass.key.Key == ConsoleKey.P)
                    {
                        HolderClass.debug = !HolderClass.debug;
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        scene.StartOwnMenu();
                    }
                }

                scene.Update();

                System.Diagnostics.Debug.WriteLine("Ended render: " + HolderClass.sw.Elapsed);
                System.Diagnostics.Debug.WriteLine("");

                System.Threading.Thread.Sleep(25);
                //Console.ReadLine();
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        static void EngagingBaseMenu()
        {
            string[] options = new string[]
            {
                "Choose scene",
                "Set scenes path",
                "Set prefabs path",
                "Controls help"
            };

            List<object> optionFns = new List<object>();

            Ref<string> scenesPath = new Ref<string>(HolderClass.scenesPath);
            Ref<string> prefabsPath = new Ref<string>(HolderClass.prefabsPath);

            optionFns.Add(new List<Scene>());
            optionFns.Add(scenesPath);
            optionFns.Add(prefabsPath);
            optionFns.Add("testing");

            while (true)
            {
                ListMenu<object> menu = new ListMenu<object>(new Ref<string>("Main menu"), options, optionFns);
                menu.EngageMenu();

                HolderClass.prefabsPath = prefabsPath.value;
                HolderClass.scenesPath = scenesPath.value;
            }
        }

        public static void MainCycle(Scene scene)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////
            // Main Process ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
            Console.Clear();
            scene.TheyMoved(scene.objects);
            scene.UpdateColors();
            HolderClass.mainLoop = true;

            while (HolderClass.mainLoop)
            {
                System.Diagnostics.Debug.WriteLine("Started frame");
                HolderClass.sw.Restart();

                ConsoleKeyInfo key = HolderClass.key = new ConsoleKeyInfo();
                bool gotKey = false;

                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    gotKey = true;
                }

                if (gotKey)
                {
                    HolderClass.key = key;
                    if (HolderClass.key.Key == ConsoleKey.P)
                    {
                        HolderClass.debug = !HolderClass.debug;
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        scene.StartOwnMenu();
                    }
                }

                scene.Update();

                System.Diagnostics.Debug.WriteLine("Ended render: " + HolderClass.sw.Elapsed);
                System.Diagnostics.Debug.WriteLine("");

                System.Threading.Thread.Sleep((int)Math.Max(25 - HolderClass.sw.Elapsed.TotalMilliseconds, 0));
                //Console.ReadLine();
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
        }
    }
}
