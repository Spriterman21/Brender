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
        #region basic properties
        int type;
        float baseSpeed = 0.5f;
        float baseAngle = MathF.PI / 32f;
        int speedLevel = 2;
        #endregion



        public Controls(int type = 0)
        {
            this.type = type;
        }

        public override void Update()
        {
            switch (type % 2)
            {
                case 0:
                default:
                    SpaceEngineersCam();
                    break;
                case 1:
                    MinecraftCam();
                    break;
            }
        }

        void SpaceEngineersCam()
        {
            _object.moved = true;

            switch (HolderClass.key.Key)
            {
                case ConsoleKey.Add:
                    if (speedLevel < 6)
                    {
                        baseSpeed *= 2;
                        baseAngle *= 2;
                        speedLevel++;
                    }
                    break;
                case ConsoleKey.Subtract:
                    if (speedLevel > 0)
                    {
                        baseSpeed /= 2;
                        baseAngle /= 2;
                        speedLevel--;
                    }
                    break;
                case ConsoleKey.W:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * Vector3.Forward());
                    break;
                case ConsoleKey.A:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * Vector3.Right());
                    break;
                case ConsoleKey.S:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * -Vector3.Forward());
                    break;
                case ConsoleKey.D:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * -Vector3.Right());
                    break;
                case ConsoleKey.Spacebar:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * Vector3.Up());
                    break;
                case ConsoleKey.C:
                    _object.position += Quaternion.RotatePoint(_object.globalRotation, baseSpeed * -Vector3.Up());
                    break;
                case ConsoleKey.UpArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Right()), rotationAngle);
                    break;
                case ConsoleKey.LeftArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Up(), -baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Up()), -rotationAngle);
                    break;
                case ConsoleKey.DownArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), -baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Right()), -rotationAngle);
                    break;
                case ConsoleKey.RightArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Up(), baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Up()), rotationAngle);
                    break;
                case ConsoleKey.Q:
                    _object.QuatRotation *= new quaternion(Vector3.Forward(), baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Forward()), rotationAngle);
                    break;
                case ConsoleKey.E:
                    _object.QuatRotation *= new quaternion(Vector3.Forward(), -baseAngle);
                    //_object.quatRotation *= new quaternion(Quaternion.RotatePoint(_object.globalRotation, Vector3.Forward()), -rotationAngle);
                    break;
                default:
                    _object.moved = false;
                    break;
            }
        }

        void MinecraftCam()
        {
            _object.moved = true;
            Vector3 rotation;

            switch (HolderClass.key.Key)
            {
                case ConsoleKey.Add:
                    if (speedLevel < 6)
                    {
                        baseSpeed *= 2;
                        baseAngle *= 2;
                        speedLevel++;
                    }
                    break;
                case ConsoleKey.Subtract:
                    if (speedLevel > 0)
                    {
                        baseSpeed /= 2;
                        baseAngle /= 2;
                        speedLevel--;
                    }
                    break;
                case ConsoleKey.W:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.x = 0;
                    rotation.y = 0;
                    _object.position += Quaternion.RotatePoint(Quaternion.Euler(rotation), baseSpeed * Vector3.Forward()); ;
                    break;
                case ConsoleKey.A:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.x = 0;
                    rotation.y = 0;
                    _object.position += Quaternion.RotatePoint(Quaternion.Euler(rotation), baseSpeed * Vector3.Right());
                    break;
                case ConsoleKey.S:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.x = 0;
                    rotation.y = 0;
                    _object.position += Quaternion.RotatePoint(Quaternion.Euler(rotation), baseSpeed * -Vector3.Forward());
                    break;
                case ConsoleKey.D:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.x = 0;
                    rotation.y = 0;
                    _object.position += Quaternion.RotatePoint(Quaternion.Euler(rotation), baseSpeed * -Vector3.Right());
                    break;
                case ConsoleKey.Spacebar:
                    _object.position += baseSpeed * Vector3.Up();
                    break;
                case ConsoleKey.C:
                    _object.position += baseSpeed * -Vector3.Up();
                    break;
                case ConsoleKey.UpArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), baseAngle);
                    break;
                case ConsoleKey.LeftArrow:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.z -= baseAngle;
                    _object.QuatRotation = Quaternion.Euler(rotation);
                    break;
                case ConsoleKey.DownArrow:
                    _object.QuatRotation *= new quaternion(Vector3.Right(), -baseAngle);
                    break;
                case ConsoleKey.RightArrow:
                    rotation = Quaternion.QuatToEuler(_object.globalRotation);
                    rotation.z += baseAngle;
                    _object.QuatRotation = Quaternion.Euler(rotation);
                    break;
                default:
                    _object.moved = false;
                    break;
            }
        }

        #region Menu creation
        static readonly string[] names = new string[] /* names of changable variables to be displayed in a menu when you want to change some of them */
        {
            "Name",
            "Type",
            "Minimum movement speed",
            "Minimum rotation angle"
        };

        public override void StartOwnMenu()
        {
            // creating options
            List<object> optionFns = new List<object>();
            Ref<int> Type = new Ref<int>(type);
            Ref<float> speed = new Ref<float>(baseSpeed);
            Ref<float> angle = new Ref<float>(baseAngle);

            optionFns.Add(name);
            optionFns.Add(Type);
            optionFns.Add(speed);
            optionFns.Add(angle);

            // starting menu
            ListMenu<object> menu = new ListMenu<object>(name, names, optionFns);
            menu.EngageMenu();

            // restoring possibly changed variables
            type = Type.value;
            baseSpeed = speed.value;
            baseAngle = angle.value;
        }

        #endregion
    }
}
