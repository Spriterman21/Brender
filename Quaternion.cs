using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    [Serializable()]
    public struct quaternion
    {
        public float w;
        public Vector3 vector;

        public quaternion(float w, Vector3 vector)
        {
            this.w = w;
            this.vector = vector;
        }

        public quaternion(Vector3 axes, float angle)
        {
            w = MathF.Cos(angle / 2);
            vector = axes * MathF.Sin(angle / 2);
        }

        public quaternion(float w, float x, float y, float z)
        {
            this.w = w;
            vector = new Vector3(x, y, z);
        }

        public static quaternion operator +(quaternion q0, quaternion q1)
        {
            return new quaternion(q0.w + q1.w, q0.vector + q1.vector);
        }

        public static quaternion operator -(quaternion q0, quaternion q1)
        {
            return new quaternion(q0.w - q1.w, q0.vector - q1.vector);
        }

        public static quaternion operator -(quaternion q)
        {
            q.w = -q.w;
            q.vector = -q.vector;
            return q;
        }

        public static quaternion operator *(quaternion q0, quaternion q1)
        {
            quaternion q = new quaternion();

            q.w = q0.w * q1.w - (q0.vector * q1.vector);
            q.vector = (q1.vector * q0.w) + (q0.vector * q1.w) + (q0.vector % q1.vector);

            return q;
        }

        public static quaternion operator* (quaternion q, float f)
        {
            return new quaternion(q.w * f, q.vector * f);
        }

        public static quaternion operator *(float f, quaternion q)
        {
            return new quaternion(q.w * f, q.vector * f);
        }

        public float Length()
        {
            return MathF.Sqrt(w * w + vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        public quaternion Normalize()
        {
            float length = Length();
            w /= length;
            vector /= length;
            return Round();
        }

        public quaternion Round(int digits = 6)
        {
            w = MathF.Round(w, digits);
            vector.x = MathF.Round(vector.x, digits);
            vector.y = MathF.Round(vector.y, digits);
            vector.z = MathF.Round(vector.z, digits);

            return this;
        }
    }



    public static class Quaternion
    {
        /// <summary>
        /// returns the almost inverse of the inputed quaternion; they are technically equivalent to each other
        /// in terms of rotation (just google it)
        /// </summary>
        /// <param name="q">quaternion to conjugate</param>
        /// <returns>conjugated quaternion</returns>
        public static quaternion Conjugate(quaternion q)
        {
            q.vector = -q.vector;
            return q;
        }

        /// <summary>
        /// Rotates a vector (or basicaly a point around the origin) by some rotation defined by the quaternion
        /// </summary>
        /// <param name="rotation">rotation defined by a quaternion</param>
        /// <param name="point">a point in 3D space</param>
        /// <returns>returns the point rotated around the origin by some amount</returns>
        public static Vector3 RotatePoint(quaternion rotation, Vector3 point)
        {
            return (rotation * new quaternion(0, point) * Conjugate(rotation)).vector;
        }

        public static quaternion[] Slerp(quaternion q0, quaternion q1, float[] t, bool shorterPath = true)
        {
            q0.Normalize();
            q1.Normalize();

            float dotProduct = q0.vector * q1.vector + q0.w * q1.w;

            if (dotProduct < 0 && shorterPath)
            {
                q1.vector = -q1.vector;
                q1.w *= -1;
                dotProduct *= -1;
            }

            quaternion[] returner = new quaternion[t.Length];

            if (dotProduct > 0.99995f)
            {
                quaternion holder = q1 - q0;
                for (int i = 0; i < t.Length; i++)
                {
                    returner[i] = (q0 + (holder * t[i])).Normalize();
                }
            }

            float alpha0 = MathF.Acos(dotProduct);
            float sinAlpha0 = MathF.Sin(alpha0);

            for (int i = 0; i < t.Length; i++)
            {
                float alpha1 = alpha0 * t[i];
                float sinAlpha1 = MathF.Sin(alpha1);
                float s0 = MathF.Cos(alpha1) - dotProduct * sinAlpha1 / sinAlpha0;
                float s1 = sinAlpha1 / sinAlpha0;

                returner[i] = ((q0 * s0) + (q1 * s1)).Normalize();
            }

            return returner;
        }

        /// <summary>
        /// transforms inputed euler degrees into quaternions
        /// </summary>
        /// <param name="rotation">rotation in euler degrees</param>
        /// <param name="radians">true if euler is in radians, false if in degrees</param>
        /// <returns>rotation in quaternions</returns>
        public static quaternion Euler(Vector3 rotation, bool radians = true)
        {
            // just making it into radians
            if (!radians)
            {
                rotation.x *= MathF.PI / 180;
                rotation.y *= MathF.PI / 180;
                rotation.z *= MathF.PI / 180;
            }

            // pretty much just a coppied math formula
            float c0 = MathF.Cos(rotation.y / 2);
            float c1 = MathF.Cos(rotation.z / 2);
            float c2 = MathF.Cos(rotation.x / 2);
            float s0 = MathF.Sin(rotation.y / 2);
            float s1 = MathF.Sin(rotation.z / 2);
            float s2 = MathF.Sin(rotation.x / 2);

            return new quaternion(
                c0 * c1 * c2 - s0 * s1 * s2,
                s0 * s1 * c2 + c0 * c1 * s2,
                s0 * c1 * c2 + c0 * s1 * s2,
                c0 * s1 * c2 - s0 * c1 * s2
            ).Normalize();
        }

        /// <summary>
        /// Transforms a quaternion rotation to euler degrees
        /// </summary>
        /// <param name="q">the quaternion</param>
        /// <param name="outRadians">true to output radians, false to output degrees</param>
        /// <returns>rotation in euler degrees</returns>
        public static Vector3 QuatToEuler(quaternion q, bool outRadians = true)
        {
            q.Normalize();

            // again pretty much just a math formula with a bit of conversion into radians thrown in
            if (outRadians)
            {
                return new Vector3(
                    MathF.Atan2(2 * (q.w * q.vector.x + q.vector.y * q.vector.z), 1 - 2 * (q.vector.x * q.vector.x + q.vector.y * q.vector.y)),
                    MathF.Asin(2 * (q.w * q.vector.y - q.vector.z * q.vector.x)),
                    MathF.Atan2(2 * (q.w * q.vector.z + q.vector.x * q.vector.y), 1 - 2 * (q.vector.y * q.vector.y + q.vector.z * q.vector.z))
                ).Round();
            }

            return new Vector3(
                MathF.Atan2(2 * (q.w * q.vector.x + q.vector.y * q.vector.z), 1 - 2 * (q.vector.x * q.vector.x + q.vector.y * q.vector.y)) * 180 / MathF.PI,
                MathF.Asin(2 * (q.w * q.vector.y - q.vector.z * q.vector.x)) * 180 / MathF.PI,
                MathF.Atan2(2 * (q.w * q.vector.z + q.vector.x * q.vector.y), 1 - 2 * (q.vector.y * q.vector.y + q.vector.z * q.vector.z)) * 180 / MathF.PI
            ).Round();
        }

        /// <summary>
        /// Unused quaternion to euler
        /// </summary>
        /// <param name="q"></param>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static Vector3 QuatE2 (quaternion q, bool radians = true)
        {
            // not even sure, where I got this one, guess I should delete it soon
            float test = q.vector.x * q.vector.y + q.vector.z * q.w;

            float attitude;
            float bank;
            float heading;

            if (test > 0.499)
            { // singularity at north pole
                bank = MathF.PI / 2f;
                heading = 0;
                attitude = 2 * MathF.Atan2(q.vector.x, q.w);
            }
            else if (test < -0.499)
            { // singularity at south pole
                attitude = -2 * MathF.Atan2(q.vector.x, q.w);
                bank = -MathF.PI / 2f;
                heading = 0;
            }
            else
            {
                float sqx = q.vector.x * q.vector.x;
                float sqy = q.vector.y * q.vector.y;
                float sqz = q.vector.z * q.vector.z;
                attitude = MathF.Atan2(2 * q.vector.y * q.w - 2 * q.vector.x * q.vector.z, 1 - 2 * sqy - 2 * sqz);
                bank = MathF.Asin(2 * test);
                heading = MathF.Atan2(2 * q.vector.x * q.w - 2 * q.vector.y * q.vector.z, 1 - 2 * sqx - 2 * sqz);
            }

            if (!radians)
            {
                attitude *= 180 / MathF.PI;
                bank *= 180 / MathF.PI;
                heading *= 180 / MathF.PI;
            }

            return new Vector3(heading, attitude, bank);
            }
    }
}
