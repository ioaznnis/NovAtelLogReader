using NLog;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NovAtelLogReader.LogRecordFormats
{
    class BinaryLogRecordFormat : ILogRecordFormat
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<int, IParser> _parsers = new Dictionary<int, IParser>();

        public BinaryLogRecordFormat()
        {
            foreach (var type in Util.GetTypesWithAttribute<ParserAttribute>())
            {
                var attr = type.GetCustomAttribute<ParserAttribute>();

                if (attr.Fromat == ParserFromat.Binary)
                {
                    _parsers.Add(attr.Id, (IParser)Activator.CreateInstance(type));
                }
            }
        }

        public LogRecord ExtrcatLogRecord(byte[] data)
        {
            var messageId = BitConverter.ToUInt16(data, 4);
            LogRecord record = new LogRecord();
            record.Header = new LogHeader();
            record.Data = new List<LogDataBase>();
            record.Header.Timestamp = Util.GpsToUtcTime(BitConverter.ToUInt16(data, 14), BitConverter.ToUInt32(data, 16));
            _parsers[messageId].Parse(data, record);
            _logger.Trace("Новое сообщение {0} @ {1}", messageId, record.Header.Timestamp);
            return record;
        }
    }
}
