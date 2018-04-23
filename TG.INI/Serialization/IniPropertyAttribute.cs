using System;
using System.Collections.Generic;
using System.Text;

namespace TG.INI.Serialization
{
    /// <summary>
    /// This attribute can be used to indicate that a private property should be serialized or to serialize/deserialize a property as a different name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IniPropertyAttribute : Attribute
    {

        /// <summary>
        /// Gets an overridden property name.
        /// </summary>
        public string PropertyNameOverride { get; }

        /// <summary>
        /// Gets whether the <see cref="PropertyNameOverride"/> has been set with a value.
        /// </summary>
        public bool HasPropertyNameOverride
        {
            get
            {
                return !string.IsNullOrEmpty(PropertyNameOverride);
            }
        }
    }
}


