using System;
using System.Collections.Generic;
using System.Text;

namespace TG.INI.Serialization
{
    /// <summary>
    /// Tells the serializer to quote the value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IniQuoteValueAttribute : Attribute
    {
    }
}
