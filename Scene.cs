using System;
using System.Collections.Generic;

namespace Brender_0_5
{
    [Serializable]
    public class Scene : IMenu, ICreatable
    {
        #region basic properties ////////////////////////////////////
        public Ref<string> name = new Ref<string> { value = "test" };
        public Object[] objects = new Object[0];
        public Camera mainCamera;
        public CustomColor[] colors;

        public string Name
        {
            get { return name.value; }
            set { name.value = value; }
        }
        #endregion

        public Scene(Object[] objects)
        {           
            Name = "scene";

            this.objects = objects;

            foreach (Object _object in objects)
            {
                foreach (Component component in _object.components)
                {
                    if (component is Camera camera)
                    {
                        mainCamera = camera;
                        goto loopExit;
                    }
                }
            }
            loopExit:;

            Name = "Name";

            colors = new CustomColor[16];
            for (int i = 0; i < 16; i++)
            {
                colors[i] = new CustomColor(i);
                colors[i].SetBaseColor();
            }
        }

        public List<Component> components;

        /// <summary>
        /// Makes objects update their coordinates
        /// </summary>
        public void UpdateObjects()
        {
            foreach (Object _object in objects)
            {
                _object.Update(Vector3.Zero(), Quaternion.Euler(Vector3.Zero()), Vector3.FullOne());
            }
        }

        /// <summary>
        /// Updates components in the desired order
        /// </summary>
        public void UpdateComponents()
        {
            HolderClass.polygons = new List<Polygon>();
            
            FindComponents(); // finds all components
            Type[] types = new Type[] { typeof(Controls), typeof(Mesh), typeof(Camera) }; // the order in which components will be updated

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
            foreach (Object _object in objects)
            {
                foreach (Component component in _object.components)
                {
                    components.Add(component);
                    if (component._object == null)
                    {
                        component._object = _object;
                    }
                }
                foreach (Object child in _object.children)
                {
                    ComponentsOfChildren(child);
                }
            }
        }

        public void ComponentsOfChildren(Object _object)
        {
            foreach (Component component in _object.components)
            {
                components.Add(component);
                if (component._object == null)
                {
                    component._object = _object;
                }
            }
            foreach (Object child in _object.children)
            {
                ComponentsOfChildren(child);
            }
        }

        public void Update()
        {
            UpdateObjects();
            UpdateComponents();

            if (mainCamera != null)
            {
                FTLConsole.Render(mainCamera.image);
            }

            /*Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, 0);
            //Console.WriteLine("{0}, {1}, {2}", mainCamera._object.Rotation.x, mainCamera._object.Rotation.y, mainCamera._object.Rotation.z);*/
        }


        #region defining changable properties ///////////////////////
        static readonly string[] options = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "objects",
            "Main Camera",
            "Color palette",
            "SAVE",
            "EXIT"
        };

        public void StartOwnMenu()
        {
            // creating option functions
            List<object> optionFns = new List<object>();
            
            List<Object> obj = new List<Object>();
            for (int i = 0; i < objects.Length; i++)
            {
                obj.Add(objects[i]);
            }

            List<CustomColor> listColors = new List<CustomColor>(colors);
            
            optionFns.Add(name);
            optionFns.Add(obj);
            optionFns.Add(mainCamera);
            optionFns.Add(listColors);
            optionFns.Add(new Saver(this));
            optionFns.Add(new SceneExit());
            
            // starting menu
            ListMenu<object> menu = new ListMenu<object>(name, options, optionFns);
            menu.EngageMenu();

            // user might have chanched stuff; would be hard to figure out exactly what so just update everything
            TheyMoved(objects);

            // restoring some possibly changed variables back to this class
            objects = obj.ToArray();
            colors = listColors.ToArray();
        }
        #endregion

        // the position of objects would not be recalculated after changing in the pause menu, seting individual objects to recalculate seems
        // due to the nature of pause menu difficult, so lets set them all as if they have been moved
        public void TheyMoved(Object[] objs)
        {
            foreach (Object obj in objs)
            {
                obj.moved = true;
                TheyMoved(obj.children);
            }
        }

        public void UpdateColors()
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].UpdateColor();
            }
        }
    }

    #region Planned for main camera changing
    /*
    /// <summary>
    /// I need this separated because I don't want to search through all objects to find cameras immediately when I pause
    /// </summary>
    public class MainCompModifier<T> where T : Component
    {
        Scene scene;
        
        List<Ref<T>> comp = null; //I also need to put these in the Ref class to differentiate them from normal cameras in the overloaded f'ns in the menu
        List<string> names = null;
        
        public T SearchForComponents()
        {
            comp = new List<Ref<T>>();
            names = new List<string>();

            CheckChildren(scene.objects);

            ListMenu<T> menu = new ListMenu<T>("Choose main component", names.ToArray(), comp.ToArray());
            menu.returns = true;
            menu.EngageMenu();

            return (T)new Component();
        }

        /// <summary>
        /// Simply finds components and names of such objects that have them (children included)
        /// </summary>
        /// <param name="objects"></param>
        void CheckChildren(Object[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                try
                {
                    comp.Add(new Ref<T> { value = objects[i].GetComponent<T>() });
                    names.Add(objects[i].Name);
                }
                catch (MissingMemberException)
                {
                }

                CheckChildren(objects[i].children);
            }
        }
    }*/
    #endregion

    public class SceneExit
    {

    }
}
