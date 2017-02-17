namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// This class represents a section of INI.
    /// </summary>
    public class IniSection : System.Collections.CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="IniSection"/> with the provided name.
        /// </summary>
        /// <param name="name">The name of the new section.</param>
        public IniSection(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Parameter name must a valid ini section name.");
            Name = name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or Sets the section name.
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Gets the parent <see cref="IniDocument"/>.
        /// </summary>
        public IniDocument ParentDocument { get; internal set; }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the <see cref="IniEntry"/> at the given index within the collection.
        /// </summary>
        /// <param name="index">The index to retrieve the <see cref="IniEntry"/>.</param>
        /// <returns>The <see cref="IniEntry"/> at the given index.</returns>
        public IniEntry this[int index]
        {
            get { return List[index] as IniEntry; }
        }

        /// <summary>
        /// Gets an <see cref="IniKeyValue"/> with a matching key value.
        /// </summary>
        /// <param name="key">The key to match to a <see cref="IniKeyValue"/>.</param>
        /// <returns>The matching <see cref="IniKeyValue"/>; otherwise a new <see cref="IniKeyValue"/> will be returned.</returns>
        public IniKeyValue this[string key]
        {
            get
            {
                var result = Find(key);
                if (result == null)
                    return AddKeyValue(key, null);
                else
                    return result;
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds an <see cref="IniEntry"/> to the section.
        /// </summary>
        /// <param name="entry">The <see cref="IniEntry"/> to add.</param>
        public void Add(IniEntry entry)
        {
            if (entry != null)
            {
                List.Add(entry);
                entry.ParentDocument = this.ParentDocument;
            }
        }

        /// <summary>
        /// Adds an <see cref="IniKeyValue"/> to this section.
        /// </summary>
        /// <param name="keyValue">The <see cref="IniKeyValue"/> to add.</param>
        public void Add(IniKeyValue keyValue)
        {
            if (keyValue == null)
                return;
            if (ContainsKey(keyValue.Key))
                throw new Exception("Key already exists in section.");
            keyValue.ParentDocument = this.ParentDocument;
            List.Add(keyValue);
        }

        /// <summary>
        /// Adds a new key/value entry to the section.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        /// <returns>A new instance of <see cref="IniKeyValue"/>.</returns>
        public IniKeyValue AddKeyValue(string key, string value)
        {
            return AddKeyValue(key, value, false, false);
        }

        /// <summary>
        /// Adds a new key/value entry to the section.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        /// <param name="encryptValue">Should the value be encrypted?</param>
        /// <param name="quoteValue">Should the value be quoted?</param>
        /// <returns>A new instance of <see cref="IniKeyValue"/>.</returns>
        public IniKeyValue AddKeyValue(string key, string value, bool encryptValue, bool quoteValue)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (ContainsKey(key))
                throw new Exception("Key already exists in section.");
            var kv = new IniKeyValue(key, value, encryptValue, quoteValue);
            kv.ParentDocument = this.ParentDocument;
            List.Add(kv);
            return kv;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="IniComment"/> and add it to the section.
        /// </summary>
        /// <param name="value">The comment value.</param>
        /// <returns>A new instance of <see cref="IniComment"/>.</returns>
        public IniComment AddComment(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            var c = new IniComment(value);
            Add(c);
            return c;
        }

        /// <summary>
        /// Checks if a key exists in the section.
        /// </summary>
        /// <param name="key">The key to look for.</param>
        /// <returns>Returns true if the key exists in the series; otherwise false.</returns>
        public bool ContainsKey(string key)
        {
            return Find(key) != null;
        }

        public void RemoveEntry(IniEntry entry)
        {
            base.InnerList.Remove(entry);
        }

        public IniKeyValue Find(string key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                IniKeyValue kv = List[i] as IniKeyValue;
                if (string.Equals(kv?.Key, key, StringComparison.CurrentCultureIgnoreCase))
                    return kv;
            }
            return null;
        }

        /// <summary>
        /// Returns the INI section output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{this.Name}]";
        }

        #endregion Methods
    }
}