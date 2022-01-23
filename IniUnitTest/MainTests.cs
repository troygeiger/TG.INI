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
            Assert.AreEqual(obj.NumberValue, obj2.NumberValue);
            Assert.AreEqual(obj.PointValue, obj2.PointValue);
            Assert.AreEqual(obj.RectangleValue, obj2.RectangleValue);
        }

        [TestMethod]
        public void TestRoundTripWriteRead()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Written.ini");
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

            Assert.IsTrue(doc.Sections.Count == 1 && doc.Sections[0].Name == "Test");

            doc.Write(path);

            IniDocument doc2 = new IniDocument(path);


            for (int i = 0; i < doc.GlobalSection.Count; i++)
            {
                Assert.AreEqual(doc.GlobalSection[i].Value, doc2.GlobalSection[i].Value);
            }
            var test = doc.Sections["Test"];
            var test2 = doc2.Sections["Test"];
            for (int i = 0; i < test.Count; i++)
            {
                Assert.AreEqual(test[i].Value, test2[i].Value);
            }



        }

        [TestMethod]
        public void TestNullableSerialization()
        {
            NullableClass nullable = new NullableClass();

            nullable.NullInt = 3;

            IniDocument document = IniSerialization.SerializeObjectToNewDocument(nullable);

            NullableClass result = IniSerialization.DeserializeDocument<NullableClass>(document);

            Assert.AreEqual(nullable.NullInt, result.NullInt);
            Assert.AreEqual(nullable.NullBool, result.NullBool);
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
                Owner = new Person() { FirstName = "John", LastName = "Doe"}
            };
            var ini = IniSerialization.SerializeObjectToNewDocument(obj);
            ConvertObj obj2 = IniSerialization.DeserializeDocument<ConvertObj>(ini);
            Assert.AreEqual(obj.Owner.FirstName, obj2.Owner.FirstName);
            Assert.AreEqual(obj.Owner.LastName, obj2.Owner.LastName);
        }

        [TestMethod]
        public void TestSerializeNullBlankFalse()
        {
            TestObj obj = new TestObj()
            {
                GlobalEntry = "" //Test Blank
            };
            var serializer = new IniSerialization() { SerializeNullAndBlank = false };
            
            var ini = serializer.SerializeObject(obj);

            Assert.IsFalse(ini.GlobalSection.ContainsKey(nameof(TestObj.GlobalEntry)));
            Assert.IsFalse(ini.GlobalSection.ContainsKey(nameof(TestObj.IShouldStayNull)));
            Assert.IsTrue(ini.Sections["Test"].ContainsKey(nameof(TestObj.Pass)));
        }

        [TestMethod]
        public void TestDuplicateKeys()
        {

            try
            {
                var ini = IniDocument.Parse(@"GlobalEntry=""Yes""
[Test]
Pass = True
Pass = False
NumberValue = 1
ColorValue = ""Red""
PointValue = ""{X=1.1,Y=2.2}""");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void TestDuplicateSections()
        {
            try
            {
                var ini = IniDocument.Parse(@"GlobalEntry=""Yes""
[Test]
Pass = True
[Test]
Pass = False
NumberValue = 1
ColorValue = ""Red""
PointValue = ""{X=1.1,Y=2.2}""");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class TestObj
    {
        [IniQuoteValue]
        public string GlobalEntry { get; set; }

        [IniSection("Test")]
        public bool Pass { get; set; }

        [IniSection("Test")]
        public float NumberValue { get; set; }

        [Category("Test")]
        public Color ColorValue { get; set; } = Color.Transparent;

        [IniSection("Test")]
        [Category("Do not put me in a section")]
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
        public Person Owner { get; set; }
    }

    public class NullableClass
    {
        public int? NullInt { get; set; }

        public bool? NullBool { get; set; }
    }

    [TypeConverter(typeof(PersonConverter))]
    public class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class PersonConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str == null) return null;
            string[] values = str.Split('~');
            if (values.Length == 0) return null;
            return new Person() { FirstName = values[0], LastName = values[1]};
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var person = (Person)value;
            return $"{person.FirstName}~{person.LastName}";
        }
    }
}
