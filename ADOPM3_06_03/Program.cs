using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ADOPM3_06_03
{
	public class Person
	{
		public string Name;
		public Secret mySecret;
	}
	public class Secret : IXmlSerializable
	{
		private string aSecret; // Note that the field is private
		
		
		public XmlSchema GetSchema() => null;

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			aSecret = reader.ReadElementContentAsString("aSecret", "");
			reader.ReadEndElement();
		}
		public void WriteXml(XmlWriter writer)
		{
			writer.WriteElementString("aSecret", aSecret);
		}
		

		public void SetSecret ()
        {
			aSecret = "A private field";
		}
	}
	class Program
    {
        static void Main(string[] args)
        {
			Person p = new Person { Name = "Anne", mySecret = new Secret()};
			p.mySecret.SetSecret();

			var xs = new XmlSerializer(typeof(Person));
			using (Stream s = File.Create(fname("Example8_03.xml")))
				xs.Serialize(s, p);

			
			Person p2;
			using (Stream s = File.OpenRead(fname("Example8_03.xml")))
				p2 = (Person)xs.Deserialize(s);

			Console.WriteLine($"{p2.Name}"); // Anne
			
			static string fname(string name)
			{
				var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				documentPath = Path.Combine(documentPath, "AOOP2", "Examples");
				if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
				return Path.Combine(documentPath, name);
			}
		}
	}
	//Exercise:
	//1.	Investigate p2 with debugger to ensure the private field is deserialized. 
	//2.	Add a public member to the Secret and serialize it 
}
