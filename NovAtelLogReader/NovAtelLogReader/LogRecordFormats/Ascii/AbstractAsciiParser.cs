using System;
using NovAtelLogReader.LogData;

namespace NovAtelLogReader.LogRecordFormats.Ascii
{
    abstract class AbstractAsciiParser : IParser
    {
        public void Parse(object payload, LogRecord record)
        {
            Parse(payload as string[], record);
        }

        abstract public void Parse(string[] body, LogRecord record);
    }
}
