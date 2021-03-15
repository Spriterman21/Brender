using System;
using System.Collections.Generic;

namespace Brender_0_5
{
    [Serializable()]
    public class Vector3 : IMenu, ICreatable
    {
        #region basic properties ////////////////////////////////////////////
        public Ref<string> name = new Ref<string>("Vector3");

        public float x = 0;
        public float y = 0;
        public float z = 0;
        #endregion

        public Vector3()
        {
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator +(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.x, v0.y, v0.z);
            returner.x += v1.x;
            returner.y += v1.y;
            returner.z += v1.z;

            return returner;
        }

        public static Vector3 operator -(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.x, v0.y, v0.z);
            returner.x -= v1.x;
            returner.y -= v1.y;
            returner.z -= v1.z;

            return returner;
        }
        public static Vector3 operator -(Vector3 v)
        {
            Vector3 returner = new Vector3(v.x, v.y, v.z);
            returner.x *= -1;
            returner.y *= -1;
            returner.z *= -1;

            return returner;
        }


        public static Vector3 operator *(Vector3 v, float s)
        {
            Vector3 returner = new Vector3(v.x, v.y, v.z);
            returner.x *= s;
            returner.y *= s;
            returner.z *= s;

            return returner;
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            Vector3 returner = new Vector3(v.x, v.y, v.z);
            returner.x *= s;
            returner.y *= s;
            returner.z *= s;

            return returner;
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            Vector3 returner = new Vector3(v.x, v.y, v.z);
            returner.x /= s;
            returner.y /= s;
            returner.z /= s;

            return returner;
        }

        public static Vector3 operator &(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.x, v0.y, v0.z);

            returner.x *= v1.x;
            returner.y *= v1.y;
            returner.z *= v1.z;

            return returner;
        }


        public static float operator *(Vector3 v0, Vector3 v1)
        {
            return v0.x * v1.x + v0.y * v1.y + v0.z * v1.z;
        }

        public static Vector3 operator %(Vector3 v0, Vector3 v1)
        {
            return new Vector3(
                v0.y * v1.z - v0.z * v1.y,
                v0.z * v1.x - v0.x * v1.z,
                v0.x * v1.y - v0.y * v1.x
            );
        }

        public float Length()
        {
            return MathF.Sqrt(x * x + y * y + z * z);
        }

        public void Normalize()
        {
            float length = Length();
            x /= length;
            y /= length;
            z /= length;
        }

        public static Vector3 Forward()
        {
            return new Vector3(0, 1, 0);
        }

        public static Vector3 Up()
        {
            return new Vector3(0, 0, 1);
        }

        public static Vector3 Right()
        {
            return new Vector3(1, 0, 0);
        }

        public static Vector3 Zero()
        {
            return new Vector3(0, 0, 0);
        }

        public static Vector3 FullOne()
        {
            return new Vector3(1, 1, 1);
        }

        public Vector3 Round(int digits = 6)
        {
            return new Vector3(
            MathF.Round(x, digits),
            MathF.Round(y, digits),
            MathF.Round(z, digits)
            );
        }

        #region Menu handling
        static readonly string[] options = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "X",
            "Y",
            "Z"
        };

        public void StartOwnMenu()
        {
            List<object> optionFns = new List<object>();
            optionFns.Add(name);
            optionFns.Add(new Ref<float>(x));
            optionFns.Add(new Ref<float>(y));
            optionFns.Add(new Ref<float>(z));

            ListMenu<object> menu = new ListMenu<object>(new Ref<string>("Vector3"), options, optionFns);
            menu.EngageMenu();

            x = ((Ref<float>)optionFns[1]).value;
            y = ((Ref<float>)optionFns[2]).value;
            z = ((Ref<float>)optionFns[3]).value;
        }
        #endregion
    }
}
