using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Windy.Entitities
{
    [Serializable]
    public class WindmillData
    {
        public string Client { get; set; }

        public string MillId { get; set; }

        public double MegaWatt { get; set; }

        public static  byte[] AsByteArray(WindmillData data)
        {
            if (data == null)
                return null;

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, data);
                return memoryStream.ToArray();
            }
        }
    }
}
