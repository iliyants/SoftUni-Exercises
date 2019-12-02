using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer.Helpers
{
    public class XMLSerializationHelper
    {
        public static T[] DeserializedCollection<T>(string rootAttribute, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            T currentClass = (T)Activator.CreateInstance(typeof(T));

            var typeDeserialization = (T[])serializer
            .Deserialize(new StringReader(inputXml));

            return typeDeserialization;
        }

        public static string SerializeCollectionToXML<T>(string rootAttribute, T[] collection)
        {
            var serializer = new XmlSerializer(typeof(T[]),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), collection, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string SerializeObjectToXML<T>(string rootAttribute, T currentObject)
        {
            var serializer = new XmlSerializer(typeof(T),
                       new XmlRootAttribute(rootAttribute));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), currentObject, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
