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

        public override void Update()
        {
            for (int i = 0; i < polygons.Length; i++)
            {
                for (int j = 0; j < polygons[i].localPoints.Length; j++)
                {
                    polygons[i].realPoints[j] = _object.globalPosition + Quaternion.RotatePoint(_object.globalRotation, polygons[i].localPoints[j] & _object.globalScale);
                    //System.Diagnostics.Debug.WriteLine("{0},  {1},  {2}", polygons[i].localPoints[j].x, polygons[i].localPoints[j].y, polygons[i].localPoints[j].z);
                }

                HolderClass.polygons.Add(polygons[i]);
            }
        }
    }
}
