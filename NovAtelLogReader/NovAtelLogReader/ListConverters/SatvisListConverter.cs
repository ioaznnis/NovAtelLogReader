using NovAtelLogReader.DataPoints;
using NovAtelLogReader.LogData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NovAtelLogReader.ListConverters
{
    [ListConverter(Name = "SATVIS")]
    class SatvisConverter : IListConverter
    {
        public IEnumerable<object> ToList(LogRecord record)
        {
            return record.Data.Where(data => data is LogDataSatvis).Select(data =>
            {
                var satvis = data as LogDataSatvis;
                return new DataPointSatvis()
                {
                    Timestamp = record.Header.Timestamp,
                    NavigationSystem = satvis.NavigationSystem,
                    Prn = satvis.Prn,
                    Satellite = String.Format("{0}{1}", satvis.NavigationSystem, satvis.Prn),
                    GloFreq = satvis.GloFreq,
                    SatVis = satvis.SatVis,
                    Health = satvis.Health,
                    Elev = satvis.Elev,
                    Az = satvis.Az
                };
            });
        }
    }
}
