namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class represents a INI comment line.
    /// </summary>
    public class IniComment : IniEntry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IniComment"/>.
        /// </summary>
        public IniComment()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniComment"/> with the comment value provided.
        /// </summary>
        /// <param name="value">The comment value of this <see cref="IniComment"/></param>
        public IniComment(string value)
        {
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets value <see cref="EntryTypes.Comment"/>.
        /// </summary>
        public override EntryTypes EntryType
        {
            get
            {
                return EntryTypes.Comment;
            }
        }

        /// <summary>
        /// Gets or Sets the comment value.
        /// </summary>
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns comment output.
        /// </summary>
        /// <returns>Returns comment output.</returns>
        public override string ToString()
        {
            return (ParentDocument == null ? ";" : ParentDocument.CommentLineIndicator) + Value;
        }

        #endregion
    }
}