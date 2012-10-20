using System.Text;

namespace Antix.Serializing
{
    public interface ISerializerSettings
    {
        Encoding Encoding { get; set; }
        bool IncludeNulls { get; set; }
    }
}