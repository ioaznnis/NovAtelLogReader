using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "PSRPOS")]
    class PsrposConverter : IListConverter
    {
        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataPsrpos).Select(data =>
            {
                var psrpos = data as LogDataPsrpos;
                var dataPoint = new DataPointPsrpos()
                {
                    Timestamp = record.Header.Timestamp,
                    Lat = psrpos.Lat,
                    Lon = psrpos.Lon,
                    Hgt = psrpos.Hgt,
                    LatStdDev = psrpos.LatStdDev,
                    LonStdDev = psrpos.LonStdDev,
                    HgtStdDev = psrpos.HgtStdDev
                };

                return dataPoint;
            });
        }
    }
}
