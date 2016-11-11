namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class represents a key/value line. Ex. Year=2015
    /// </summary>
    public class IniKeyValue : IniEntry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/>.
        /// </summary>
        public IniKeyValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key and string value.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <param name="value">The string value.</param>
        public IniKeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key and int value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public IniKeyValue(string key, int value)
        {
            this.Key = key;
            this.ValueInt = value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get value <see cref="EntryTypes.KeyValue"/>.
        /// </summary>
        public override EntryTypes EntryType
        {
            get
            {
                return EntryTypes.KeyValue;
            }
        }

        /// <summary>
        /// Gets or Sets the key for this entry.
        /// </summary>
        public string Key
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets whether values should be enclosed in quotation marks.
        /// </summary>
        public bool QuoteValue
        {
            get; set;
        }
        
        /// <summary>
        /// Gets or Sets the value as a <see cref="bool"/>.
        /// </summary>
        public bool ValueBoolean
        {
            get
            {
                bool b = false;
                bool.TryParse(Value, out b);
                return b;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a <see cref="DateTime"/>.
        /// </summary>
        public DateTime ValueDateTime
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(Value, out dt);
                return dt;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a decimal type.
        /// </summary>
        public decimal ValueDecimal
        {
            get
            {
                decimal d = -1;
                decimal.TryParse(Value, out d);
                return d;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a float type.
        /// </summary>
        public float ValueFloat
        {
            get
            {
                float f = -1;
                float.TryParse(Value, out f);
                return f;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a double type.
        /// </summary>
        public double ValueDouble
        {
            get
            {
                double d = -1;
                double.TryParse(Value, out d);
                return d;
            }
            set
            {
                Value = value.ToString();
            }
        }
        
        /// <summary>
        /// Gets or Sets the value as an integer type.
        /// </summary>
        public int ValueInt
        {
            get
            {
                int i = -1;
                int.TryParse(Value, out i);
                return i;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a 64 bit integer.
        /// </summary>
        public long ValueInt64
        {
            get
            {
                long l = -1;
                long.TryParse(Value, out l);
                return l;
            }
            set
            {
                Value = value.ToString();
            }
        }

        #endregion Properties

        #region Methods

        public void SetEncryptedValue(string encryptionKey, string value)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    this.Value = c.EncryptBase64(value);
            }
            catch (Exception)
            {
                
            }
        }

        public string GetEncryptedValue(string encryptionKey)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    return c.DecryptBase64(Value);
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Returns the INI key/value output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return QuoteValue ? $"{Key}=\"{Value}\"" : $"{Key}={Value}";
        }
    }
}