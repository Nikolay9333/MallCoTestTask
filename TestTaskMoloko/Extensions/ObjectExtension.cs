using System.IO;
using System.Xml.Serialization;

namespace TestTaskMoloko.Extensions
{
    public static class ObjectExtension
    {
        public static string Serialize<T>(this T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
    }
}
