/// Напишите приложение, конвертирующее произвольный JSON в XML. Используйте JsonDocument.

using System.Data;
using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Xml;

/// Напишите приложение, конвертирующее произвольный JSON в XML. Используйте JsonDocument.
namespace Task1
{
    internal class Program
    {
        static void GetXmlElement(JsonElement jsonElement, XmlElement rootElement)
        {
            foreach (var property in jsonElement.EnumerateObject())
            {
                XmlElement element = rootElement.OwnerDocument.CreateElement(property.Name);
                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    rootElement.AppendChild(element);
                    GetXmlElement(property.Value, element);
                }
                else if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        XmlElement childElement = rootElement.OwnerDocument.CreateElement(property.Name);
                        childElement.InnerText = item.ToString();
                        rootElement.AppendChild(childElement);                        
                    }
                }
                else
                {
                    element.InnerText = property.Value.ToString();
                    rootElement.AppendChild(element);
                }
            }          
        }
        
        static void Main(string[] args)
        {
            string jsonString = """
                    {
                        "Id": 1,
                        "Email": "james@example.com",
                        "Active": true,
                        "CreatedDate": "2024-01-20T10:00:00Z",
                        "Roles": [
                            "User",
                            "Admin"
                        ],
                        "Team": {
                            "Id": 2,
                            "Name": "Software Developers",
                            "Description": "Creators of fine software products and services."
                        }
                    }
                    """;


            using JsonDocument document = JsonDocument.Parse(jsonString);
            
            // Создадим XML Документ
            XmlDocument xmlDocument = new XmlDocument();
            // Создадим и добавим корневой элемент 
            XmlElement root = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(root);

            // Выполняем основное преобразование из JSON в XML
            GetXmlElement(document.RootElement, root);

            Console.WriteLine(xmlDocument.OuterXml);
            xmlDocument.Save("output.xml");

            /// < root >< Id > 1 </ Id >< Email > james@example.com </ Email >< Active > True </ Active >< CreatedDate > 2024 - 01 - 20T10: 00:00Z </ CreatedDate >
            /// < Roles > User </ Roles >< Roles > Admin </ Roles >< Team >< Id > 2 </ Id >< Name > Software Developers </ Name >< Description > Creators of fine software products and services.
            /// </ Description ></ Team ></ root >



        }
    }
}
