using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    [Serializable()]
    public class Mesh : Component
    {
        public Polygon[] polygons;
        
        public Mesh(Polygon[] polygons)
        {
            this.polygons = polygons;
        }

        public Mesh()
        {
            polygons = new Polygon[0];
        }

        public override void Update()
        {
            for (int i = 0; i < polygons.Length; i++)
            {
                for (int j = 0; j < polygons[i].localPoints.Length; j++)
                {
                    if (polygons[i].localPoints.Length != polygons[i].realPoints.Length)
                    {
                        int length = polygons[i].localPoints.Length;
                        polygons[i].realPoints = new Vector3[length];
                        polygons[i].canvasPoints = new float[length][];
                        polygons[i].camDistance = new float[length];
                    }

                    polygons[i].realPoints[j] = _object.globalPosition + Quaternion.RotatePoint(_object.globalRotation, polygons[i].localPoints[j] & _object.globalScale);
                }

                HolderClass.polygons.Add(polygons[i]);
            }
        }

        #region Menu creation
        static readonly string[] names = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Polygons",
        };

        public override void StartOwnMenu()
        {
            // creating options
            List<object> optionFns = new List<object>();
            List<Polygon> pol = new List<Polygon>();
            for (int i = 0; i < polygons.Length; i++)
            {
                pol.Add(polygons[i]);
            }

            optionFns.Add(name);
            optionFns.Add(pol);

            // starting menu
            ListMenu<object> menu = new ListMenu<object>(name, names, optionFns);
            menu.EngageMenu();

            // restoring possibly changed variables
            polygons = pol.ToArray();
        }
        #endregion
    }
}
