using System.Xml.Serialization;

namespace HelvyTools.Utils.Xml
{
    public static class XmlUtils
    {
        public static string Serialize<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StringWriter())
            {
                serializer.Serialize(stream, value);
                return stream.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StringReader(xml))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
