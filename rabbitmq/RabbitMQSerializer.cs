using messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace rabbitmq
{
    /// <summary>
    /// Utility library to help on serialization when binary messages are used
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RabbitMQSerializer<T> where T: IGenericMessage
    {
        public T DeserialiseFromXml(byte[] messageBody)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(messageBody, 0, messageBody.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(T));
            return (T)xmlSerialiser.Deserialize(memoryStream);
        }

        public T DeserialiseFromBinary(byte[] messageBody)
        {
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(messageBody, 0, messageBody.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (T)binaryFormatter.Deserialize(memoryStream);
        }

        public byte[] SerialiseIntoXml(T instance)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerialiser = new XmlSerializer(instance.GetType());
            xmlSerialiser.Serialize(memoryStream, instance);
            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.GetBuffer();
        }
        public byte[] SerialiseIntoBinary(T instance)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, instance);
            memoryStream.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.GetBuffer();
        }

    }
}
