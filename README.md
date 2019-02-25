# TG.INI
Reads and writes INI files and make navigating them easy.<br/>
It provides serialization, encryption of values and a built in editor by calling the ShowEditor method.

## Example INI
GlobalEntry="Yes"<br/>
[Test]<br/>
Pass=True<br/>
NumberValue=1<br/>
ColorValue="Red"<br/>
PointValue="{X=1.1,Y=2.2}"<br/>

## Loading From File, Get and Set Values
```
IniDocument document = new IniDocument(path);
bool passValue = document.GetKeyValue("Test/Pass")?.ValueBoolean ?? false;
Color colorValue = document.GetKeyValue("Test/ColorValue")?.ValueColor ?? Color.Black;

document["Test"]["Pass"].ValueBoolean = true;
document.Write(path);
```

## Serialize and De-Serialize Objects
```
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
TestObj obj2 = IniSerialization.DeserializeDocument<TestObj>(doc);
```

## Encryption
```
IniDocument document = new IniDocument(obj);
document["Secrets"].AddKeyValue("MyPassword", "<My Secret>", true, true);
document.EncryptionHandler = new IniRijndaelEncryption("Secret Key");
document.Write(path);
```
