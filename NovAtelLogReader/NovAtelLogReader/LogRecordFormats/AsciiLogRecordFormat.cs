using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NovAtelLogReader.LogData;
using NLog;
using System.Reflection;

namespace NovAtelLogReader.LogRecordFormats
{
    class AsciiLogRecordFormat : ILogRecordFormat
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, IParser> _parsers = new Dictionary<string, IParser>();

        public AsciiLogRecordFormat()
        {
            foreach (var type in Util.GetTypesWithAttribute<ParserAttribute>())
            {
                var attr = type.GetCustomAttribute<ParserAttribute>();

                if (attr.Fromat == ParserFromat.Ascii)
                {
                    _parsers.Add(attr.Name, (IParser)Activator.CreateInstance(type));
                }
            }
        }

        public LogRecord ExtrcatLogRecord(byte[] data)
        {
            return ParseString(Encoding.ASCII.GetString(data));
        }

        private LogRecord ParseString(string data)
        {
            var parts = data.Split(new char[] { ';', '*' });

            if (parts.Length != 3 || !parts[0].StartsWith("#") || parts[0].EndsWith("A"))
            {
                _logger.Error("Неверный формат лога");
                throw new InvalidOperationException("Wrong log line format");
            }

            var header = parts[0].Split(',');
            var body = parts[1].Split(',');
            var checksum = parts[2];

            var record = new LogRecord()
            {
                Header = new LogHeader(),
                Data = new List<LogDataBase>()
            };

            // Удалить # в начале и A в конце
            record.Header.Name = header[0].Remove(header[0].Length - 1).Remove(0, 1);
            record.Header.Port = header[1];

            // Рассчитать время
            var week = Int32.Parse(header[5]);
            var ms = Convert.ToInt64(Double.Parse(header[6], CultureInfo.InvariantCulture) * 1000);
            record.Header.Timestamp = Util.GpsToUtcTime(week, ms);

            _parsers[record.Header.Name].Parse(body, record);
            return record;
        }
    }
}
