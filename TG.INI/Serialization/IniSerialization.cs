using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using TG.INI.Encryption;

namespace TG.INI.Serialization
{
    /// <summary>
    /// Used to serialize simple object to INI and deserialize INI to a simple object.
    /// </summary>
    public class IniSerialization : ISerializer
    {
        #region Fields

        static Dictionary<Type, ConstructorInfo> constructorCache = new Dictionary<Type, ConstructorInfo>();
        static Dictionary<Type, PropertyInfoEx[]> propertyCache = new Dictionary<Type, PropertyInfoEx[]>();
        static IniSerialization _instance = null;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public IniSerialization()
        {
        }

        /// <summary>
        /// Creates a new instance and specifies the <see cref="Encryption.IEncryptionHandler"/> to use when serializing and deserializing.
        /// </summary>
        public IniSerialization(IEncryptionHandler encryptionHandler)
        {
            EncryptionHandler = encryptionHandler;
        }


        #endregion Fields

        /// <summary>
        /// Specifies the <see cref="INI.Encryption.IEncryptionHandler"/> to use when serializing and deserializing.
        /// </summary>
        public INI.Encryption.IEncryptionHandler EncryptionHandler { get; set; }

        /// <summary>
        /// If true, properties with null or blank strings will still be serialized. Default true.
        /// </summary>
        public bool SerializeNullAndBlank { get; set; } = true;

        private static IniSerialization Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new IniSerialization();
                }
                return _instance;
            }
        }

        #region Methods

        /// <summary>
        /// Deserializes the <see cref="IniDocument"/> into the provided type.
        /// </summary>
        /// <typeparam name="T">The type of object that should be created and returned.</typeparam>
        /// <param name="document">The <see cref="IniDocument"/> containing the values to be applied.</param>
        /// <returns>A new instance of T type.</returns>
        public virtual T Deserialize<T>(IniDocument document)
        {
            
            ConstructorInfo constructor = GetConstructor(typeof(T));

            if (constructor == null)
            {
                return default(T);
            }

            object obj = constructor.Invoke(null);
            
            DeserializeInto(document, obj);
            
            return (T)obj;
        }

        /// <summary>
        /// Deserializes the <see cref="IniDocument"/> into the provided type.
        /// </summary>
        /// <typeparam name="T">The type of object that should be created and returned.</typeparam>
        /// <param name="document">The <see cref="IniDocument"/> containing the values to be applied.</param>
        /// <returns>A new instance of T type.</returns>
        public static T DeserializeDocument<T>(IniDocument document)
        {
            return Instance.Deserialize<T>(document);
        }

        /// <summary>
        /// Deserializes the provided <see cref="IniDocument"/> into the obj.
        /// </summary>
        /// <param name="document">The <see cref="IniDocument"/> to deserialize.</param>
        /// <param name="obj">The object to deserialize into.</param>
        public virtual void DeserializeInto(IniDocument document, object obj)
        {
            Encryption.IEncryptionHandler originalHandler = document.EncryptionHandler;
            if (EncryptionHandler != null) document.EncryptionHandler = EncryptionHandler;

            if (obj == null) return;
            IniSection section = document.GlobalSection;

            foreach (PropertyInfoEx prop in GetProperties(obj))
            {
                if (prop.IgnoreProperty)
                {
                    continue;
                }

                if (prop.Section == null)
                {
                    section = document.GlobalSection;
                }
                else if (section.Name != prop.Section)
                {
                    section = document.Sections[prop.Section];
                }

                if (!section.ContainsKey(prop.Name)) continue;

                IniKeyValue kv = section[prop.Name];

                //string l = prop.PropertyType.FullName.ToLower();
                bool converted = false;
                if (prop.TypeConverter != null)
                {
                    try
                    {
                        if (prop.TypeConverter.CanConvertFrom(typeof(string)))
                        {
                            prop.SetValue(obj, prop.TypeConverter.ConvertFrom(kv.Value), null);
                            converted = true;
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    try
                    {
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(obj, Enum.Parse(prop.PropertyType, kv.Value), null);
                        }
                        else
                        {
                            Type ntype = Nullable.GetUnderlyingType(prop.PropertyType);
                            if (ntype != null)
                            {
                                if (kv.Value == null)
                                {
                                    prop.SetValue(obj, null, null);
                                }
                                else
                                {
                                    prop.SetValue(obj, Convert.ChangeType(kv.Value, ntype), null);
                                }
                            }
                            else
                            {
                                prop.SetValue(obj, Convert.ChangeType(kv.Value, prop.PropertyType), null);
                            }
                        }

                        converted = true;
                    }
                    catch (Exception)
                    {

                    }
                }

                if (!converted)
                {
                    switch (prop.PropertyType.FullName.ToLower())
                    {
                        case "system.string":
                            prop.SetValue(obj, kv.Value, null);
                            break;
                        case "system.boolean":
                            prop.SetValue(obj, kv.ValueBoolean, null);
                            break;
                        case "system.byte":
                            prop.SetValue(obj, kv.ValueByte, null);
                            break;
                        case "system.int16":
                            prop.SetValue(obj, kv.ValueInt16, null);
                            break;
                        case "system.int32":
                            prop.SetValue(obj, kv.ValueInt, null);
                            break;
                        case "system.int64":
                            prop.SetValue(obj, kv.ValueInt64, null);
                            break;
                        case "system.single":
                            prop.SetValue(obj, kv.ValueFloat, null);
                            break;
                        case "system.double":
                            prop.SetValue(obj, kv.ValueDouble, null);
                            break;
                        case "system.decimal":
                            prop.SetValue(obj, kv.ValueDecimal, null);
                            break;
                        case "system.datetime":
                            prop.SetValue(obj, kv.ValueDateTime, null);
                            break;
                        case "system.drawing.color":
                            prop.SetValue(obj, kv.ValueColor, null);
                            break;
                        case "system.drawing.point":
                            prop.SetValue(obj, kv.ValuePoint, null);
                            break;
                        case "system.drawing.pointf":
                            prop.SetValue(obj, kv.ValuePointF, null);
                            break;
                        case "system.drawing.size":
                            prop.SetValue(obj, kv.ValueSize, null);
                            break;
                        case "system.drawing.sizef":
                            prop.SetValue(obj, kv.ValueSizeF, null);
                            break;
                        case "system.drawing.rectagle":
                            prop.SetValue(obj, kv.ValueRectangle, null);
                            break;
                        case "system.drawing.rectanglef":
                            prop.SetValue(obj, kv.ValueRectangleF, null);
                            break;
                        default:
                            break;
                    }
                }
            }
            document.EncryptionHandler = originalHandler;
        }

        /// <summary>
        /// Deserializes the provided <see cref="IniDocument"/> into the obj.
        /// </summary>
        /// <param name="document">The <see cref="IniDocument"/> to deserialize.</param>
        /// <param name="obj">The object to deserialize into.</param>
        public static void DeserializeDocumentInto(IniDocument document, object obj)
        {
            Instance.DeserializeInto(document, obj);
        }

        /// <summary>
        /// Serialize an object into a new <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A new instance of <see cref="IniDocument"/> containing the key/values of the object's properties.</returns>
        public virtual IniDocument SerializeObject(object obj)
        {
            IniDocument document = new IniDocument();
            SerializeObjectInto(obj, document);
            return document;
        }

        /// <summary>
        /// Serialize an object into a new <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A new instance of <see cref="IniDocument"/> containing the key/values of the object's properties.</returns>
        public static IniDocument SerializeObjectToNewDocument(object obj)
        {
            return Instance.SerializeObject(obj);
        }

        /// <summary>
        /// Serialize an object into an <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="document">The <see cref="IniDocument"/> to add the properties and their values into.</param>
        public virtual void SerializeObjectInto(object obj, IniDocument document)
        {
            if (obj == null)
            {
                return;
            }
            Encryption.IEncryptionHandler originalHandler = document.EncryptionHandler;
            if (EncryptionHandler != null) document.EncryptionHandler = EncryptionHandler;

            foreach (PropertyInfoEx prop in GetProperties(obj))
            {
                if (prop.IgnoreProperty)
                {
                    continue;
                }

                IniSection section;
                if (prop.Section == null)
                {
                    section = document.GlobalSection;
                }
                else
                {
                    section = document.Sections[prop.Section];
                }
                object value = prop.GetValue(obj, null);

                if ((value is null || (value is string strValue && string.IsNullOrEmpty(strValue) == true )) 
                    && SerializeNullAndBlank == false)
                {
                    continue;
                }
                
                IniKeyValue kv = section[prop.Name];
                kv.EncryptValue = prop.EncryptValue;
                kv.QuoteValue = prop.QuoteValue;

                
                if (value != null)
                {
                    if (prop.TypeConverter?.CanConvertTo(typeof(string)) == true)
                    {
                        kv.Value = prop.TypeConverter.ConvertTo(value, typeof(string)) as string;
                    }
                    else
                    {
                        kv.Value = value.ToString();
                    }
                    continue;
                }
                kv.Value = "";

            }
            document.EncryptionHandler = originalHandler;
        }

        /// <summary>
        /// Serialize an object into an <see cref="IniDocument"/>.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="document">The <see cref="IniDocument"/> to add the properties and their values into.</param>
        public static void SerializeObjectIntoDocument(object obj, IniDocument document)
        {
            Instance.SerializeObjectInto(obj, document);
        }

        [DebuggerStepThrough]
        private ConstructorInfo GetConstructor(Type type)
        {
            if (constructorCache.ContainsKey(type))
            {
                return constructorCache[type];
            }

            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                if (constructor.GetParameters().Length == 0)
                {
                    constructorCache.Add(type, constructor);
                    return constructor;
                }
            }
            return null;
        }
        [DebuggerStepThrough]
        private PropertyInfoEx[] GetProperties(object obj)
        {
            Type type = obj.GetType();
            if (propertyCache.ContainsKey(type))
            {
                return propertyCache[type];
            }

            List<PropertyInfoEx> info = new List<PropertyInfoEx>();

            foreach (PropertyInfo item in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                PropertyInfoEx prop = new PropertyInfoEx(item, false);
                if (prop.IniProperty != null)
                {
                    info.Add(prop);
                }
            }

            foreach (PropertyInfo item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                info.Add(new PropertyInfoEx(item, true));
            }
            PropertyInfoEx[] ex = info.ToArray();
            propertyCache.Add(type, ex);
            return ex;
        }

        #endregion Methods

    }
}
