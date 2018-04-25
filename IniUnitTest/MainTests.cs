using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TG.INI;
using TG.INI.Serialization;
using TG.INI.Encryption;
using System.Globalization;

namespace IniUnitTest
{
    [TestClass]
    public class MainTests
    {

        private IniDocument GetTestDocument(IEncryptionHandler encryptionHandler = null)
        {
            return new IniDocument(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test.ini"),
                encryptionHandler
                );
        }

        [TestMethod]
        public void TestInitializeFromFile()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Test.ini");
            IniDocument document = new IniDocument(path);
            if (!document.GlobalSection.ContainsKey("GlobalEntry"))
            {
                Assert.Fail("Missing Global");
            }

            if (!document.Sections.Contains("Test"))
            {
                Assert.Fail("Missing [test] section");
            }
            else
            {
                IniSection test = document.Sections["Test"];
                if (!test.ContainsKey("Pass"))
                {
                    Assert.Fail("Missing Pass key");
                }
            }
        }

        [TestMethod]
        public void TestDeserialize()
        {
            IniDocument document = GetTestDocument();
            TestObj obj = IniSerialization.DeserializeDocument<TestObj>(document);
        }

        [TestMethod]
        public void TestRoundTripSerialization()
        {
            TestObj obj = new TestObj()
            {
                GlobalEntry = "Hello World",
                Pass = true,
                NumberValue = 100,
                ColorValue = Color.Purple,
                PointValue = new PointF(20, 50),
                RectangleValue = new RectangleF(1, 2, 3, 4)
            };
            IniDocument doc = IniSerialization.SerializeObjectToNewDocument(obj);
            string ini = doc.ToString();
            TestObj obj2 = IniSerialization.DeserializeDocument<TestObj>(doc);
            Assert.AreEqual(obj.GlobalEntry, obj2.GlobalEntry);
            Assert.AreEqual(obj.Pass, obj2.Pass);
            Assert.AreEqual(obj.IShouldStayNull, obj2.IShouldStayNull);
            Assert.AreEqual(obj.NumberValue, obj2.NumberValue);
            Assert.AreEqual(obj.PointValue, obj2.PointValue);
            Assert.AreEqual(obj.RectangleValue, obj2.RectangleValue);
        }

        [TestMethod]
        public void TestEncryption()
        {
            EncryptType obj = new EncryptType() { Value = "Hello World" };
            IniDocument document = new IniDocument(obj);
            document.EncryptionHandler = new IniRijndaelEncryption("67A68B03CC2A4104AA153D2EDAA76BE4");
            string ini = document.ToString();
            int c = ini.IndexOf("crypto:");
            if (c <= 0)
            {
                Assert.Fail("Missing crypto: in ini string.");
            }
            else if (ini.Substring(c + 7).Length == 0)
            {
                Assert.Fail("Value was not encrypted.");
            }
            document = IniDocument.Parse(ini, document.EncryptionHandler);
            EncryptType obj2 = IniSerialization.DeserializeDocument<EncryptType>(document);
            Assert.AreEqual(obj.Value, obj2.Value);

        }

        [TestMethod]
        public void TestNullEncryption()
        {
            EncryptType obj = new EncryptType() { Value = null };
            IniDocument document = new IniDocument(obj);
            document.EncryptionHandler = new IniRijndaelEncryption("Bla");
            string ini = document.ToString();
            int c = ini.IndexOf("crypto:");
            if (c <= 0)
            {
                Assert.Fail("Missing crypto: in ini string.");
            }

            document = IniDocument.Parse(ini, document.EncryptionHandler);
            EncryptType obj2 = IniSerialization.DeserializeDocument<EncryptType>(document);
            Assert.AreEqual(string.IsNullOrEmpty(obj.Value), string.IsNullOrEmpty(obj2.Value));
        }

        //[TestMethod]
        public void TestIniEditor()
        {
            IniDocument document = GetTestDocument(new IniRijndaelEncryption("Bla"));
            document.GlobalSection["GlobalEntry"].EncryptValue = true;

            if (document.ShowEditor() == DialogResult.Cancel)
            {
                string value = document.GlobalSection["GlobalEntry"].Value;
            }
        }

        [TestMethod]
        public void TestTypeConvertion()
        {
            ConvertObj obj = new ConvertObj()
            {
                TestPoint = new Point(1, 1),
                WindowState = FormWindowState.Maximized,
                IntValue = 20
            };
            var ini = IniSerialization.SerializeObjectToNewDocument(obj);
            ConvertObj obj2 = IniSerialization.DeserializeDocument<ConvertObj>(ini);
            Assert.AreEqual(obj.IntValue, obj2.IntValue);
            Assert.AreEqual(obj.TestPoint, obj2.TestPoint);
            Assert.AreEqual(obj.WindowState, obj2.WindowState);
        }

    }

    public class TestObj
    {
        [IniQuoteValue]
        public string GlobalEntry { get; set; }

        [Category("Test")]
        public bool Pass { get; set; }

        [Category("Test")]
        public float NumberValue { get; set; }

        [Category("Test")]
        public Color ColorValue { get; set; } = Color.Transparent;

        [Category("Test")]
        public PointF PointValue { get; set; }

        public RectangleF RectangleValue { get; set; }

        public string IShouldStayNull { get; set; }


    }

    public class EncryptType
    {
        [IniEncryptValue]
        public string Value { get; set; }
    }

    public class ConvertObj
    {
        [TypeConverter(typeof(WinStateConverter))]
        public FormWindowState WindowState { get; set; }

        public Point TestPoint { get; set; }

        public int IntValue { get; set; }
    }

    public class WinStateConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            FormWindowState state;
            Enum.TryParse<FormWindowState>(value as string, out state);
            return state;
        }

    }
}
