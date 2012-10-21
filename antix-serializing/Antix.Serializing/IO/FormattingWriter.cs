using System;
using System.IO;
using System.Text;

namespace Antix.Serializing.IO
{
    public class FormattingWriter : StreamWriter
    {
        readonly IFormatProvider _formatProvider;

        public FormattingWriter(
            Stream stream,
            Encoding encoding,
            IFormatProvider formatProvider) : base(stream, encoding)
        {
            _formatProvider = formatProvider;
        }

        public override IFormatProvider FormatProvider
        {
            get { return _formatProvider; }
        }
    }
}