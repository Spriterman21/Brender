using System;
using System.Collections.Generic;
using System.Text;

namespace Brender_0_5
{
    public interface CreatablesFactory
    {
        Tuple<ICreatable, Ref<string>> Create();
    }

    public class ObjectFactory : CreatablesFactory
    {
        public Tuple<ICreatable, Ref<string>> Create()
        {
            Object newObject = null;

            string[] prefabs = HolderClass.FileNames(HolderClass.prefabsPath);
            string[] options = new string[prefabs.Length + 1];
            options[0] = "\\empty\\";

            for (int i = 0; i < prefabs.Length; i++)
            {
                options[i + 1] = prefabs[i];
            }

            ListMenu<Object> menu = new ListMenu<Object>("Choose object to add", options);
            int chosenObject = menu.EngageMenu();

            // nothing chosen so just quit
            if (chosenObject == -1)
            {
                return null;
            }
            // empty chosen so just create one
            if (chosenObject == 0)
            {
                newObject = new Object();
                return new Tuple<ICreatable, Ref<string>>(newObject, newObject.name);
            }

            // might have chosen importable object, in that case, import it
            if (options[chosenObject].Length >= 4)
            {
                string cut = options[chosenObject].Substring(options[chosenObject].Length - 4);
                if (cut == ".txt" || cut == ".obj")
                {
                    newObject = ObjImporter.Import(HolderClass.prefabsPath + "\\" + options[chosenObject]);
                    newObject.moved = true;
                    return new Tuple<ICreatable, Ref<string>>(newObject, newObject.name);
                }

            }

            // load up the selected object the normal way
            newObject = HolderClass.Loader<Object>(HolderClass.prefabsPath + "\\" + options[chosenObject]);
            newObject.moved = true;
            return new Tuple<ICreatable, Ref<string>>(newObject, newObject.name);
        }
    }

    public class ComponentFactory : CreatablesFactory
    {
        public Tuple<ICreatable, Ref<string>> Create()
        {
            Component newComponent = null;

            string[] options = new string[]
            {
                "Mesh",
                "Controls",
                "Camera"
            };

            ListMenu<Component> menu = new ListMenu<Component>("Choose a component to add", options);
            int chosenObject = menu.EngageMenu();

            switch (chosenObject)
            {
                case 0:
                    newComponent = new Mesh();
                    break;
                case 1:
                    newComponent = new Controls();
                    break;
                case 2:
                    newComponent = new Camera();
                    break;
                default: return null;
            }

            return new Tuple<ICreatable, Ref<string>>(newComponent, newComponent.name);
        }
    }

    public class PolygonFactory : CreatablesFactory
    {
        public Tuple<ICreatable, Ref<string>> Create()
        {
            Polygon newPol = new Polygon();
            return new Tuple<ICreatable, Ref<string>>(newPol, newPol.name);
        }
    }

    public class Vector3Factory : CreatablesFactory
    {
        public Tuple<ICreatable, Ref<string>> Create()
        {
            Vector3 newV3 = new Vector3();
            return new Tuple<ICreatable, Ref<string>>(newV3, newV3.name);
        }
    }

    public class SceneFactory : CreatablesFactory
    {
        public Tuple<ICreatable, Ref<string>> Create()
        {
            Scene newScene = new Scene(new Object[0]);
            return new Tuple<ICreatable, Ref<string>>(newScene, newScene.name);
        }
    }
}
