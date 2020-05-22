using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TG.INI.Serialization;

namespace TG.INI.Serialization
{
    [DebuggerStepThrough]
    public class PropertyInfoEx
    {
        #region Constructors

        public PropertyInfoEx(PropertyInfo info, bool isPublic)
        {
            Info = info;
            IsPublic = isPublic;
            ReadAttributes();
        }

        #endregion Constructors

        #region Properties

        public bool CanRead
        {
            get { return Info.CanRead; }
        }

        public bool CanWrite
        {
            get { return Info.CanWrite; }
        }

        public string Section { get; private set; }

        public bool EncryptValue { get; set; }

        public bool QuoteValue { get; set; }

        public bool HasNameOverride
        {
            get
            {
                return IniProperty != null && IniProperty.HasPropertyNameOverride;
            }
        }

        public bool IgnoreProperty { get; set; }

        public PropertyInfo Info { get; private set; }

        public IniPropertyAttribute IniProperty { get; set; }

        public bool IsPublic { get; private set; }

        public string Name
        {
            get { return HasNameOverride ? IniProperty.PropertyNameOverride : Info.Name; }
        }

        public Type PropertyType
        {
            get { return Info.PropertyType; }
        }

        public TypeConverter TypeConverter { get; private set; }

        #endregion Properties

        #region Methods

        internal object GetValue(object obj, object[] index)
        {
            return Info.GetValue(obj, index);
        }

        internal void SetValue(object obj, object value, object[] index)
        {
            Info.SetValue(obj, value, index);
        }

        private void ReadAttributes()
        {
            bool hasSection = false;

            foreach (object att in Info.GetCustomAttributes(false))
            {
                TypeConverterAttribute tconvt = att as TypeConverterAttribute;

                if (!string.IsNullOrEmpty(tconvt?.ConverterTypeName))
                {
                    try
                    {
                        Type ctype = Type.GetType(tconvt.ConverterTypeName);
                        TypeConverter = Activator.CreateInstance(ctype) as TypeConverter;
                    }
                    catch (Exception) { }
                }

                IniSectionAttribute sec = att as IniSectionAttribute;

                if (sec != null)
                {
                    hasSection = true;
                    Section = sec.SectionName;
                }

                if (!hasSection)
                {
                    CategoryAttribute cat = att as CategoryAttribute;

                    if (cat != null)
                    {
                        Section = cat.Category;
                    } 
                }

                if (IniProperty == null)
                {
                    IniProperty = att as IniPropertyAttribute;
                }
                IniIgnorePropertyAttribute iniIgnore = att as IniIgnorePropertyAttribute;
                if (iniIgnore != null)
                {
                    IgnoreProperty = iniIgnore.Ignore;
                }
                else if (att is IniEncryptValueAttribute)
                {
                    EncryptValue = true;
                }
                else if (att is IniQuoteValueAttribute)
                {
                    QuoteValue = true;
                }
            }

            if (TypeConverter == null)
            {
                foreach (object att in Info.PropertyType.GetCustomAttributes(true))
                {
                    TypeConverterAttribute tconvt = att as TypeConverterAttribute;

                    if (!string.IsNullOrEmpty(tconvt?.ConverterTypeName))
                    {
                        try
                        {
                            Type ctype = Type.GetType(tconvt.ConverterTypeName);
                            TypeConverter = Activator.CreateInstance(ctype) as TypeConverter;
                        }
                        catch (Exception) { }
                    }
                }
            }

        }

        #endregion Methods
    }
}
