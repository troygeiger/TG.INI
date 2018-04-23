using System;
using System.Collections.Generic;
using System.Text;

namespace TG.INI.Serialization
{
    /// <summary>
    /// Interface for serializing and deserializing INI files.
    /// </summary>
    public interface ISerializer
    {

        /// <summary>
        /// Serialize an object into a new <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A new instance of <see cref="IniDocument"/> containing the key/values of the object's properties.</returns>
        IniDocument SerializeObject(object obj);

        /// <summary>
        /// Serialize an object into an <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="document">The <see cref="IniDocument"/> to add the properties and their values into.</param>
        void SerializeObjectInto(object obj, IniDocument document);

        /// <summary>
        /// Deserializes the <see cref="IniDocument"/> into the provided type.
        /// </summary>
        /// <typeparam name="T">The type of object that should be created and returned.</typeparam>
        /// <param name="document">The <see cref="IniDocument"/> containing the values to be applied.</param>
        /// <returns>A new instance of T type.</returns>
        T Deserialize<T>(IniDocument document);

        /// <summary>
        /// Deserializes the provided <see cref="IniDocument"/> into the obj.
        /// </summary>
        /// <param name="document">The <see cref="IniDocument"/> to deserialize.</param>
        /// <param name="obj">The object to deserialize into.</param>
        void DeserializeInto(IniDocument document, object obj);
    }
}
