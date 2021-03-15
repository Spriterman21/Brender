using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Brender_0_5
{
    /// <summary>
    /// A basic object placable in the scene
    /// It is possible to attach multiple components to it
    /// </summary>
    [Serializable()]
    public class Object : IMenu, ICreatable
    {
        #region basic properties ////////////////////////////////////////////

        public Ref<string> name = new Ref<string>();
        public Vector3 position = new Vector3();
        Ref<quaternion> quatRotation = new Ref<quaternion>();
        public Vector3 scale = new Vector3();
        public Object[] children = new Object[0];
        public Component[] components = new Component[0];

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

        [NonSerialized]
        public bool moved = true;
        #endregion

        // runtime coordinates in sceene inertial frame of reference
        [NonSerialized]
        public Vector3 globalPosition = new Vector3();
        [NonSerialized]
        public quaternion globalRotation = new quaternion();
        [NonSerialized]
        public Vector3 globalScale = new Vector3();

        #region some ways to declare instances
        public Object()
        {
            Name = "name";
            position = Vector3.Zero();
            Rotation = Vector3.Zero();
            scale = Vector3.FullOne();
        }

        public Object(Component[] components)
        {
            Name = "name";
            position = Vector3.Zero();
            Rotation = Vector3.Zero();
            scale = Vector3.FullOne();
            
            this.components = components;
            foreach (Component component in this.components)
            {
                component._object = this;
            }
        }

        public Object(string name, Vector3 pos)
        {
            Name = name;
            position = pos;
            Rotation = Vector3.Zero();
            scale = Vector3.FullOne();
        }
        #endregion

        public void Update(Vector3 parentGP, quaternion parentGR, Vector3 parentGS, bool parentMoved = false)
        {
            moved = parentMoved || moved;

            if (moved)
            {
                globalPosition = parentGP + Quaternion.RotatePoint(parentGR, position & parentGS);
                globalRotation = parentGR * QuatRotation;
                globalScale = scale & parentGS;
            }

            //Console.WriteLine(name);
            //Console.WriteLine("{0},  {1},  {2}", globalPosition.x, globalPosition.y, globalPosition.z);
            //Console.WriteLine("{0},  {1},  {2},  {3}", globalRotation.w, globalRotation.vector.x, globalRotation.vector.y, globalRotation.vector.z);
            //Console.WriteLine("{0},  {1},  {2}", globalScale.x, globalScale.y, globalScale.z);

            foreach (Object child in children)
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
                new Vector3(-size.x, -size.y, -size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(size.x, size.y, -size.z),
                new Vector3(-size.x, size.y, -size.z),
                new Vector3(-size.x, -size.y, size.z),
                new Vector3(size.x, -size.y, size.z),
                new Vector3(size.x, size.y, size.z),
                new Vector3(-size.x, size.y, size.z)
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

            Component[] newComponents = new Component[components.Length + 1];
            Array.Copy(components, newComponents, components.Length);

            newComponents[newComponents.Length - 1] = new Mesh(polygons);
            newComponents[newComponents.Length - 1]._object = this;

            components = newComponents;
        }

        #region defining menu option and their names ///////////////////////
        static readonly string[] names = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Position",
            "Rotation",
            "Scale",
            "Components",
            "Children",
            "Create prefab"
        };

        public void StartOwnMenu()
        {
            // preparing function for each f'n name
            List<object> optionFns = new List<object>();

            
            List<Object> obj = new List<Object>();
            for (int i = 0; i < children.Length; i++)
            {
                obj.Add(children[i]);
            }

            List<Component> comp = new List<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                comp.Add(components[i]);
            }

            optionFns.Add(name);
            optionFns.Add(position);
            optionFns.Add(quatRotation);
            optionFns.Add(scale);
            optionFns.Add(comp);
            optionFns.Add(obj);
            optionFns.Add(new Saver(this));

            // creating a menu
            ListMenu<object> menu = new ListMenu<object>(name, names, optionFns);
            menu.EngageMenu();

            // restoring some possibly changed variables back to this class
            children = obj.ToArray();
            components = comp.ToArray();
        }
        #endregion

        public X GetComponent<X>() where X : Component
        {
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is X)
                {
                    return (X)components[i];
                }
            }

            throw new MissingMemberException();
        }
    }

}
