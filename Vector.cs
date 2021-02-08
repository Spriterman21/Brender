using System;

namespace Brender_0_5
{
    [Serializable()]
    public class Vector3 : IMenu
    {
        #region basic properties ////////////////////////////////////////////
        Ref<float> x = new Ref<float>();
        Ref<float> y = new Ref<float>();
        Ref<float> z = new Ref<float>();
        public float X
        {
            get { return x.value; }
            set { x.value = value; }
        }
        public float Y
        {
            get { return y.value; }
            set { y.value = value; }
        }
        public float Z
        {
            get { return z.value; }
            set { z.value = value; }
        }
        #endregion

        #region defining changable properties ///////////////////////
        readonly string[] options = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "X",
            "Y",
            "Z"
        };

        object[] optionFns = new object[3];

        void DefineChangables()
        {
            optionFns[0] = x;
            optionFns[1] = y;
            optionFns[2] = z;
        }
        #endregion

        public Vector3()
        {
            DefineChangables();
        }

        public Vector3(float x, float y, float z)
        {
            DefineChangables();

            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 operator +(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.X, v0.Y, v0.Z);
            returner.X += v1.X;
            returner.Y += v1.Y;
            returner.Z += v1.Z;

            return returner;
        }

        public static Vector3 operator -(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.X, v0.Y, v0.Z);
            returner.X -= v1.X;
            returner.Y -= v1.Y;
            returner.Z -= v1.Z;

            return returner;
        }
        public static Vector3 operator -(Vector3 v)
        {
            Vector3 returner = new Vector3(v.X, v.Y, v.Z);
            returner.X *= -1;
            returner.Y *= -1;
            returner.Z *= -1;

            return returner;
        }


        public static Vector3 operator *(Vector3 v, float s)
        {
            Vector3 returner = new Vector3(v.X, v.Y, v.Z);
            returner.X *= s;
            returner.Y *= s;
            returner.Z *= s;

            return returner;
        }

        public static Vector3 operator *(float s, Vector3 v)
        {
            Vector3 returner = new Vector3(v.X, v.Y, v.Z);
            returner.X *= s;
            returner.Y *= s;
            returner.Z *= s;

            return returner;
        }

        public static Vector3 operator /(Vector3 v, float s)
        {
            Vector3 returner = new Vector3(v.X, v.Y, v.Z);
            returner.X /= s;
            returner.Y /= s;
            returner.Z /= s;

            return returner;
        }

        public static Vector3 operator &(Vector3 v0, Vector3 v1)
        {
            Vector3 returner = new Vector3(v0.X, v0.Y, v0.Z);
            returner.X *= v1.X;
            returner.Y *= v1.Y;
            returner.Z *= v1.Z;

            return returner;
        }


        public static float operator *(Vector3 v0, Vector3 v1)
        {
            return v0.X * v1.X + v0.Y * v1.Y + v0.Z * v1.Z;
        }

        public static Vector3 operator %(Vector3 v0, Vector3 v1)
        {
            return new Vector3(
                v0.Y * v1.Z - v0.Z * v1.Y,
                v0.Z * v1.X - v0.X * v1.Z,
                v0.X * v1.Y - v0.Y * v1.X
            );
        }

        public float Length()
        {
            return MathF.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {
            float length = Length();
            X /= length;
            Y /= length;
            Z /= length;
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
            MathF.Round(X, digits),
            MathF.Round(Y, digits),
            MathF.Round(Z, digits)
            );
        }


        public bool StartOwnMenu()
        {
            Menu menu = new Menu("Vector3", options, optionFns);
            menu.EngageMenu();

            return false;
        }
    }
}
