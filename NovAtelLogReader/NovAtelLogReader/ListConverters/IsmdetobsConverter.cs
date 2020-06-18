using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "ISMDETOBS")]
    class IsmdetobsListConverter : IListConverter
    {
        private static IEnumerable<object> GetIsmdetDataPoints(LogHeader header, LogDataBase data)
        {
            var ismdet = data as LogDataIsmdetobs;
            var timestamp = header.Timestamp;

            foreach (var power in ismdet.Powers)
            {
                yield return new DataPointIsmdetobs()
                {
                    Timestamp = timestamp,
                    Satellite = String.Format("{0}{1}", ismdet.NavigationSystem, ismdet.Prn),
                    NavigationSystem = ismdet.NavigationSystem,
                    GloFreq = ismdet.GloFreq,
                    Prn = ismdet.Prn,
                    SignalType = ismdet.SignalType,
                    Power = power
                };

                timestamp += 20;
            }
        }

        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataIsmdetobs).SelectMany(data => GetIsmdetDataPoints(record.Header, data));
        }
    }
}
