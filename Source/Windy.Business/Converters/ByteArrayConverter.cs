using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Windy.Domain.Contracts;

namespace Windy.Business.Converters
{
    public class ByteArrayConverter<TEntity> : IByteArrayConverter<TEntity> where TEntity : class
    {
        public byte[] ConvertToBytes(TEntity entity)
        {
            if (entity == null)
                return new byte[0];

            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                memStream.Seek(0, SeekOrigin.Begin);
                binaryFormatter.Serialize(memStream, entity);
                return memStream.ToArray();
            }
        }

        public TEntity ConvertFromBytes(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(byteArray))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var result = (TEntity)binaryFormatter.Deserialize(memoryStream);

                if (result == null)
                    return default(TEntity);

                return result;
            }
        }
    }
}
