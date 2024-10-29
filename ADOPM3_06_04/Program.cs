using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ADOPM3_06_04
{
    public class Address 
    { 
        public string Street { get; set; }
        public string PostCode { get; set; } 
    }
    public class Person
    {
        public string Name { get; set; }
        public Address currentAddress { get; set; }
        public List<Address> pastAddresses { get; set; }

        [JsonIgnore]
        public int Age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person
            {
                Name = "Stacey", Age = 35,
                currentAddress = new Address { Street = "An Address", PostCode = "A Zip Code" },
                pastAddresses = new List<Address>
                {
                  new Address { Street = "A Generic Street", PostCode = "A Generic Zip" }
                }
            };

            Console.WriteLine("Serialized");
            Console.WriteLine($"{p.Name}"); // Stacy
            Console.WriteLine($"{p.Age}"); 
            Console.WriteLine(p.currentAddress.Street);
            foreach (var item in p.pastAddresses)
            {
                Console.WriteLine(item.Street);
            }

            //Console.WriteLine();
            //Console.WriteLine(JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true }));

            using (Stream s = File.Create(fname("Example8_04.json")))
            using (TextWriter writer = new StreamWriter(s))
            {
                var sjson = JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true });
                writer.Write(sjson);
            }


            
            Person p2;
            using (Stream s = File.OpenRead(fname("Example8_04.json")))
            using (TextReader reader = new StreamReader(s))
            {

                var sjson = reader.ReadToEnd();
                p2 = JsonSerializer.Deserialize<Person>(sjson);
            }

            //Deep copy the json way
            var str = JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true });
            var p3 = JsonSerializer.Deserialize<Person>(str);


            Console.WriteLine();
            Console.WriteLine("DeSerialized");
            Console.WriteLine($"{p2.Name}"); // Stacy
            Console.WriteLine($"{p2.Age}");

            foreach (var item in p2.pastAddresses)
            {
                Console.WriteLine(item.Street);      //Note the difference
            }
            

            static string fname(string name)
            {
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                documentPath = Path.Combine(documentPath, "AOOP2", "Examples");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
                return Path.Combine(documentPath, name);
            }
        }
    }
    //Exercise:
    //1.    Experiment with property decorator [JsonIgnore], [JsonPropertyName()]
    //2.    Make all members of type Person and type Address public fields instead of properties. Seriealize and deserialize. What Happens?
}
