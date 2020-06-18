using NovAtelLogReader.LogData;

namespace NovAtelLogReader.LogRecordFormats
{
    interface IParser
    {
        void Parse(object payload, LogRecord record);
    }
}
