using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ服務器加客戶端測試例子
{
    class StringCodeChange
    {
        public static byte[] SerializeObject(object obj)
        {
            byte[] data;

            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                data = stream.ToArray();
            }

            return data;
        }

        public static T DeserializeObject<T>(byte[] data)
        {
            return DeserializeObject<T>(data, 0 ,data.Length);
        }
        private static T DeserializeObject<T>(byte[] data, int dataOffset, int dataLength)
        {
            T obj = default(T);

            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(data, dataOffset, dataLength))
            {
                obj = (T)formatter.Deserialize(stream);
            }

            return obj;
        }
    }
}
