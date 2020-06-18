using NovAtelLogReader.LogData;

namespace NovAtelLogReader.LogRecordFormats
{
    interface ILogRecordFormat
    {
        LogRecord ExtrcatLogRecord(byte[] data);
    }
}
