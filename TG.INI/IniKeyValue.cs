using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace TG.INI
{

    /// <summary>
    /// This class represents a key/value line. Ex. Year=2015
    /// </summary>
    public class IniKeyValue : IniEntry
    {

        #region Fields

        const string cryptoPrefix = "crypto:";
        string _key = null;
        static Regex rexSysDraw = new Regex(@"(?<rect>\{X=(?<rx>[\d\.]{1,}),\s*Y=(?<ry>[\d\.]{1,}),\s*Width=(?<rw>[\d\.]{1,}),\s*Height=(?<rh>[\d\.]{1,})})|(?<size>{Width=(?<sw>[\d\.]{1,}),\s*Height=(?<sh>[\d\.]{1,})})|(?<point>{X=(?<px>[\d\.]{1,}),\s*Y=(?<py>[\d\.]{1,})})");
        static Regex rexColor = new Regex(@"^#(?<hex>(?<r>[0-9a-fA-F]{2,2})(?<g>[0-9a-fA-F]{2,2})(?<b>[0-9a-fA-F]{2,2})(?<a>[0-9a-fA-F]{2,2})?)|(?<name>\w{1,})$");

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
            : this(key, value, false, false)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key and int value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public IniKeyValue(string key, int value)
            : this(key, value.ToString(), false, false)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key, value and if the value should be encrypted.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encryptValue"></param>
        public IniKeyValue(string key, string value, bool encryptValue)
            : this(key, value, encryptValue, false)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key, value and if the value should be encrypted and quoted.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encryptValue"></param>
        /// <param name="quoteValue"></param>
        public IniKeyValue(string key, string value, bool encryptValue, bool quoteValue)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="IniKeyValue"/> with a key, value and if the value should be encrypted and quoted.
        /// </summary>
        /// <param name="parentDocumnt"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encryptValue"></param>
        /// <param name="quoteValue"></param>
        public IniKeyValue(IniDocument parentDocumnt, string key, string value, bool encryptValue, bool quoteValue)
        {
            ParentDocument = parentDocumnt;
            this.QuoteValue = quoteValue;
            this.EncryptValue = encryptValue;
            this.Key = key;
            this.Value = value;
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
                return base.Value;
            }

            set
            {
                if (value?.StartsWith(cryptoPrefix) == true)
                {
                    EncryptValue = true;
                    base.Value = DecryptString(value);
                }
                else
                {
                    base.Value = value;
                }
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
        /// Gets or Sets the value as an byte type.
        /// </summary>
        public byte ValueByte
        {
            get
            {
                byte i = 0;
                byte.TryParse(Value, out i);
                return i;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or Sets the value as a short type.
        /// </summary>
        public short ValueInt16
        {
            get
            {
                short s = -1;
                short.TryParse(Value, out s);
                return s;
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
        /// Get or set a <see cref="Point"/> value.
        /// </summary>
        public Point ValuePoint
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["point"]) != null)
                {
                    int x = 0, y = 0;
                    int.TryParse(m.Groups["px"].Value, out x);
                    int.TryParse(m.Groups["py"].Value, out y);
                    return new Point(x, y);
                }
                return Point.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="PointF"/> value.
        /// </summary>
        public PointF ValuePointF
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["point"]) != null)
                {
                    float x = 0, y = 0;
                    float.TryParse(m.Groups["px"].Value, out x);
                    float.TryParse(m.Groups["py"].Value, out y);
                    return new PointF(x, y);
                }
                return PointF.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="Size"/> value.
        /// </summary>
        public Size ValueSize
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["size"]) != null)
                {
                    int w = 0, h = 0;
                    int.TryParse(m.Groups["sw"].Value, out w);
                    int.TryParse(m.Groups["sh"].Value, out h);
                    return new Size(w, h);
                }
                return Size.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="SizeF"/> value.
        /// </summary>
        public SizeF ValueSizeF
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["size"]) != null)
                {
                    float w = 0, h = 0;
                    float.TryParse(m.Groups["sw"].Value, out w);
                    float.TryParse(m.Groups["sh"].Value, out h);
                    return new SizeF(w, h);
                }
                return SizeF.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="Rectangle"/> value.
        /// </summary>
        public Rectangle ValueRectangle
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["rect"]) != null)
                {
                    int x = 0, y = 0, w = 0, h = 0;
                    int.TryParse(m.Groups["rx"].Value, out x);
                    int.TryParse(m.Groups["ry"].Value, out y);
                    int.TryParse(m.Groups["rw"].Value, out w);
                    int.TryParse(m.Groups["rh"].Value, out h);
                    return new Rectangle(x, y, w, h);
                }
                return Rectangle.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="RectangleF"/> value.
        /// </summary>
        public RectangleF ValueRectangleF
        {
            get
            {
                Match m = rexSysDraw.Match(Value);

                if (m.Success && (m.Groups["rect"]) != null)
                {
                    float x = 0, y = 0, w = 0, h = 0;
                    float.TryParse(m.Groups["rx"].Value, out x);
                    float.TryParse(m.Groups["ry"].Value, out y);
                    float.TryParse(m.Groups["rw"].Value, out w);
                    float.TryParse(m.Groups["rh"].Value, out h);
                    return new RectangleF(x, y, w, h);
                }
                return RectangleF.Empty;
            }
            set
            {
                Value = value.ToString();
            }
        }

        /// <summary>
        /// Get or set a <see cref="Color"/> value.
        /// </summary>
        public Color ValueColor
        {
            get
            {
                Match m = rexColor.Match(Value);
                if (m.Success)
                {
                    if (m.Groups["hex"].Length > 0)
                    {
                        int r, g, b, a = 255;
                        r = int.Parse(m.Groups["r"].Value, System.Globalization.NumberStyles.HexNumber);
                        g = int.Parse(m.Groups["g"].Value, System.Globalization.NumberStyles.HexNumber);
                        b = int.Parse(m.Groups["b"].Value, System.Globalization.NumberStyles.HexNumber);
                        if (m.Groups.Count == 5)
                        {
                            a = int.Parse(m.Groups["a"].Value, System.Globalization.NumberStyles.HexNumber);
                        }
                        return Color.FromArgb(r, g, b, a);
                    }
                    else
                    {
                        try
                        {
                            return Color.FromName(m.Groups["name"].Value);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                return Color.Transparent;
            }
            set
            {
                if (value.IsNamedColor)
                {
                    Value = $"#{value.Name}";
                }
                else
                {
                    Value = $"#{value.R.ToString("X2")}{value.G.ToString("X2")}{value.B.ToString("X2")}{(value.A < 0 ? value.A.ToString("X2") : "")}";
                }

            }
        }

        /// <summary>
        /// Gets or Sets if the value should be encrypted.
        /// </summary>
        public bool EncryptValue { get; set; }

        #endregion Properties

        #region Methods

        private string EncryptString(string value)
        {
            return cryptoPrefix +
                ParentDocument?.EncryptionHandler?.EncryptBase64(value) ?? value;
        }

        private string DecryptString(string value)
        {
            if (value?.StartsWith(cryptoPrefix) == true)
            {
                return ParentDocument?.EncryptionHandler?
                    .DecryptBase64(value.Substring(cryptoPrefix.Length));
            }
            else
            {
                return ParentDocument?.EncryptionHandler?.DecryptBase64(value);
            }
        }

        #endregion Methods

        /// <summary>
        /// Returns the INI key/value output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.EncryptValue && ParentDocument?.HasEncryptionHandler == true)
                return QuoteValue || ParentDocument?.QuoteAllValues == true
                    ? $"{Key}=\"{EncryptString(Value)}\""
                    : $"{Key}={EncryptString(Value)}";
            else
                return QuoteValue || ParentDocument?.QuoteAllValues == true ? $"{Key}=\"{base.Value}\"" : $"{Key}={base.Value}";

        }
    }
}