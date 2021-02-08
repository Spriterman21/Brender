using System;
using System.Collections.Generic;

namespace Brender_0_5
{
    public class Scene : IMenu
    {
        #region basic properties ////////////////////////////////////
        Ref<string> name = new Ref<string> { value = "test" };
        Ref<Object[]> objects = new Ref<Object[]> { value = new Object[0] };
        
        string Name
        {
            get { return name.value; }
            set { name.value = value; }
        }
        public Object[] Objects
        {
            get { return objects.value; }
            set { objects.value = value; }
        }
        public Camera mainCamera = new Camera();
        #endregion

        #region defining changable properties ///////////////////////
        readonly string[] options = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Objects",
            "Main Camera"
        };

        object[] optionFns = new object[3];

        void DefineChangables()
        {
            optionFns[0] = name;
            optionFns[1] = objects;
            optionFns[2] = mainCamera;
        }
        #endregion

        bool paused = true;

        public Scene(Object[] objects)
        {           
            Name = "scene";

            this.Objects = objects;

            foreach (Object _object in objects)
            {
                foreach (Component component in _object.Components)
                {
                    if (component is Camera camera)
                    {
                        mainCamera = camera;
                        goto loopExit;
                    }
                }
            }
            loopExit:;

            DefineChangables();

            Name = "fucking work";
        }

        public List<Component> components;

        public void UpdateObjects()
        {
            foreach (Object _object in Objects)
            {
                _object.Update(Vector3.Zero(), Quaternion.Euler(Vector3.Zero()), Vector3.FullOne());
            }
        }

        public void UpdateComponents()
        {
            HolderClass.polygons = new List<Polygon>();
            
            FindComponents();
            Type[] types = new Type[] { typeof(Controls), typeof(Mesh), typeof(Camera) };

            for (int i = 0; i < types.Length; i++)
            {
                for (int j = 0; j < components.Count; j++)
                {
                    if (components[j].GetType() == types[i])
                    {
                        components[j].Update();
                    }
                }
            }
        }


        public void FindComponents()
        {
            components = new List<Component>();
            foreach (Object _object in Objects)
            {
                foreach (Component component in _object.Components)
                {
                    components.Add(component);
                }
                foreach (Object child in _object.Children)
                {
                    ComponentsOfChildren(child);
                }
            }
        }

        public void ComponentsOfChildren(Object _object)
        {
            foreach (Component component in _object.Components)
            {
                components.Add(component);
            }
            foreach (Object child in _object.Children)
            {
                if (paused)
                {

                }
                
                ComponentsOfChildren(child);
            }
        }

        public void Update()
        {
            UpdateObjects();
            UpdateComponents();

            FTLConsole.Render(mainCamera.image);

            /*Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, 0);
            //Console.WriteLine("{0}, {1}, {2}", mainCamera._object.Rotation.x, mainCamera._object.Rotation.y, mainCamera._object.Rotation.z);*/
        }

        public bool StartOwnMenu()
        {
            Menu menu = new Menu(name, options, optionFns);
            menu.EngageMenu();

            TheyMoved(Objects);

            return false;
        }

        // the position of objects would not be recalculated after changing in the pause menu, seting individual objects to recalculate seems
        // due to the nature of pause menu difficult, so lets set them all as if they have been moved
        void TheyMoved(Object[] objs)
        {
            foreach (Object obj in objs)
            {
                obj.moved = true;
                TheyMoved(obj.Children);
            }
        }
    }
}
