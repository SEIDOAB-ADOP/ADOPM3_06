using System;
using System.IO;
using System.Xml.Serialization;

namespace ADOPM3_06_01
{
    [XmlRoot("Candidate", Namespace = "http://mynamespace/test/")]
    public class Person
    {
        [XmlElement("FirstName")] 
        public string Name { get; set; }

        [XmlAttribute("RoughAge")] 
        public int Age { get; set; }
 
        [XmlIgnore()] 
        public DateTime Birthday { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person { Name = "Stacey", Age = 30, Birthday = DateTime.Parse("1980.02.05") };

            var xs = new XmlSerializer(typeof(Person));

            using (Stream s = File.Create(fname("Example8_01.xml")))
                xs.Serialize(s, p);

            Person p2;
            using (Stream s = File.OpenRead(fname("Example8_01.xml")))
                p2 = (Person)xs.Deserialize(s);

            Console.WriteLine(fname("Example8_01.xml"));
            Console.WriteLine($"{p2.Name} {p2.Age}"); // Stacy 30
            Console.WriteLine(p2.Birthday); // 0001-01-01 which is default date


            static string fname(string name)
            {
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                documentPath = Path.Combine(documentPath, "AOOP2", "Examples");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
                return Path.Combine(documentPath, name);
            }
        }
    }
    //Exercises:
    //1.    Modify code to experiment with XmlElement, XmlAttribute, and XmlIgnore
    //2.    What happens if you remove XmlRoot?
    //2.    Try to Serialize a private member. What happens?
}
