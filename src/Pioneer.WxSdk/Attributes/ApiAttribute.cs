using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApipathAttribute : Attribute
    {
        public string Name
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public ApipathAttribute(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }
    }
}
