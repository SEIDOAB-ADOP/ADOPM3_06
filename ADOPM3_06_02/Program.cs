using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace ADOPM3_06_02
{
    [XmlInclude(typeof(AUAddress))]
    [XmlInclude(typeof(USAddress))]
    public class Address
    {
        public string Street { get; set; }
        public string PostCode { get; set;}
    }

    public class USAddress : Address { }
    public class AUAddress : Address { }
  
    public class Person
    {
        [XmlElement("FirstName")]
        public string Name { get; set; }            
        public int Age { get; set; }    //Property

        [XmlArray("BestAdresses")]
        [XmlArrayItem ("GoodAdress")]
        public List<Address> pastAddresses { get; set; } = new List<Address>();

        public Person BestFriend { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person
            {
                Name = "Stacey", Age = 25,
                pastAddresses = new List<Address>
                { new USAddress { Street = "An US Street", PostCode = "An US Zip" },
                  new AUAddress { Street = "An AU Street", PostCode = "An AU Zip" },
                  new Address { Street = "A Generic Street", PostCode = "A Generic Zip" }},
                BestFriend = new Person { Name = "Bob", Age = 30}
            };

            #region XML serialize
            Console.WriteLine("Serialized");
            Console.WriteLine($"{p.Name}"); // Stacy
            foreach (var item in p.pastAddresses)
            {
                Console.WriteLine(item.GetType());
            }

            var xs = new XmlSerializer(typeof(Person));
            using (Stream s = File.Create(fname("Example8_02.xml")))
                xs.Serialize(s, p);


            
            Person p2;
            using (Stream s = File.OpenRead(fname("Example8_02.xml")))
                p2 = (Person)xs.Deserialize(s);

            Console.WriteLine();
            Console.WriteLine("XML DeSerialized");
            Console.WriteLine($"{p2.Name}"); // Stacy
            foreach (var item in p2.pastAddresses)
            {
                Console.WriteLine(item.GetType());
            }
            #endregion


            #region Json serialize
            using (Stream s = File.Create(fname("M3_06_02.json")))
            using (TextWriter writer = new StreamWriter(s))
            {
                var sjson = JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true });
                writer.Write(sjson);
            }

            Person p_des;
            using (Stream s = File.OpenRead(fname("M3_06_02.json")))
            using (TextReader reader = new StreamReader(s))
            {

                var sjson = reader.ReadToEnd();
                p_des = JsonSerializer.Deserialize<Person>(sjson);
            }

            Console.WriteLine();
            Console.WriteLine("JSON DeSerialized");
            Console.WriteLine($"{p_des.Name}"); // Stacy
            foreach (var item in p_des.pastAddresses)
            {
                Console.WriteLine(item.GetType());
            }
            #endregion


            static string fname(string name)
            {
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                documentPath = Path.Combine(documentPath, "AOOP2", "Examples");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
                return Path.Combine(documentPath, name);
            }
        }
    }
    //Exercises
    //1.    Modify the code to inlude a Nested Type "current address" and serialize it maintaining the derived type
    //2.    Modify the code rename the collection of past addresses using XmlArray and XmlArrayItem to rename outer and inner collection elements
    //      To appropirate names
}