using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace Brender_0_5
{
    // this is pretty much just a fancier (or maybe uglier) pointer, since c# doesn't have those
    [Serializable]
    public class Ref<T> /*where T : struct*/ //https://stackoverflow.com/questions/13371433/possible-to-store-references-to-objects-in-list
    {
        public T value { get; set; }
    }

    public class Menu // for creating a menu where you can choose from multiple options
    {
        Ref<string> title = new Ref<string>(); // will be highlighted as the name of the menu
        Ref<string>[] options = new Ref<string>[0];

        string Title
        {
            get { return title.value; }
            set { title.value = value; }
        }
        object[] changables; // array of different kinds of variables coresponding to their names in _options_
        int alignment = 0; // the topmost index shown on the screen (for when you want to scroll through the list, but your window is too small)


        public Menu(Ref<string> t, Ref<string>[] o, object[] c)
        {
            title = t;
            options = o;
            changables = c;
        }

        public Menu(Ref<string> t, string[] o, object[] c)
        {
            title = t;
            options = new Ref<string>[o.Length];
            for (int i = 0; i < o.Length; i++)
            {
                options[i] = new Ref<string> { value = o[i] };
            }
            changables = c;
        }

        public Menu(string t, Ref<string>[] o, object[] c)
        {
            Title = t;
            options = o;
            changables = c;
        }

        public Menu(string t, string[] o, object[] c)
        {
            Title = t;
            options = new Ref<string>[o.Length];
            for (int i = 0; i < o.Length; i++)
            {
                options[i] = new Ref<string> { value = o[i] };
            }
            changables = c;
        }


        public void EngageMenu(bool canDelete = false)
        {
            Console.Clear();

            int index = 0;
            alignment = 0;
            bool stay = true;

            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < options.Length + 1; i++)
            {
                Console.WriteLine(new string(' ', Console.BufferWidth));
            }

            Redraw(index);

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
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.W:
                            if (index == 0)
                            {
                                index = options.Length - 1;
                                break;
                            }
                            index--;
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.S:
                            if (index == options.Length - 1)
                            {
                                index = 0;
                                break;
                            }
                            index++;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.D:
                            Debug.WriteLine(changables[index]);
                            DoThings((dynamic)changables[index]); // dynamically chooses what to do, with the selected option
                            Console.Clear();
                            break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.LeftArrow:
                            stay = false;
                            break;
                        case ConsoleKey.Delete:
                            break;
                        default:
                            break;
                    }

                    Console.SetCursorPosition(0, 0);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < options.Length + 1; i++)
                    {
                        Console.WriteLine(new string(' ', Console.BufferWidth));
                    }

                    Redraw(index);
                }
            }
            Console.Clear();
        }

        #region Necessities for menu drawing ///////////////////////////////////////////////
        void Redraw(int index)
        {
            Console.SetCursorPosition(0, 0);
            
            int height = Math.Min(Console.BufferHeight, Console.WindowHeight);
            int width = Math.Min(Console.BufferWidth, Console.WindowWidth);

            if (height < 2)
            {
                Console.Clear();
                Console.WriteLine(":(");
                return;
            }

            int usableLines = height - 1;

            if (index < alignment)
            {
                alignment = index;
            }
            else if (alignment < index && index - alignment >= usableLines)
            {
                alignment = Math.Max(index - usableLines + 1, 0);
            }

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(PrepareString(Title, width));

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
        }

        /// <summary>
        /// Circumcises the string to fit on the screen, no matter the resolution (or at least hopefully,
        /// might have some errors in some extreme cases; hopefully user is not an idiot)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        string PrepareString(string s, int width)
        {
            if (width < 3)
            {
                return "";
            }

            if (s.Length > width)
            {
                s = s.Substring(0, Math.Max(width - 3, 0));
                return s + "...";
            }
            else
            {
                double spaces = width - s.Length;
                return new string(' ', (int)(spaces / 2)) + s + new string(' ', (int)Math.Ceiling(spaces / 2));
            }
        }
        #endregion


        #region Dynamic option's handlerer /////////////////////////////////////////////////
        /* this is a series of overloaded functions, one of which will be dynamically chosen based on the selected option in the menu
         * since the chosen option can lead to multiple things, like changing a string (for ex name), changing a float or starting
         * a completely new choosing menu, it is necesary for this to be as dynamic and expandable as possible, therefore for every type
         * possible it is only needed to add a new overload of a method and the operations following the choosing of such option
         * will be done in that new method
         */

        /// <summary>
        /// Method to change ints
        /// </summary>
        /// <param name="value"></param>
        void DoThings(Ref<int> i)
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
        void DoThings(Ref<float> f)
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

            Debug.WriteLine(output);
            f.value = output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        void DoThings(Ref<string> s)
        {
            Console.Clear();
            Console.WriteLine("Old name:");
            Console.WriteLine(s.value);
            Console.WriteLine("New name:");
            s.value = Console.ReadLine();
        }

        /// <summary>
        /// for when you want to see and alter components of an object
        /// </summary>
        /// <param name="components"></param>
        void DoThings(Ref<Component[]> components)
        {
            string title = "Components";
            List<string> compNames = new List<string>();
            foreach (Component component in components.value)
            {
                compNames.Add(component.name);
            }

            Menu menu = new Menu(title, compNames.ToArray(), components.value);
            menu.EngageMenu();
        }


        /// <summary>
        /// For when you've chosen a component and want to change it somehow
        /// </summary>
        /// <param name="component"></param>
        void DoThings(Component component)
        {
            component.StartOwnMenu();
        }

        /// <summary>
        /// For when you wanna choose from a list of objects
        /// </summary>
        /// <param name="objects"></param>
        void DoThings(Ref<Object[]> objects)
        {
            string title = "Children";
            List<Ref<string>> chNames = new List<Ref<string>>();
            foreach (Object obj in objects.value)
            {
                chNames.Add(obj.name);
            }

            Menu menu = new Menu(title, chNames.ToArray(), objects.value);
            menu.EngageMenu();
        }

        /// <summary>
        /// for when you've chosen an object and want to alter what's inside
        /// </summary>
        /// <param name="obj"></param>
        void DoThings(Object obj)
        {
            obj.StartOwnMenu();
        }

        void DoThings(Ref<Vector3> vector3)
        {
            vector3.value.StartOwnMenu();
        }

        void DoThings(Ref<quaternion> quaternion)
        {
            Vector3 euler = Quaternion.QuatToEuler(quaternion.value, false);
            DoThings(new Ref<Vector3> { value = euler });
            quaternion.value = Quaternion.Euler(euler, false);
        }
        #endregion
        
        #region remnants
        /*
        public static void ChangeString(ref string s)
        {
            Console.Clear();
            Console.WriteLine("Old name:");
            Console.WriteLine(s);
            Console.WriteLine("New name:");
            s = Console.ReadLine();
        }

        public static void ChangeFloat(ref float f)
        {
            Console.Clear();
            Console.WriteLine("old f:" + f);
            Console.WriteLine("new f:");
            float output;
            while (!float.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out output))
            {
                Console.Clear();
                Console.WriteLine("invalid input");
                Console.WriteLine("old f:" + f);
                Console.WriteLine("new f:");
            }

            Debug.WriteLine(output);
            f = output;
        }

        public static void ChangeInt(ref int i)
        {
            Console.Clear();
            Console.WriteLine("old int:" + i);
            Console.WriteLine("new int:");
            int output;
            while (!int.TryParse(Console.ReadLine(), out output))
            {
                Console.Clear();
                Console.WriteLine("invalid input");
                Console.WriteLine("old int:" + i);
                Console.WriteLine("new int:");
            }

            i = output;
        }

        public static void MenuForObjects(Object[] objects)
        {
            string title = "Children";
            List<string> chNames = new List<string>();
            foreach (Object obj in objects)
            {
                chNames.Add(obj.Name);
            }

            int chosen = 0;
            while(true)
            {
                chosen = Menu.EngageMenu(title, chNames.ToArray());

                if (chosen == -1)
                {
                    break;
                }
                if (chosen == -2)
                {

                }
                if (chosen == -3)
                {

                }

                objects[chosen].StartOwnMenu();
            }
        }
        */
        #endregion
    }
}
