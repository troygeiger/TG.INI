using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TG.INI.Encryption;

namespace TG.INI
{
    
    /// <summary>
    /// Represents an INI file structure.
    /// </summary>
    public class IniDocument : IDisposable
    {
        #region Fields

        SectionCollection _sections;
        
        Serialization.ISerializer serializer = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="IniDocument"/>.
        /// </summary>
        public IniDocument()
        {
            _sections = new SectionCollection(this);
            GlobalSection = new IniSection("GLOBAL") { ParentDocument = this };
            CommentLineIndicator = ";";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> then reads from a path.
        /// </summary>
        /// <param name="path">The path to an INI file.</param>
        public IniDocument(string path) : this()
        {
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> with a designated <see cref="IEncryptionHandler"/>.
        /// </summary>
        /// <param name="encryptionHandler">The <see cref="IEncryptionHandler"/> to be set to <see cref="EncryptionHandler"/>.</param>
        public IniDocument(IEncryptionHandler encryptionHandler) : this()
        {
            EncryptionHandler = encryptionHandler;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> then reads from a path.
        /// </summary>
        /// <param name="path">The path to an INI file.</param>
        /// <param name="encryptionHandler">The <see cref="IEncryptionHandler"/> to user for decrypting values.</param>
        public IniDocument(string path, IEncryptionHandler encryptionHandler) : this()
        {
            EncryptionHandler = encryptionHandler;
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> then reads from a stream. 
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public IniDocument(Stream stream) : this()
        {
            Read(stream);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> then reads from a stream. 
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encryptionHandler">The <see cref="IEncryptionHandler"/> to user for decrypting values.</param>
        public IniDocument(Stream stream, IEncryptionHandler encryptionHandler) : this()
        {
            EncryptionHandler = encryptionHandler;
            Read(stream);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IniDocument"/> and deserializes the provided object.
        /// </summary>
        /// <param name="obj">The object to deserialize.</param>
        public IniDocument(object obj) : this()
        {
            Serialization.IniSerialization.SerializeObjectIntoDocument(obj, this);
        }
        
        #endregion Constructors

        /// <summary>
        /// Gets a <see cref="IniSection"/> by name.
        /// </summary>
        /// <param name="SectionName">The name of the <see cref="IniSection"/> to get.</param>
        /// <returns>The <see cref="IniSection"/>, if found; otherwise null.</returns>
        public IniSection this[string SectionName]
        {
            get
            {
                return this.Sections[SectionName];
            }
        }

        #region Properties

        /// <summary>
        /// Gets or Sets the string that indicates a line is a comment.
        /// </summary>
        public string CommentLineIndicator
        {
            get; set;
        }

        /// <summary>
        /// Gets the Global section of the INI file.
        /// </summary>
        public IniSection GlobalSection
        {
            get;
        }

        /// <summary>
        /// Gets a collection of <see cref="IniSection"/>.
        /// </summary>
        public SectionCollection Sections
        {
            get { return _sections; }
        }

        /// <summary>
        /// Get or set the <see cref="IEncryptionHandler"/> to use for encryption.
        /// </summary>
        public IEncryptionHandler EncryptionHandler { get; set; }
        
        /// <summary>
        /// Gets if the <see cref="EncryptionHandler"/> property is not null.
        /// </summary>
        public bool HasEncryptionHandler
        {
            get
            {
                return EncryptionHandler != null;
            }
        }
        
        /// <summary>
        /// Gets or Sets whether all <see cref="IniKeyValue.Value"/> properties should be quoted on output.
        /// </summary>
        public bool QuoteAllValues { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns the INI data as string.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            using (StringWriter sw = new StringWriter())
            {
                Write(sw);
                return sw.ToString();
            }
        }

        /// <summary>
        /// Parses an INI string.
        /// </summary>
        /// <param name="iniData">A string of INI text data.</param>
        /// <returns><see cref="IniDocument"/></returns>
        public static IniDocument Parse(string iniData)
        {
            return Parse(iniData, null);
        }

        /// <summary>
        /// Parses an INI string.
        /// </summary>
        /// <param name="iniData">A string of INI text data.</param>
        /// <param name="globalEncryptionHandler">The <see cref="IEncryptionHandler"/> to use for decryption.</param>
        /// <returns>A new instance of <see cref="IniDocument"/>.</returns>
        public static IniDocument Parse(string iniData, IEncryptionHandler globalEncryptionHandler)
        {
            using (StringReader reader = new StringReader(iniData))
            {
                var i = new IniDocument(globalEncryptionHandler);
                i.Read(reader);
                return i;
            }
        }

        /// <summary>
        /// Clones the current <see cref="IniDocument"/> and creates a new instance.
        /// </summary>
        /// <returns>A new instance of <see cref="IniDocument"/> with copied values of the current <see cref="IniDocument"/>.</returns>
        public IniDocument Clone()
        {
            string idata = this.ToString();
            var id = new IniDocument(this.EncryptionHandler);
            using (StringReader reader = new StringReader(idata))
                id.Read(reader);
            
            
            id.QuoteAllValues = this.QuoteAllValues;
            return id;
        }

        /// <summary>
        /// Reads an INI file and parses the data.
        /// </summary>
        /// <param name="path">Path to the INI file.</param>
        public void Read(string path)
        {
            using (var reader = new StreamReader(path))
                Read(reader);
        }

        /// <summary>
        /// Reads an INI file from a stream and parses the data.
        /// </summary>
        /// <param name="stream">A stream to read from.</param>
        public void Read(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                Read(reader);
        }


        /// <summary>
        /// Reads an INI file and parses the data.
        /// </summary>
        /// <param name="reader">A <see cref="TextReader"/> used to read the INI data.</param>
        public void Read(TextReader reader)
        {
            if (reader == null)
                return;

            IniSection gSection = GlobalSection;
            gSection.Clear();
            Sections.Clear();
            var kvRex = new Regex("((.+?)=\"(.*)\")|((.+?)=(.*))");
            var secRex = new Regex("\\[([a-zA-Z0-9_\\s]+)\\]");

            try
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (string.IsNullOrEmpty(line))
                        gSection.Add(new IniWhiteSpace(line));
                    else if (line.StartsWith(this.CommentLineIndicator))
                        gSection.AddComment(line.Remove(0, this.CommentLineIndicator.Length));
                    else
                    {
                        var m = kvRex.Match(line);

                        if (m.Success)
                        {
                            if (m.Groups[2].Length > 0)
                            {
                                //gSection.Add(new IniKeyValue(m.Groups[2].Value, m.Groups[3].Value) { QuoteValue = true });
                                gSection.InternalAddKeyValue(m.Groups[2].Value, m.Groups[3].Value, true);
                            }
                            else if (m.Groups[5].Length > 0)
                            {
                                //gSection.Add(new IniKeyValue(m.Groups[5].Value, m.Groups[6].Value));
                                gSection.InternalAddKeyValue(m.Groups[5].Value, m.Groups[6].Value, false);
                            }
                        }
                        else
                        {
                            m = secRex.Match(line);
                            if (m.Success)
                                gSection = this.Sections.Add(m.Groups[1].Value);

                        }
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Writes the content of <see cref="IniDocument"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/> to write to.</param>
        public void Write(TextWriter writer)
        {
            bool flag = false;

            for (int i = 0; i < GlobalSection.Count; i++)
            {
                if (flag) writer.Write("\r\n");
                writer.Write(GlobalSection[i].ToString());
                flag = true;
            }

            for (int i = 0; i < Sections.Count; i++)
            {
                var sec = Sections[i];
                if (flag) writer.Write("\r\n");
                writer.Write(sec.ToString());
                flag = true;
                for (int j = 0; j < sec.Count; j++)
                {
                    if (flag) writer.Write("\r\n");
                    writer.Write(sec[j].ToString());
                    flag = true;
                }
                flag = true;
            }
        }

        /// <summary>
        /// Writes the content of <see cref="IniDocument"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        public void Write(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
                Write(writer);
        }

        /// <summary>
        /// Writes the content of <see cref="IniDocument"/> to a file./>.
        /// </summary>
        /// <param name="path">The file path to write to.</param>
        public void Write(string path)
        {
            using (var writer = new StreamWriter(path))
                Write(writer);
        }

        /// <summary>
        /// Navigates to a section and looks for a key from the path.
        /// </summary>
        /// <param name="path">A string containing the section name separated by a backslash followed by the key name. If key/value is within the global section, enter the key name only.</param>
        /// <returns>Returns the <see cref="IniKeyValue"/> if found; otherwise null is returned.</returns>
        public IniKeyValue GetKeyValue(string path)
        {
            return GetKeyValue(path, false);
        }

        /// <summary>
        /// Navigates to a section and looks for a key from the path.
        /// </summary>
        /// <param name="path">A string containing the section name separated by a backslash followed by the key name. If key/value is within the global section, enter the key name only.</param>
        /// <param name="createIfNotExists">If true and either the section or key does not exist, they will be automatically created.</param>
        /// <returns>Returns the <see cref="IniKeyValue"/> if found; otherwise null is returned.</returns>
        public IniKeyValue GetKeyValue(string path, bool createIfNotExists)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            string[] sp = path.Split('\\');
            switch (sp.Length)
            {
                case 1:
                    IniKeyValue gval = GlobalSection.Find(sp[0]);
                    if (gval == null && createIfNotExists)
                    {
                        gval = new IniKeyValue(sp[0], "");
                        GlobalSection.Add(gval);
                    }
                    return gval;
                case 2:
                    IniSection sec = Sections.Find(sp[0]);
                    IniKeyValue kv = null;
                    if (sec != null)
                    {
                        kv = sec.Find(sp[1]);
                    }
                    else
                    {
                        sec = new IniSection(sp[0]);
                        Sections.Add(sec);
                    }
                    if (kv == null && createIfNotExists && sec != null)
                    {
                        kv = new IniKeyValue(sp[1], "");
                        sec.Add(kv);
                    }
                    return kv;
                default:
                    return null;
            }
        }

#if FULLNET
        /// <summary>
        /// Shows the IniEditor window for the current IniDocument.
        /// </summary>
        public System.Windows.Forms.DialogResult ShowEditor()
        {
            return ShowEditor(EditorPrivileges.All);
        }

        /// <summary>
        /// Shows the IniEditor window for the current IniDocument.
        /// </summary>
        /// <param name="privileges">>The privileges that the editor should have.</param>
        public System.Windows.Forms.DialogResult ShowEditor(EditorPrivileges privileges)
        {
            System.Windows.Forms.DialogResult result;
            using (var editor = new Controls.IniEditor())
            {
                editor.LoadDocument(this);
                editor.Privileges = privileges;
                result = editor.ShowDialog();
            }
            return result;
        }
#endif
        /// <summary>
        /// Disposes the document.
        /// </summary>
        public void Dispose()
        {
            //if (EncryptionHandler != null)
            //{
            //    EncryptionHandler.Dispose();
            //    EncryptionHandler = null;
            //}
            Sections.Clear();
            GlobalSection.Clear();
        }

#endregion Methods
    }
}