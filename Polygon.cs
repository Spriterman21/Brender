using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    [Serializable()]
    public class Polygon
    {
        public Polygon(Vector3[] localPoints, CharInfo edges, CharInfo fill, int drawPreference = 0, bool BFC = true)
        {
            this.localPoints = localPoints;
            this.edges = edges;
            this.fill = fill;
            this.drawPreference = drawPreference;
            this.applyBackFaceCulling = BFC;

            realPoints = new Vector3[localPoints.Length];
            canvasPoints = new float[localPoints.Length][];
            camDistance = new float[localPoints.Length];
        }

        public Vector3[] localPoints = new Vector3[3]; // multiples of x, y, z axis of parent object

        public Vector3[] realPoints = new Vector3[3]; // real coordinates of points in scene
        public float[][] canvasPoints = new float[3][]; // coordinates of points on canvas
        public float[] camDistance = new float[3];

        public CharInfo edges;
        public CharInfo fill;

        public double distance;
        public int drawPreference;
        public bool draw;

        public bool applyBackFaceCulling;
    }
}
