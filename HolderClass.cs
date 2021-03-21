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
        public static Scene activeScene; // litterally what it says
        public static bool mainLoop;

        public static List<Polygon> polygons; // collection of all polygons in active scene to be used by cameras
        public static ConsoleKeyInfo key; // the key pressed in this frame
        public static CharInfo background; // the default background of the console
        public static bool debug = false; // outputs some debug info from around the program using diagnostics

        public static int x; // horizontal size of window
        public static int y; // vertical size of window

        public static Stopwatch sw = new Stopwatch();

        public static string prefabsPath = @"..\..\..\Prefabs";
        public static string scenesPath = @"..\..\..\Scenes";

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

        public static T Loader<T>(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        public static void Saver<T>(this T obj, string path)
        {
            //Directory.CreateDirectory(path);
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        public static string[] FileNames(string folderPath)
        {
            //Directory.CreateDirectory(folderPath);
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] files = dir.GetFiles();
            string[] fileNames = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = files[i].Name;
            }

            return fileNames;
        }
    }

    public class Saver
    {
        public Saver(ICreatable toSave)
        {
            this.toSave = toSave;
        }

        ICreatable toSave;

        public void Save()
        {
            if (toSave is Object)
            {
                string fileName = (toSave as Object).Name;
                fileName = fileName.Trim( '\\', '/', ':', '*', '?', '"', '<', '>', '|');

                if (fileName.Length == 0)
                {
                    fileName = DateTime.Now.ToString("yyyy/MM//dd/hh/mm/ss");
                }
                HolderClass.Saver(toSave as Object, HolderClass.prefabsPath + "\\" + fileName);
            }
            else if (toSave is Scene)
            {
                string fileName = (toSave as Scene).Name;
                fileName = fileName.Trim('\\', '/', ':', '*', '?', '"', '<', '>', '|');
                if (fileName.Length == 0)
                {
                    fileName = DateTime.Now.ToString("yyyy/MM//dd/hh/mm/ss");
                }
                HolderClass.Saver(toSave as Scene, HolderClass.scenesPath + "\\" + fileName);
            }
        }
    }
}
