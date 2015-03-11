using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class StringMaxLengthAttribute : Attribute
    {
        public int MaxLength;

        public StringMaxLengthAttribute(int max)
        {
            this.MaxLength = max;
        }
    }
}
