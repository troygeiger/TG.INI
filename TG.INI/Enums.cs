namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    #region Enumerations

    /// <summary>
    /// Represents the type of entry the line is within an INI file.
    /// </summary>
    public enum EntryTypes
    {
        /// <summary>
        /// Represents a blank line.
        /// </summary>
        WhiteSpace,

        Comment,

        KeyValue
    }

    #endregion Enumerations
}