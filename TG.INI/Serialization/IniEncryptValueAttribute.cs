using System;
using System.Collections.Generic;
using System.Text;

namespace TG.INI.Serialization
{

    /// <summary>
    /// This attribute instructs the serializer to encrypt the string value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IniEncryptValueAttribute : Attribute
    {
    }
}
