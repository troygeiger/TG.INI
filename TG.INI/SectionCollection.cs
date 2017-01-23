namespace TG.INI
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A collection to store <see cref="IniSection"/>.
    /// </summary>
    public class SectionCollection : System.Collections.CollectionBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SectionCollection"/>.
        /// </summary>
        protected internal SectionCollection(IniDocument document)
        {
            this.ParentDocument = document;
        }

        #endregion Constructors

        #region Indexers

        /// <summary>
        /// Gets the <see cref="IniSection"/> by name.
        /// </summary>
        /// <param name="name">The name of the <see cref="IniSection"/> to get.</param>
        /// <returns>The <see cref="IniSection"/>, if found; otherwise a new <see cref="IniSection"/> will be created.</returns>
        public IniSection this[string name]
        {
            get
            {
                IniSection section = Find(name);
                if (section == null)
                    return this.Add(name);
                else
                    return section;
            }
        }

        /// <summary>
        /// Gets the <see cref="IniSection"/> as a given index.
        /// </summary>
        /// <param name="index">The index of the section.</param>
        /// <returns><see cref="IniSection"/></returns>
        public IniSection this[int index]
        {
            get
            {
                return List[index] as IniSection;
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds an <see cref="IniSection"/> to the collection.
        /// </summary>
        /// <param name="section">The <see cref="IniSection"/> to be added.</param>
        /// <returns>The value of param section.</returns>
        public IniSection Add(IniSection section)
        {
            List.Add(section);
            section.ParentDocument = this.ParentDocument;
            return section;
        }

        /// <summary>
        /// Initializes a new <see cref="IniSection"/> with the provided name and adds it to the collection.
        /// </summary>
        /// <param name="name">The name of the section.</param>
        /// <returns>The instance of the new <see cref="IniSection"/>.</returns>
        public IniSection Add(string name)
        {
            var section = new IniSection(name) { ParentDocument = this.ParentDocument };
            List.Add(section);
            return section;
        }

        /// <summary>
        /// Determines if the collection contains a section with the provided name.
        /// </summary>
        /// <param name="name">The name of the section to find.</param>
        /// <returns>True if the collection contains the section; otherwise false.</returns>
        public bool Contains(string name)
        {
            return Find(name) != null;
        }

        /// <summary>
        /// Searches the collection for an <see cref="IniSection"/> by the name.
        /// </summary>
        /// <param name="name">The name of the <see cref="IniSection"/> to find.</param>
        /// <returns>The <see cref="IniSection"/>, if found; otherwise null.</returns>
        public IniSection Find(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                IniSection sec = List[i] as IniSection;
                if (string.Equals(sec.Name, name, StringComparison.CurrentCultureIgnoreCase))
                    return sec;
            }
            return null;
        }

        /// <summary>
        /// Remove an <see cref="IniSection"/> from the collection.
        /// </summary>
        /// <param name="section"></param>
        public void Remove(IniSection section)
        {
            if (section != null)
            {
                section.ParentDocument = null;
                List.Remove(section);
            }
        }

        /// <summary>
        /// Remove an <see cref="IniSection"/> from the collection, by name.
        /// </summary>
        /// <param name="name">The name of the section to remove.</param>
        public void Remove(string name)
        {
            var sec = Find(name);
            sec.ParentDocument = null;
            if (sec != null)
                List.Remove(sec);
        }

        #endregion Methods

        /// <summary>
        /// Gets the parent <see cref="IniDocument"/>.
        /// </summary>
        public IniDocument ParentDocument { get; internal set; }
    }
}