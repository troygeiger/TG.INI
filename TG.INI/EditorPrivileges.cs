using System;
using System.Collections.Generic;
using System.Text;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Extensions attribute required for the EnumExtensions class
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

namespace TG.INI
{
    /// <summary>
	/// Extensions for enums.
	/// </summary>
	public static class EnumExtensions
    {
        /// <summary>
        /// A FX 3.5 way to mimic the FX4 "HasFlag" method.
        /// </summary>
        /// <param name="variable">The tested enum.</param>10
        /// <param name="value">The value to test.</param>
        /// <returns>True if the flag is set. Otherwise false.</returns>
        public static bool HasFlag(this Enum variable, Enum value)
        {
            // check if from the same type.
            if (variable.GetType() != value.GetType())
            {
                throw new ArgumentException("The checked flag is not from the same type as the checked variable.");
            }

            ulong num = Convert.ToUInt64(value);
            ulong num2 = Convert.ToUInt64(variable);

            return (num2 & num) == num;
        }
    }
    /// <summary>
    /// Defines the privileges 
    /// </summary>
    public enum EditorPrivileges
    {
        /// <summary>
        /// Disables all editing.
        /// </summary>
        ReadOnly = 0,
        
        /// <summary>
        /// Allows users to add new sections.
        /// </summary>
        AddSections = 1,
        
        /// <summary>
        /// Allows users to remove sections.
        /// </summary>
        RemoveSections = 2,

        /// <summary>
        /// Allows users to add new entries.
        /// </summary>
        AddEntries = 4,

        /// <summary>
        /// Allows users to remove entries.
        /// </summary>
        RemoveEntries = 8,

        /// <summary>
        /// Allows users to change the Key column values.
        /// </summary>
        ChangeKeys = 16,

        /// <summary>
        /// Allows users to change the Value column values.
        /// </summary>
        ChangeValues = 32,

        /// <summary>
        /// Allows users to all functionality within the editor.
        /// </summary>
        All = 63
    }

}
