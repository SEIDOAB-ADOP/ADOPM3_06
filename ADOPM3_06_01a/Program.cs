using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace ADOPM3_06_01a;

public class Person
{
    [XmlElement("FirstName")]
    public string Name { get; set; }

    [XmlAttribute]
    public int Age { get; set; }

    [XmlIgnore]
    public DateTime Birthday { get; set; }

    public string LastName { get; set; } = "Hello";

}

class Program
{
    static void Main(string[] args)
    {
        Person p = new Person { Name = "Stacey", Age = 30, Birthday = DateTime.Parse("1980.02.05") };

        #region json serialize
        using (Stream s = File.Create(fname("M3_06_01.json")))
        using (TextWriter writer = new StreamWriter(s))
        {
            var sjson = JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true });
            writer.Write(sjson);
        }

        Person p_des;
        using (Stream s = File.OpenRead(fname("M3_06_01.json")))
        using (TextReader reader = new StreamReader(s))
        {

            var sjson = reader.ReadToEnd();
            p_des = JsonSerializer.Deserialize<Person>(sjson);
        }
        #endregion

        #region xml serialize
        var xs = new XmlSerializer(typeof(Person));

        using (Stream s = File.Create(fname("M3_06_01.xml")))
            xs.Serialize(s, p);

        Person p2;
        using (Stream s = File.OpenRead(fname("M3_06_01.xml")))
            p2 = (Person)xs.Deserialize(s);
        #endregion
    }

    static string fname(string name)
    {
        var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        documentPath = Path.Combine(documentPath, "ADOP", "Examples");
        if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
        return Path.Combine(documentPath, name);
    }
}

