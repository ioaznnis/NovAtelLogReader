using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "ISMRAWTEC")]
    class IsmrawtecListConverter : IListConverter
    {
        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataIsmrawtec).Select(data =>
            {
                var ismrawtec = data as LogDataIsmrawtec;
                return new DataPointIsmrawtec()
                {
                    Timestamp = record.Header.Timestamp,
                    NavigationSystem = ismrawtec.NavigationSystem,
                    Prn = ismrawtec.Prn,
                    Satellite = String.Format("{0}{1}", ismrawtec.NavigationSystem, ismrawtec.Prn),
                    GloFreq = ismrawtec.GloFreq,
                    PrimarySignal = ismrawtec.PrimarySignal,
                    SecondarySignal = ismrawtec.SecondarySignal,
                    Tec = ismrawtec.Tec
                };
            });
        }
    }
}
