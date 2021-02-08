using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Brender_0_5
{
    public static class ObjImporter
    {
        public static Object Import(string[] lines)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int[]> vertOrder = new List<int[]>();

            //https://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = regex.Replace(lines[i], " ");
                
                if (lines[i].Length < 8)
                {
                    continue;
                }

                if (lines[i][0] == 'v' && lines[i][1] == ' ')
                {
                    vertices.Add(GetVertecees(lines[i]));
                }

                if (lines[i][0] == 'f' && lines[i][1] == ' ')
                {
                    vertOrder.Add(GetFaces(lines[i]));
                }
            }

            foreach (Vector3 vertex in vertices)
            {
                Console.WriteLine(vertex.X + ", " + vertex.Y + ", " + vertex.Z);
            }
            Console.WriteLine();
            foreach (int[] order in vertOrder)
            {
                foreach (int connection in order)
                {
                    Console.Write(connection + ", ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
            //Mesh test = MeshGenerator(vertices.ToArray(), vertOrder.ToArray());
            //Console.WriteLine(test.polygons[0].localPoints[0].x + "  " + test.polygons[0].localPoints[0].y + "  " + test.polygons[0].localPoints[0].z);
            return new Object(new Component[] { MeshGenerator(vertices.ToArray(), vertOrder.ToArray()) });

        }

        public static Vector3 GetVertecees(string line)
        {
            string[] numbers = line.Split(' ');

            float[] n = new float[3];
            for (int i = 0; i < 3; i++)
            {
                //System.Diagnostics.Debug.WriteLine(i);
                float.TryParse(numbers[i + 1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out n[i]);
            }

            return new Vector3(n[0], n[1], n[2]);
        }

        public static int[] GetFaces(string line)
        {
            string[] groups = line.Split(' ');
            if (groups.Length <= 1)
            {
                return null;
            }
            int[] numbers = new int[Math.Max(groups.Length - 1, 0)];

            for (int i = 1; i < groups.Length; i++)
            {
                if (groups[i] == "" || groups[i] == " ")
                {
                    Array.Resize(ref numbers, i - 1);
                    break;
                }
                Console.WriteLine(groups[i]);
                int.TryParse(groups[i].Split('/')[0], out numbers[i -1]);
            }

            return numbers;
        }

        public static Mesh MeshGenerator(Vector3[] vertices, int[][] vertOrder)
        {
            Polygon[] polygons = new Polygon[vertOrder.Length];
            
            for (int i = 0; i < vertOrder.Length; i++)
            {
                Vector3[] localPoints = new Vector3[vertOrder[i].Length];
                for (int j = 0; j < vertOrder[i].Length; j++)
                {
                    try
                    {
                        localPoints[j] = vertices[vertOrder[i][j] - 1];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine(localPoints.Length);
                        Console.WriteLine(i + "  " + j);
                        Console.WriteLine(vertices.Length);
                        Console.WriteLine(vertOrder[i][j]);
                        Console.Read();
                    }
                }

                foreach (Vector3 vector3 in localPoints)
                {
                    System.Diagnostics.Debug.WriteLine(vector3.X + ", " + vector3.Y + ", " + vector3.Z);
                }

                polygons[i] = new Polygon(localPoints, new CharInfo('X', 15, 0), new CharInfo('\0', 0, 0), 0, true);
            }

            System.Diagnostics.Debug.WriteLine("");

            return new Mesh(polygons);
        }
    }
}
