using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "RANGE")]
    class RangeListConverter : IListConverter
    {
        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataRange).Select(data =>
            {
                var range = data as LogDataRange;
                return new DataPointRange()
                {
                    Timestamp = record.Header.Timestamp,
                    NavigationSystem = range.NavigationSystem,
                    SignalType = range.SignalType,
                    Prn = range.Prn,
                    Satellite = String.Format("{0}{1}", range.NavigationSystem, range.Prn),
                    GloFreq = range.GloFreq,
                    Adr = range.Adr,
                    Psr = range.Psr,
                    LockTime = range.LockTime,
                    CNo = range.CNo
                };
            }).Where(range => range.IsValid());
        }
    }
}
