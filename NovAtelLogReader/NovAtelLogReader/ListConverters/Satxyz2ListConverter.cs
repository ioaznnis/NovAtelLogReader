using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "SATXYZ2")]
    class Satxyz2ListConverter : IListConverter
    {
        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataSatxyz2).Select(data =>
            {
                var satxyz2 = data as LogDataSatxyz2;
                return new DataPointSatxyz2()
                {
                    Timestamp = record.Header.Timestamp,
                    NavigationSystem = satxyz2.NavigationSystem,
                    Prn = satxyz2.Prn,
                    Satellite = String.Format("{0}{1}", satxyz2.NavigationSystem, satxyz2.Prn),
                    X = satxyz2.X,
                    Y = satxyz2.Y,
                    Z = satxyz2.Z
                };
            });
        }
    }
}
