using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VworksAtcPlugin
{
    public static class ExtensionMethods
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            var enc = new NonUsAsciiEncoding();

            using (MemoryStream ms = new MemoryStream())
            {
                
                XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

                using (XmlTextWriter textWriter = new XmlTextWriter(ms,enc))
                {
                    XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
                    nameSpace.Add("", "");
                    xmlSerializer.Serialize(textWriter, toSerialize, nameSpace);                    
                }
                return enc.GetString(ms.ToArray());
            }                
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        //Creates an object from an XML string.
        public static object FromXml(string Xml, System.Type ObjType)
        {

            XmlSerializer ser;
            ser = new XmlSerializer(ObjType);
            StringReader stringReader;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return obj;

        }
    }

    public class NonUsAsciiEncoding : ASCIIEncoding
    {
        public override string EncodingName
        {
            get { return "ascii"; }
        }
        public override string WebName
        {
            get { return "ascii"; }
        }
        public override string BodyName
        {
            get { return "ascii"; }
        }
        public override string HeaderName
        {
            get { return "ascii"; }
        }
    }

}
