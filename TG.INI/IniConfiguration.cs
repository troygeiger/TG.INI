using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TG.INI
{
    /// <summary>
    /// 
    /// </summary>
    public class IniConfiguration<T>
    {
        

        /// <summary>
        /// Initializes a new instance of <see cref="IniConfiguration{T}"/>.
        /// </summary>        
        /// <param name="path">The path to an INI file.</param>
        public IniConfiguration(string path) : this(path, null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="IniConfiguration{T}"/>.
        /// </summary>        
        /// <param name="path">The path to an INI file.</param>
        /// <param name="encryptionHandler">The <see cref="Encryption.IEncryptionHandler"/> to user for encrypting values.</param>
        public IniConfiguration (string path, Encryption.IEncryptionHandler encryptionHandler)
        {
            Path = path;
            EncryptionHandler = encryptionHandler;
        }


        public static T Properties { get; set; }

        public static string Path { get; set; }

        public static Encryption.IEncryptionHandler EncryptionHandler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static void Load()
        {
            if (File.Exists(Path))
            {
                using (IniDocument ini = new IniDocument(Path, EncryptionHandler))
                {
                    Properties = Serialization.IniSerialization.DeserializeDocument<T>(ini);
                }
            }
            else
            {
                Properties = Activator.CreateInstance<T>();
            }
        }

        public static void Save()
        {
            using (IniDocument ini = new IniDocument(EncryptionHandler))
            {
                Serialization.IniSerialization.SerializeObjectIntoDocument(Properties, ini);
                ini.Write(Path);
            }
            
        }
    }

}
