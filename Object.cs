using System;
using System.Collections.Generic;
using System.Text;


namespace Brender_0_5
{
    /// <summary>
    /// A basic object placable in the scene
    /// It is possible to attach multiple components to it
    /// </summary>
    [Serializable()]
    public class Object : IMenu
    {
        #region basic properties ////////////////////////////////////////////

        public Ref<string> name = new Ref<string>();
        Ref<Vector3> position = new Ref<Vector3>();
        Ref<quaternion> quatRotation = new Ref<quaternion>();
        Ref<Vector3> scale = new Ref<Vector3>();
        Ref<Object[]> children = new Ref<Object[]> { value = new Object[0] };
        Ref<Component[]> components = new Ref<Component[]> { value = new Component[0] };

        public string Name
        {
            get
            {
                return name.value;
            }
            set
            {
                name.value = value;
            }
        }
        public Vector3 Position
        {
            get { return position.value; }
            set { position.value = value; }
        }
        public quaternion QuatRotation
        {
            get { return quatRotation.value; }
            set { quatRotation.value = value; }
        }
        public Vector3 Rotation
        {
            get
            {
                return Quaternion.QuatToEuler(quatRotation.value, false);
            }
            set
            {
                quatRotation.value = Quaternion.Euler(value, false);
            }
        }
        public Vector3 Scale
        {
            get { return scale.value; }
            set { scale.value = value; }
        }

        public Object[] Children
        {
            get { return children.value; }
            set { children.value = value; }
        }
        public Component[] Components
        {
            get { return components.value; }
            set { components.value = value; }
        }

        [NonSerialized]
        public bool moved = true;
        #endregion

        #region defining changable properties ///////////////////////
        readonly string[] names = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Position",
            "Rotation",
            "Scale",
            "Components",
            "Children"
        };

        object[] variables = new object[6];

        void DefineChangables()
        {
            variables[0] = name;
            variables[1] = position;
            variables[2] = quatRotation;
            variables[3] = scale;
            variables[4] = Components;
            variables[5] = Children;
        }
        #endregion

        // runtime coordinates in sceene inertial frame of reference
        [NonSerialized]
        public Vector3 globalPosition;
        [NonSerialized]
        public quaternion globalRotation;
        [NonSerialized]
        public Vector3 globalScale;

        #region some ways to declare instances
        public Object()
        {
            DefineChangables();
        }

        public Object(Component[] components)
        {
            DefineChangables();
            
            this.Name = "name";
            this.Position = Vector3.Zero();
            Rotation = Vector3.Zero();
            Scale = Vector3.FullOne();
            
            this.Components = components;
            foreach (Component component in this.Components)
            {
                component._object = this;
            }
        }

        public Object(string name, Vector3 position)
        {
            DefineChangables();
            
            this.Name = name;
            this.Position = position;
            Rotation = Vector3.Zero();
            Scale = Vector3.FullOne();
        }
        #endregion

        public void Update(Vector3 parentGP, quaternion parentGR, Vector3 parentGS, bool parentMoved = false)
        {
            moved = parentMoved || moved;

            if (moved)
            {
                globalPosition = parentGP + Quaternion.RotatePoint(parentGR, Position & parentGS);
                globalRotation = parentGR * QuatRotation;
                globalScale = Scale & parentGS;
            }

            //Console.WriteLine(name);
            //Console.WriteLine("{0},  {1},  {2}", globalPosition.x, globalPosition.y, globalPosition.z);
            //Console.WriteLine("{0},  {1},  {2},  {3}", globalRotation.w, globalRotation.vector.x, globalRotation.vector.y, globalRotation.vector.z);
            //Console.WriteLine("{0},  {1},  {2}", globalScale.x, globalScale.y, globalScale.z);

            foreach (Object child in Children)
            {
                child.Update(globalPosition, globalRotation, globalScale, moved);
            }

            moved = false;
        }

        /// <summary>
        /// Creates a cube, sort of like prefab, but work in progress
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sides"></param>
        /// <param name="outline"></param>
        public void Cube(Vector3 size, CharInfo[] sides, CharInfo outline)
        {
            size /= 2;

            Vector3[] points = new Vector3[8]
            {
                new Vector3(-size.X, -size.Y, -size.Z),
                new Vector3(size.X, -size.Y, -size.Z),
                new Vector3(size.X, size.Y, -size.Z),
                new Vector3(-size.X, size.Y, -size.Z),
                new Vector3(-size.X, -size.Y, size.Z),
                new Vector3(size.X, -size.Y, size.Z),
                new Vector3(size.X, size.Y, size.Z),
                new Vector3(-size.X, size.Y, size.Z)
            };

            int[][] pointOrder = new int[6][]
            {
                new int[] {0, 3, 2, 1},
                new int[] {0, 1, 5, 4},
                new int[] {1, 2, 6, 5},
                new int[] {2, 3, 7, 6},
                new int[] {3, 0, 4, 7},
                new int[] {4, 5, 6, 7}
            };


            int[][] endpoints = new int[4][]
            {
                new int[]{ 0, 1},
                new int[]{ 1, 2},
                new int[]{ 2, 3},
                new int[]{ 3, 0}
            };
            Polygon[] polygons = new Polygon[6];

            for (int i = 0; i < 6; i++)
            {
                Vector3[] localPoints = new Vector3[]
                {
                    points[pointOrder[i][0]],
                    points[pointOrder[i][1]],
                    points[pointOrder[i][2]],
                    points[pointOrder[i][3]]
                };

                polygons[i] = new Polygon(localPoints, outline, sides[i]);
            }

            Component[] newComponents = new Component[Components.Length + 1];
            Array.Copy(Components, newComponents, Components.Length);

            newComponents[newComponents.Length - 1] = new Mesh(polygons);
            newComponents[newComponents.Length - 1]._object = this;

            Components = newComponents;
        }

        public bool StartOwnMenu()
        {
            Menu menu = new Menu(name, names, variables);
            menu.EngageMenu();
            return false;
        }
    }

}
