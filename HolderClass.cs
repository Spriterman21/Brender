using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// https://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-of-an-object-in-net

namespace Brender_0_5
{
    public static class HolderClass
    {
        public static List<Polygon> polygons; // collection of all polygons in active scene to be used by cameras
        public static ConsoleKeyInfo key; // the key pressed in this frame
        public static CharInfo background; // the default background of the console
        public static bool debug = false; // outputs some debug info from around the program using diagnostics

        public static int x; // horizontal size of window
        public static int y; // vertical size of window

        public static Stopwatch sw = new Stopwatch();

        /// <summary>
        /// creates a deep copy of a class (or something else)
        /// </summary>
        /// <typeparam name="T">type of class it returns</typeparam>
        /// <param name="obj">the stuff to copy</param>
        /// <returns>the coppied class</returns>
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
