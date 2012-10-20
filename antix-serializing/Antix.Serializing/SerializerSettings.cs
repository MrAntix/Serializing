using System.Text;

namespace Antix.Serializing
{
    public class SerializerSettings :
        ISerializerSettings
    {
        public SerializerSettings()
        {
            Encoding = Encoding.Default;
            IncludeNulls = false;
        }

        public Encoding Encoding { get; set; }

        public bool IncludeNulls { get; set; }
    }
}