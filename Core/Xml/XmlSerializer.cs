namespace Core.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XMLSerializeManager
    {
        private static readonly Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();
        public static bool Verbose;
        private static bool m_Serializing;
        private static bool m_Deserializing;

        public static bool Serializing
        {
            get { return m_Serializing; }
        }

        public static bool Deserializing
        {
            get { return m_Deserializing; }
        }

        public static XmlSerializer GetSerializer(Type t)
        {
            if (!serializers.ContainsKey(t))
            {
                serializers[t] = new XmlSerializer(t);
            }

            return serializers[t];
        }

        /// <summary>
        ///   To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name = "characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return constructedString;
        }

        /// <summary>
        ///   Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name = "pXmlString"></param>
        /// <returns></returns>
        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        public static MemoryStream SerializeObjectToMemoryStream(object pObject, Type pType)
        {
            var memoryStream = new MemoryStream();
            var xs = GetSerializer(pType);
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            var xmlns = new XmlSerializerNamespaces();
            m_Serializing = true;
            xs.Serialize(xmlTextWriter, pObject, xmlns);
            m_Serializing = false;
            return (MemoryStream) xmlTextWriter.BaseStream;
        }

        public static string SerializeObject(object pObject, Type pType)
        {
            return UTF8ByteArrayToString(SerializeObjectToMemoryStream(pObject, pType).ToArray());
        }

        public static void SerializeObjectToFile(object pObject, Type pType, string file_path)
        {
            SerializeObjectToFile(pObject, pType, file_path, true);
        }

        public static void SerializeObjectToFile(object pObject, Type pType, string file_path, bool useSafeOverwrite)
        {
            var streamPath = useSafeOverwrite ? file_path + ".tmp" : file_path;
            var success = false;
            var directory = Path.GetDirectoryName(file_path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fs = new FileStream(streamPath, FileMode.Create);
            var xs = GetSerializer(pType);
            var xmlns = new XmlSerializerNamespaces();

// xmlns.Add("", "");
            try
            {
                m_Serializing = true;
                xs.Serialize(fs, pObject, xmlns);
                success = true;
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show("Couldn't serialize to: " + file_path + "\n" + e);
#else
                System.Windows.Forms.MessageBox.Show("Failed to Save Document: "+file_path);
#endif
            }
            finally
            {
                m_Serializing = false;
            }

            fs.Close();

            if (success && useSafeOverwrite)
            {
                try
                {
                    File.Delete(file_path);
                    File.Move(streamPath, file_path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Warning, could not overwrite: " + file_path + "\n" + ex.Message + "\n\nSaved as: " +
                                    streamPath);
                }
            }
        }

        public static MemoryStream DeserializeStringToMemoryStream(string pXmlizedString)
        {
            return new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        }

        public static object DeserializeObject(MemoryStream stream, Type pType)
        {
            var xs = GetSerializer(pType);
            try
            {
                m_Deserializing = true;
                return xs.Deserialize(stream);
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show("Couldn't deserialize stream:\n" + e);
#endif
                return null;
            }
            finally
            {
                m_Deserializing = false;
            }
        }

        public static object DeserializeObject(string pXmlizedString, Type pType)
        {
            return DeserializeObject(DeserializeStringToMemoryStream(pXmlizedString), pType);
        }

        public static object DeserializeObjectFromFile(string file_path, Type pType)
        {
            var xs = GetSerializer(pType);
            var fs = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            m_Deserializing = true;
            var ans = xs.Deserialize(fs);
            m_Deserializing = false;
            fs.Close();
            return ans;
        }

        internal static void Report(string message)
        {
            if (Verbose)
            {
                Debug.Print(message);
            }
        }
    }

    public class XmlSerializer<T>
    {
        public static string SerializeObject(T obj)
        {
            return XMLSerializeManager.SerializeObject(obj, typeof (T));
        }

        public static MemoryStream SerializeObjectToMemoryStream(T obj)
        {
            return XMLSerializeManager.SerializeObjectToMemoryStream(obj, typeof (T));
        }

        public static void SerializeObjectToFile(T obj, string file_path, bool useSafeOverwrite)
        {
            XMLSerializeManager.SerializeObjectToFile(obj, typeof (T), file_path, useSafeOverwrite);
        }

        public static void SerializeObjectToFile(T obj, string file_path)
        {
            XMLSerializeManager.SerializeObjectToFile(obj, typeof (T), file_path);
        }

        public static T DeserializeObject(MemoryStream stream)
        {
            return (T) XMLSerializeManager.DeserializeObject(stream, typeof (T));
        }

        public static T DeserializeObject(string xmlStr)
        {
            return (T) XMLSerializeManager.DeserializeObject(xmlStr, typeof (T));
        }

        public static T DeserializeObjectFromFile(string file_path)
        {
            return (T) XMLSerializeManager.DeserializeObjectFromFile(file_path, typeof (T));
        }
    }
}