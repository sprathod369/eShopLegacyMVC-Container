using System.IO;
using System.Text.Json;

namespace eShopLegacyMVC_Core.Services
{
    public class Serializing
    {
        public byte[] SerializeBinary(object obj)
        {
            // Serialize the object to a byte array using JSON
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }
    }
}
