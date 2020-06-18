using System;
using System.Collections.Generic;

namespace NovAtelLogReader.LogData
{
    public class LogRecord
    {
        public LogHeader Header { get; set; }
        public List<LogDataBase> Data { get; set; }
        public UInt32 Checksum { get; set; }
    }
}
