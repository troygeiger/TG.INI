namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This is the base class that represents an INI entry.
    /// </summary>
    public abstract class IniEntry
    {
        #region Properties

        /// <summary>
        /// Gets the type of entry represented by <see cref="EntryTypes"/>. 
        /// </summary>
        public abstract EntryTypes EntryType
        {
            get;
        }

        /// <summary>
        /// Gets or Sets the string value of the entry.
        /// </summary>
        public virtual string Value
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a string that represents the entry.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// Gets the parent <see cref="IniDocument"/>.
        /// </summary>
        public IniDocument ParentDocument { get; internal set; }

        #endregion Methods
    }
}