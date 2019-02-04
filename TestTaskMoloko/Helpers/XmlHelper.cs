using System.IO;
using System.Xml.Serialization;

namespace TestTaskMoloko.Helpers
{
    public static class XmlHelper
    {
        public static T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (T) serializer.Deserialize(reader);
            }

            return result;
        }
    }
}