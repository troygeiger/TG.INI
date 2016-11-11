namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IniWhiteSpace : IniEntry
    {
        #region Fields

        int _spaces = 0;
        string _value = "";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IniWhiteSpace"/>.
        /// </summary>
        public IniWhiteSpace()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniWhiteSpace"/> with a provided number of spaces.
        /// </summary>
        /// <param name="spaces">The number of spaces the new instance represents.</param>
        public IniWhiteSpace(int spaces)
        {
            this.Spaces = spaces;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniWhiteSpace"/> with a provided white space string.
        /// </summary>
        /// <param name="whiteSpace">A string containing spaces. All other characters are ignored.</param>
        public IniWhiteSpace(string whiteSpace)
        {
            this.Value = whiteSpace;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets value <see cref="EntryTypes.WhiteSpace"/>.
        /// </summary>
        public override EntryTypes EntryType
        {
            get
            {
                return EntryTypes.WhiteSpace;
            }
        }

        /// <summary>
        /// Gets or Sets the number of spaces contained in this <see cref="IniWhiteSpace"/>.
        /// </summary>
        public int Spaces
        {
            get
            {
                return _spaces;
            }
            set
            {
                _spaces = value;
                string v = "";
                for (int i = 0; i < value; i++)
                    v += " ";
                this.Value = v;
            }
        }

        /// <summary>
        /// Gets or Sets the space string of the white space.
        /// </summary>
        public override string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (value == null)
                {
                    _spaces = 0;
                    return;
                }
                int i = 0;
                foreach (char c in value)
                {
                    if (c == ' ')
                        i++;
                }
                _spaces = i;
            }
        }

        #endregion Properties
    }
}