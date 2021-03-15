using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    [Serializable()]
    public class Polygon : IMenu, ICreatable
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

        public Polygon()
        {
            localPoints = new Vector3[0];
            edges = new CharInfo(' ', 0, 0);
            fill = new CharInfo(' ', 0, 0);
            drawPreference = 0;

            realPoints = new Vector3[localPoints.Length];
            canvasPoints = new float[localPoints.Length][];
            camDistance = new float[localPoints.Length];
        }

        public Ref<string> name = new Ref<string>("Polygon");

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



        #region Menu creation
        static readonly string[] names = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Points",
            "Outline",
            "Fill"
        };

        public void StartOwnMenu()
        {
            // creating options
            List<object> optionFns = new List<object>();

            List<Vector3> points = new List<Vector3>(localPoints);
            Ref<CharInfo> o = new Ref<CharInfo>(edges);
            Ref<CharInfo> f = new Ref<CharInfo>(fill);

            optionFns.Add(name);
            optionFns.Add(points);
            optionFns.Add(o);
            optionFns.Add(f);

            // starting menu
            ListMenu<object> menu = new ListMenu<object>(name, names, optionFns);
            menu.EngageMenu();

            // restoring possibly changed variables
            localPoints = points.ToArray();
            edges = o.value;
            fill = f.value;
        }
        #endregion
    }
}
