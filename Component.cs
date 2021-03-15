using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brender_0_5
{
    [Serializable()]
    public class Component : IMenu, ICreatable
    {
        #region basic properties 
        public bool enabled;
        public Ref<string> name = new Ref<string>("ComponentName");
        public Object _object;

        public string Name
        {
            get
            {
                return name.value;
            }
            set
            {
                name.value = value;
            }
        }
        #endregion

        [NonSerialized]
        static Dictionary<Type, string> compNames = new Dictionary<Type, string>
        {
            { typeof(Camera), "Camera" },
            { typeof(Mesh), "Mesh" },
            { typeof(Controls), "Controls" }
        };
        
        public Component()
        {
            string baseName = "ComponentName";
            compNames.TryGetValue(GetType(), out baseName);
            Name = baseName;
        }

        public virtual void Update()
        {
        }

        public virtual void StartOwnMenu()
        {
        }
    }
}
