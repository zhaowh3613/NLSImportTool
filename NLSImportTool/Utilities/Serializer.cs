using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NLSImportTool.Utilities
{
    /// <summary>
    /// Utility for serializing and deserializing objects to and from XML
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Converts XML into an object (that implements DataContract / IXmlSerializable)
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="xml">XML that represents the object</param>
        /// <returns>Instantiated instance</returns>
        public static T Deserialize<T>(string xml)
        {
            return XmlStringSerializer.Deserialize<T>(xml);
            //return DataContractStringSerializer.Deserialize<T>(xml);
        }

        /// <summary>
        /// Converts an object (that implements DataContract / IXmlSerializable) into XML
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="instance">Instance of type T</param>
        /// <returns>The serialized XML</returns>
        public static string Serialize<T>(T instance)
        {
            return XmlStringSerializer.Serialize<T>(instance);
            //return DataContractStringSerializer.Serialize<T>(instance);
        }
    }

    /// <summary>
    /// Utility for serializing and deserializing objects to and from XML
    /// </summary>
    public static class DataContractStringSerializer
    {
        /// <summary>
        /// Converts XML into an object (that implements DataContract / IXmlSerializable)
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="xml">XML that represents the object</param>
        /// <returns>Instantiated instance</returns>
        public static T Deserialize<T>(string xml)
        {
            T obj = default(T);
            var readerSettings = new XmlReaderSettings()
            {
                DtdProcessing = System.Xml.DtdProcessing.Ignore,
                XmlResolver = null,
                CheckCharacters = false,
                IgnoreWhitespace = true,
            };

            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader, readerSettings))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    obj = (T)serializer.ReadObject(xmlReader);
                }
            }

            return obj;
        }

        /// <summary>
        /// Converts an object (that implements DataContract / IXmlSerializable) into XML
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="instance">Instance of type T</param>
        /// <returns>The serialized XML</returns>
        public static string Serialize<T>(T instance)
        {
            string xml = null;
            var settings = new DataContractSerializerSettings()
            {
                PreserveObjectReferences = false,
            };

            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = new UTF8Encoding(false, false), // Prevent BOM! Very important
                // Encoding = new UnicodeEncoding(false, false), // Prevent BOM! Very important
            };

            using (var stringWriter = new StringWriter())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), settings);
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.WriteObject(xmlWriter, instance);
                }
                xml = stringWriter.ToString();
            }
            return xml;
        }
    }



    /// <summary>
    /// Wrapper for XMLSerializer
    /// </summary>
    internal static class XmlStringSerializer
    {
        /// <summary>
        /// Converts XML into an object (that implements DataContract / IXmlSerializable)
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="xml">XML that represents the object</param>
        /// <returns>Instantiated instance</returns>
        public static T Deserialize<T>(string xml)
        {
            T obj = default(T);
            var readerSettings = new XmlReaderSettings()
            {
                DtdProcessing = System.Xml.DtdProcessing.Ignore,
                XmlResolver = null,
                CheckCharacters = false,
                IgnoreWhitespace = true,
            };

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader, readerSettings))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    obj = (T)serializer.Deserialize(xmlReader);
                }
            }

            return obj;
        }


        /// <summary>
        /// Converts an object (that implements DataContract / IXmlSerializable) into XML
        /// </summary>
        /// <typeparam name="T">Type of object to create</typeparam>
        /// <param name="instance">Instance of type T</param>
        /// <returns>The serialized XML</returns>
        internal static string Serialize<T>(T instance)
        {
            string xml = null;
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = new UTF8Encoding(false, false), // Prevent BOM! Very important
                // Encoding = new UnicodeEncoding(false, false), // Prevent BOM! Very important
            };

            using (var stringWriter = new StringWriter())
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                using (var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, instance);
                }
                xml = stringWriter.ToString();
            }
            return xml;
        }
    }
}
