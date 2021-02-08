using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brender_0_5
{
    [Serializable()]
    class Controls : Component
    {
        public int type;
        public float moveSpeed = 0.5f;
        public float rotationAngle = MathF.PI / 32f;



        public Controls(int type = 0)
        {
            this.type = type;
        }

        public override void Update()
        {
            switch (type)
            {
                case 0:
                default:
                    SpaceEngineersCam();
                    break;
            }
        }

        void SpaceEngineersCam()
        {
            _object.moved = true;
            
            switch (HolderClass.key.Key)
            {
                case ConsoleKey.Add:
                    if (moveSpeed < 8)
                    {
                        moveSpeed *= 2;
                    }
                    break;
                case ConsoleKey.Subtract:
                    if (moveSpeed > 0.125)
                    {
                        moveSpeed /= 2;
                    }
                    break;
                case ConsoleKey.W:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * Vector3.Forward());
                    break;
                case ConsoleKey.A:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * Vector3.Right());
                    break;
                case ConsoleKey.S:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * -Vector3.Forward());
                    break;
                case ConsoleKey.D:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * -Vector3.Right());
                    break;
                case ConsoleKey.Spacebar:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * Vector3.Up());
                    break;
                case ConsoleKey.C:
                    _object.Position += Quaternion.RotatePoint(_object.globalRotation, moveSpeed * -Vector3.Up());
                    break;
                case ConsoleKey.I:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Right()), rotationAngle);
                    break;
                case ConsoleKey.J:
                    _object.QuatRotation *= new quaternion(Vector3.Up(), -rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Up()), -rotationAngle);
                    break;
                case ConsoleKey.K:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), -rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Right()), -rotationAngle);
                    break;
                case ConsoleKey.L:
                    _object.QuatRotation *= new quaternion(Vector3.Up(), rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Up()), rotationAngle);
                    break;
                case ConsoleKey.U:
                    _object.QuatRotation *= new quaternion(Vector3.Forward(), rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Forward()), rotationAngle);
                    break;
                case ConsoleKey.O:
                    _object.QuatRotation *= new quaternion(Vector3.Forward(), -rotationAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Forward()), -rotationAngle);
                    break;
                default:
                    _object.moved = false;
                    break;
            }
        }
    }
}
