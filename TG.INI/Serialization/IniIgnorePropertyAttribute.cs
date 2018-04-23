using System;
using System.Collections.Generic;
using System.Text;

namespace TG.INI.Serialization
{
    /// <summary>
    /// Indicates that a property should be ignored when serializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IniIgnorePropertyAttribute : Attribute
    {
        /// <summary>
        /// Default constructor where <see cref="Ignore"/> is true.
        /// </summary>
        public IniIgnorePropertyAttribute()
        {
            Ignore = true;
        }

        /// <summary>
        /// Constructor where you define if the property should be ignored.
        /// </summary>
        /// <param name="ignore"></param>
        public IniIgnorePropertyAttribute(bool ignore)
        {
            Ignore = ignore;
        }

        /// <summary>
        /// Should the property be ignored.
        /// </summary>
        public bool Ignore { get; }
    }
}
