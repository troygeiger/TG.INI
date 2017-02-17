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

        #region Fields

        string _key = null;
        bool _encryptValue = false;

        #endregion Fields

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
            this.EncryptValue = IsEncrypted;
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

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key, value and if the value should be encrypted.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encryptValue"></param>
        public IniKeyValue(string key, string value, bool encryptValue) : this(key, value)
        {
            this.EncryptValue = encryptValue;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key, value and if the value should be encrypted and quoted.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encryptValue"></param>
        /// <param name="quoteValue"></param>
        public IniKeyValue(string key, string value, bool encryptValue, bool quoteValue) : this(key, value, encryptValue)
        {
            this.QuoteValue = quoteValue;
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
            get
            {
                return _key;
            }
            set
            {
                _key = value?.Replace(" ", "");
            }
        }

        /// <summary>
        /// Gets or Sets whether values should be enclosed in quotation marks.
        /// </summary>
        public bool QuoteValue
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the base string value of the <see cref="IniKeyValue"/>. 
        /// </summary>
        public string BaseValue
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        /// <summary>
        /// Gets or Sets the string value of the entry.
        /// </summary>
        public override string Value
        {
            get
            {
                if (ParentDocument != null && ParentDocument.HasGlobalCrypto && IsEncrypted)
                    return GetEncryptedValue(ParentDocument.GlobalCrypto);
                else
                    return base.Value;
            }

            set
            {
                if (EncryptValue && ParentDocument != null && ParentDocument.HasGlobalCrypto)
                    SetEncryptedValue(ParentDocument.GlobalCrypto, value);
                else
                    base.Value = value;
            }
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

        /// <summary>
        /// Gets whether the value is encrypted.
        /// </summary>
        public bool IsEncrypted
        {
            get
            {
                return !string.IsNullOrEmpty(base.Value) && base.Value.Length >= 7 && base.Value.Substring(0, 7).ToLower() == "crypto:";
            }
        }

        /// <summary>
        /// Gets or Sets if the value should be encrypted.
        /// </summary>
        public bool EncryptValue
        {
            get { return _encryptValue; }
            set
            {
                _encryptValue = value;
                if (ParentDocument != null && ParentDocument.HasGlobalCrypto)
                {
                    if ((value && !IsEncrypted) || (!value && IsEncrypted))
                        this.Value = this.Value;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets a value as encrypted.
        /// </summary>
        /// <param name="encryptionKey">The key string used to encrypt the value.</param>
        /// <param name="value">The value to encrypt.</param>
        public void SetEncryptedValue(string encryptionKey, string value)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    base.Value = "crypto:" + (string.IsNullOrEmpty(value) ? "" : c.EncryptBase64(value));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Sets a value as encrypted.
        /// </summary>
        /// <param name="encryptionKey">The key string used to encrypt the value.</param>
        /// <param name="value">The value to encrypt.</param>
        public void SetEncryptedValue(byte[] encryptionKey, string value)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    base.Value = "crypto:" + (string.IsNullOrEmpty(value) ? "" : c.EncryptBase64(value));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Sets a value as encrypted.
        /// </summary>
        /// <param name="crypto">The <see cref="Crypto"/> to use when encrypting the value.</param>
        /// <param name="value">The value to encrypt.</param>
        public void SetEncryptedValue(Crypto crypto, string value)
        {
            try
            {
                base.Value = "crypto:" + (string.IsNullOrEmpty(value) ? "" : crypto.EncryptBase64(value));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Gets an encrypted value.
        /// </summary>
        /// <param name="crypto">The <see cref="Crypto"/> to use when unencrypting the value.</param>
        /// <returns>The unencrypted value.</returns>
        public string GetEncryptedValue(Crypto crypto)
        {
            try
            {
                return crypto.DecryptBase64(IsEncrypted ? base.Value.Substring(7) : base.Value);
            }
            catch (Exception)
            {

            }
            return null;
        }

        /// <summary>
        /// Gets an encrypted value.
        /// </summary>
        /// <param name="encryptionKey">The key used to unencrypt the value.</param>
        /// <returns>The unencrypted value.</returns>
        public string GetEncryptedValue(string encryptionKey)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    return c.DecryptBase64(IsEncrypted ? base.Value.Substring(7) : base.Value);
            }
            catch (Exception)
            {

            }
            return null;
        }

        /// <summary>
        /// Gets an encrypted value.
        /// </summary>
        /// <param name="encryptionKey">The key used to unencrypt the value.</param>
        /// <returns>The unencrypted value.</returns>
        public string GetEncryptedValue(byte[] encryptionKey)
        {
            try
            {
                using (Crypto c = new Crypto(encryptionKey))
                    return c.DecryptBase64(IsEncrypted ? base.Value.Substring(7) : base.Value);
            }
            catch (Exception)
            {

            }
            return null;
        }

        internal override void OnParentChanged()
        {
            if (ParentDocument != null && ParentDocument.HasGlobalCrypto)
            {
                if (EncryptValue && !IsEncrypted)
                    this.Value = this.Value;
            }
        }

        #endregion Methods

        /// <summary>
        /// Returns the INI key/value output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return QuoteValue || (ParentDocument != null && ParentDocument.QuoteAllValues) ? $"{Key}=\"{base.Value}\"" : $"{Key}={base.Value}";
        }
    }
}