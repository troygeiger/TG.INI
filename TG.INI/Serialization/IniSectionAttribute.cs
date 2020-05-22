using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.INI.Serialization
{

    /// <summary>
    /// Define a section for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IniSectionAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute with a section name.
        /// </summary>
        /// <param name="sectionName">The name of the section</param>
        public IniSectionAttribute(string sectionName)
        {
            SectionName = sectionName;
        }

        /// <summary>
        /// The name of the section that the property will be placed in.
        /// </summary>
        public string SectionName { get; }
    }
}
