using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ADOPM3_06_05
{
    public class Person
    {
        public string Message{ get; set; }
        public string Name { get; set; }
        
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime Born { get; set; }
    }
    public class UnixTimestampConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TryGetInt32(out int timestamp)) return new DateTime(1970, 1, 1).AddSeconds(timestamp);
            throw new Exception("Expected the timestamp as a number.");
        }
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            int timestamp = (int)(value - new DateTime(1970, 1, 1)).TotalSeconds;
            writer.WriteNumberValue(timestamp);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person {Name = "Anne", Message = "Hello World", Born = DateTime.Parse("1964-03-25")};

            using (Stream s = File.Create(fname("Example8_05.json")))
            using (TextWriter writer = new StreamWriter(s))
                writer.Write(JsonSerializer.Serialize<Person>(p, new JsonSerializerOptions() { WriteIndented = true }));

            Person p2;
            using (Stream s = File.OpenRead(fname("Example8_05.json")))
            using (TextReader reader = new StreamReader(s))

                p2 = JsonSerializer.Deserialize<Person>(reader.ReadToEnd());

            Console.WriteLine($"{p2.Name}"); // Stacy
            Console.WriteLine(p2.Born); // 1970-03-05

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
    //3.    Make a Base64Converter that converts Message content into a base64 string and Serialize. Convert it back to Unicode in Deserialization 
    //      Tips:
    //      https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonreader.trygetbytesfrombase64?view=net-5.0#System_Text_Json_Utf8JsonReader_TryGetBytesFromBase64_System_Byte____
    //      https://docs.microsoft.com/en-us/dotnet/api/system.text.json.utf8jsonwriter.writestringvalue?view=net-5.0#System_Text_Json_Utf8JsonWriter_WriteStringValue_System_String_
    //
    //      Base64 byte[] --> UniCode string:  Encoding.Unicode.GetString()
    //      UniCode string --> byte[]: Encoding.Unicode.GetBytes()
    //      byte[] --> Base64 string: Convert.ToBase64String()   
}
