using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Brender_0_5
{
    // this is pretty much just a fancier (or maybe uglier) pointer, since c# doesn't have those
    [Serializable]
    public class Ref<T> /*where T : struct*/ //https://stackoverflow.com/questions/13371433/possible-to-store-references-to-objects-in-list
    {
        public Ref()
        {

        }

        public Ref(T initialValue)
        {
            value = initialValue;
        }

        public T value { get; set; }
    }

    public class ListMenu<T>
    {
        Ref<string> title = new Ref<string>(); // will be highlighted as the name of the menu
        string Title
        {
            get { return title.value; }
            set { title.value = value; }
        }

        List<Ref<string>> options = new List<Ref<string>>();
        List<T> changables; // array of different kinds of variables coresponding to their names in _options_
        int alignment = 0; // the topmost index shown on the screen (for when you want to scroll through the list, but your window is too small)

        public bool canAddDelete = true; // meaning you can delete or add stuff in the menu
        bool returns = false;

        #region Menu constructors
        public ListMenu(Ref<string> t, string[] o, List<T> c)
        {
            title = t;
            options = new List<Ref<string>>();
            for (int i = 0; i < o.Length; i++)
            {
                options.Add(new Ref<string> { value = o[i] });
            }
            changables = c;
            CanAddDelete();
        }

        public ListMenu(string t, Ref<string>[] o, List<T> c)
        {
            Title = t;
            options = new List<Ref<string>>(o);
            changables = c;
            CanAddDelete();
        }

        public ListMenu(string t, string[] o)
        {
            Title = t;
            options = new List<Ref<string>>();
            for (int i = 0; i < o.Length; i++)
            {
                options.Add(new Ref<string> { value = o[i] });
            }

            returns = true;
        }

        void CanAddDelete()
        {
            canAddDelete = canAddDelete || !(typeof(T) is object);
        }
        #endregion

        #region The core function of the menu
        public int EngageMenu()
        {
            Console.Clear();

            int index = 0;
            alignment = 0;
            bool stay = true;

            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < options.Count + 1; i++)
            {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }

            Redraw(Title, options.ToArray(), alignment, index);

            while (stay)
            {

                bool gotKey = false;
                ConsoleKeyInfo key = new ConsoleKeyInfo();

                while (Console.KeyAvailable)
                {
                    key = Console.ReadKey(true);
                    gotKey = true;
                }

                if (gotKey)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow: // going up the list
                        case ConsoleKey.W:
                            if (index == 0)
                            {
                                index = options.Count - 1;
                                break;
                            }
                            index--;
                            break;
                        case ConsoleKey.DownArrow: // going down the list
                        case ConsoleKey.S:
                            if (index == options.Count - 1)
                            {
                                index = 0;
                                break;
                            }
                            index++;
                            break;
                        case ConsoleKey.Enter: // going deeper
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.D:
                            if(returns)
                            {
                                return index;
                            }
                            else
                            {
                                MenuNecesities.DoThings((dynamic)changables[index]); // dynamically chooses what to do, with the selected option
                                Console.Clear();
                                break;
                            }
                        case ConsoleKey.Escape: // leaving menu
                        case ConsoleKey.LeftArrow:
                            stay = false;

                            break;
                        case ConsoleKey.Delete: // deleting object if possible
                            if (canAddDelete)
                            {
                                changables.RemoveAt(index);
                                options.RemoveAt(index);

                                index = Math.Max(index - 1, 0);
                                Console.Clear();
                            }
                            break;
                        case ConsoleKey.Add: // adding object if possible
                            if (canAddDelete)
                            {
                                Add();
                                Console.Clear();
                            }
                            break;
                        default:
                            break;
                    }

                    for (int i = 0; i < options.Count + 1; i++)
                    {
                        Console.WriteLine(new string(' ', Console.BufferWidth));
                    }

                    Redraw(Title, options.ToArray(), alignment, index);
                }
            }
            Console.Clear();
            return -1;
        }
        #endregion

        #region drawing necesities
        void Redraw(string title, Ref<string>[] options, int alignment, int index)
        {
            Console.SetCursorPosition(0, 0);

            int height = Math.Min(Console.BufferHeight, Console.WindowHeight);
            int width = Math.Min(Console.BufferWidth, Console.WindowWidth);

            //if cant even display title and one option, just dont try
            if (height < 2)
            {
                Console.Clear();
                try // what if its too small to draw anything
                {
                    Console.WriteLine(":(");
                }
                catch (Exception)
                {
                }
                return;
            }

            int usableLines = height - 1; //number of lines for options (top subtracted)

            if (index < alignment) // aligning on the highlighted option
            {
                alignment = index;
            }
            else if (alignment < index && index - alignment >= usableLines)
            {
                alignment = Math.Max(index - usableLines + 1, 0);
            }

            // title
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(PrepareString(title, width));

            // writing out options
            int endPoint = Math.Min(alignment + usableLines, options.Length);
            for (int i = alignment; i < endPoint; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write("\n" + PrepareString(options[i].value, width));
            }

            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;


        }

        static string PrepareString(string s, int width)
        {
            if (width < 3) // dont even try
            {
                return "";
            }

            if (s.Length > width) // make it fit and show the name isn't full
            {
                s = s.Substring(0, Math.Max(width - 3, 0));
                return s + "...";
            }
            else // center it
            {
                double spaces = width - s.Length;
                return new string(' ', (int)(spaces / 2)) + s + new string(' ', (int)Math.Ceiling(spaces / 2));
            }
        }
        #endregion


        void Add()
        {
            Tuple<ICreatable, Ref<string>> newElement = null;

            if (changables is List<Object>) newElement = new ObjectFactory().Create();
            else if (changables is List<Component>) newElement = new ComponentFactory().Create();
            else if (changables is List<Polygon>) newElement = new PolygonFactory().Create();
            else if (changables is List<Vector3>) newElement = new Vector3Factory().Create();
            else if (changables is List<Scene>) newElement = new SceneFactory().Create();
            
            if (newElement != null && newElement.Item1 != null)
            {
                changables.Add((T)newElement.Item1);
                options.Add(newElement.Item2);
            }
        }
    }



    /// <summary>
    /// Functions necessary for all the menus for drawing and handling chosen options
    /// </summary>
    public static class MenuNecesities
    {
        /// <summary>
        /// draws the menu centered on the screen
        /// </summary>
        /// <param name="title"></param>
        /// <param name="options"></param>
        /// <param name="alignment"></param>
        /// <param name="index"></param>

        /// <summary>
        /// Circumcises the string to fit on the screen, no matter the resolution (or at least hopefully,
        /// might have some errors in some extreme cases; hopefully user is not an idiot)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <returns></returns>


        /* this is a series of overloaded functions, one of which will be dynamically chosen based on the selected option in the menu
 * since the chosen option can lead to multiple things, like changing a string (for ex name), changing a float or starting
 * a completely new choosing menu, it is necesary for this to be as dynamic and expandable as possible, therefore for every type
 * possible it is only needed to add a new overload of a method and the operations following the choosing of such option
 * will be done in that new method
 * (let's face it, it's an ugly way to do it and I don't like it that much, but other methods would requier much more learning
 * and much more time being spent and probably would've ended just as a code coppied, modified and sourced from stackoverflow)
 */
        #region End variables changes
        /// <summary>
        /// Method to change ints
        /// </summary>
        /// <param name="value"></param>
        public static void DoThings(Ref<int> i)
        {
            Console.Clear();
            Console.WriteLine("old int:" + i.value);
            Console.WriteLine("new int:");
            int output;
            while (!int.TryParse(Console.ReadLine(), out output))
            {
                Console.Clear();
                Console.WriteLine("invalid input");
                Console.WriteLine("old int:" + i.value);
                Console.WriteLine("new int:");
            }

            i.value = output;
        }

        /// <summary>
        /// method to change floats
        /// </summary>
        /// <param name="f"></param>
        public static void DoThings(Ref<float> f)
        {
            Console.Clear();
            Console.WriteLine("old f:" + f.value);
            Console.WriteLine("new f:");
            float output;
            while (!float.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out output))
            {
                Console.Clear();
                Console.WriteLine("invalid input");
                Console.WriteLine("old f:" + f.value);
                Console.WriteLine("new f:");
            }

            f.value = output;
        }

        /// <summary>
        /// method to change strings
        /// </summary>
        /// <param name="s"></param>
        public static void DoThings(Ref<string> s)
        {
            Console.Clear();
            Console.WriteLine("Old string:");
            Console.WriteLine(s.value);
            Console.WriteLine("New string:");
            s.value = Console.ReadLine();
        }

        /// <summary>
        /// method to change CharInfo
        /// </summary>
        /// <param name="s"></param>
        public static void DoThings(Ref<CharInfo> s)
        {
            Console.Clear();
            Console.WriteLine("Choose a new char:");
            char newChar = Console.ReadKey().KeyChar;

            Console.Clear();
            Console.WriteLine("Choose new background colour:");
            int background;
            while (!int.TryParse(Console.ReadLine(), out background))
            {
                Console.Clear();
                Console.WriteLine("Invalid input");
                Console.WriteLine("Choose new background colour:");
            }

            Console.Clear();
            Console.WriteLine("Choose new foreground colour:");
            int foreground;
            while (!int.TryParse(Console.ReadLine(), out foreground))
            {
                Console.Clear();
                Console.WriteLine("Invalid input");
                Console.WriteLine("Choose new foreground colour:");
            }

            s.value = new CharInfo(newChar, (short)(foreground % 16), (short)(background % 16));
        }


        #endregion

        #region Variables leeding deeper
        public static void DoThings(Ref<quaternion> quaternion)
        {
            Vector3 euler = Quaternion.QuatToEuler(quaternion.value, false);
            DoThings(euler);
            quaternion.value = Quaternion.Euler(euler, false);
        }

        public static void DoThings(IMenu menuStarter)
        {
            menuStarter.StartOwnMenu();
        }

        public static void DoThings(Scene scene)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////
            // Main Process ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
            Console.Clear();
            scene.TheyMoved(scene.objects);
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

                System.Threading.Thread.Sleep((int)Math.Max(25 - HolderClass.sw.Elapsed.TotalMilliseconds, 0));
                //Console.ReadLine();
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////
        }
        #endregion

        #region ListMenu creators
        /// <summary>
        /// For when you wanna choose from a list of objects
        /// </summary>
        /// <param name="objects"></param>
        public static void DoThings(List<Object> objects)
        {
            string title = "Parented objects";

            Ref<string>[] names = new Ref<string>[objects.Count];
            for (int i = 0; i < objects.Count; i++)
            {
                names[i] = objects[i].name;
            }

            // just starting the menu
            ListMenu<Object> menu = new ListMenu<Object>(title, names, objects);
            menu.canAddDelete = true;
            menu.EngageMenu();
        }

        public static void DoThings(List<Component> components)
        {
            string title = "Components";

            Ref<string>[] names = new Ref<string>[components.Count];
            for (int i = 0; i < components.Count; i++)
            {
                names[i] = components[i].name;
            }

            // just starting the menu
            ListMenu<Component> menu = new ListMenu<Component>(title, names, components);
            menu.canAddDelete = true;
            menu.EngageMenu();
        }

        public static void DoThings(List<Polygon> polygons)
        {
            string title = "Polygons";

            Ref<string>[] names = new Ref<string>[polygons.Count];
            for (int i = 0; i < polygons.Count; i++)
            {
                names[i] = polygons[i].name;
            }

            // just starting the menu
            ListMenu<Polygon> menu = new ListMenu<Polygon>(title, names, polygons);
            menu.canAddDelete = true;
            menu.EngageMenu();
        }

        public static void DoThings(List<Vector3> vector3s)
        {
            string title = "Points";

            Ref<string>[] names = new Ref<string>[vector3s.Count];
            for (int i = 0; i < vector3s.Count; i++)
            {
                names[i] = vector3s[i].name;
            }

            // just starting the menu
            ListMenu<Vector3> menu = new ListMenu<Vector3>(title, names, vector3s);
            menu.canAddDelete = true;
            menu.EngageMenu();
        }

        public static void DoThings(List<Scene> scenes)
        {
            string title = "Scenes";
            Ref<string>[] names;

            // find scenes in their folder
            string[] scenePaths = HolderClass.FileNames(HolderClass.scenesPath);
            scenes.Clear();

            // get scene names
            names = new Ref<string>[scenePaths.Length];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenes.Add(HolderClass.Loader<Scene>(HolderClass.scenesPath + "\\" + scenePaths[i]));
                names[i] = scenes[i].name;
            }

            // backup current state of scenes
            Scene[] backup = scenes.ToArray();

            // start menu
            ListMenu<Scene> menu = new ListMenu<Scene>(title, names, scenes);
            menu.EngageMenu();

            // for each backupped scene, check if it still exists, if not, delete its file too
            for (int i = 0; i < backup.Length; i++)
            {
                if (!scenes.Contains(backup[i]) && File.Exists(HolderClass.scenesPath + "\\" + scenePaths[i]))
                {
                    Debug.WriteLine("Deleting file " + HolderClass.scenesPath + "\\" + scenePaths[i]);
                    File.Delete(HolderClass.scenesPath + "\\" + scenePaths[i]);
                }
            }
        }

        #endregion

        public static void DoThings(Saver saver)
        {
            saver.Save();
        }
    }
}
