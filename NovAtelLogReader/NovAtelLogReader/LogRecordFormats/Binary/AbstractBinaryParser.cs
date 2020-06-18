using NovAtelLogReader.LogData;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    abstract class AbstractBinaryParser : IParser
    {
        protected const int HeaderLength = 28;

        public void Parse(object payload, LogRecord record)
        {
            Parse(payload as byte[], record);
        }

        abstract public void Parse(byte[] data, LogRecord record);
    }
}
