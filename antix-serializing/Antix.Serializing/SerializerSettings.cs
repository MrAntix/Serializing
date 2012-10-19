using System.Text;

namespace Antix.Serializing
{
    public class SerializerSettings :
        IPOXSerializerSettings
    {
        public SerializerSettings()
        {
            Encoding = Encoding.Default;
        }

        public Encoding Encoding { get; set; }
    }
}