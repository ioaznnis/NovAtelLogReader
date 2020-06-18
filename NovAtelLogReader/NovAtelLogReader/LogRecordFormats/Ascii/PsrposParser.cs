using NovAtelLogReader.LogData;
using System;
using System.Globalization;

namespace NovAtelLogReader.LogRecordFormats.Ascii
{
    [Parser(Name = "PSRPOS", Id = 47, Fromat = ParserFromat.Ascii)]
    class PsrposParser : AbstractAsciiParser
    {
        public override void Parse(string[] body, LogRecord record)
        {
            record.Data.Add(new LogDataPsrpos()
            {
                Lat = Double.Parse(body[2], CultureInfo.InvariantCulture),
                Lon = Double.Parse(body[3], CultureInfo.InvariantCulture),
                Hgt = Double.Parse(body[4], CultureInfo.InvariantCulture),
                LatStdDev = Double.Parse(body[7], CultureInfo.InvariantCulture),
                LonStdDev = Double.Parse(body[8], CultureInfo.InvariantCulture),
                HgtStdDev = Double.Parse(body[9], CultureInfo.InvariantCulture)
            });
        }
    }
}
