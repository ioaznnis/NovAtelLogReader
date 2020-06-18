using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "PSRPOS", Id = 47, Fromat = ParserFromat.Binary)]
    class PsrposParser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "PSRPOS";
            record.Data.Add(new LogDataPsrpos()
            {
                Lat = BitConverter.ToDouble(data, HeaderLength + 8),
                Lon = BitConverter.ToDouble(data, HeaderLength + 16),
                Hgt = BitConverter.ToDouble(data, HeaderLength + 24),
                LatStdDev = BitConverter.ToSingle(data, HeaderLength + 40),
                LonStdDev = BitConverter.ToSingle(data, HeaderLength + 44),
                HgtStdDev = BitConverter.ToSingle(data, HeaderLength + 48)
            });
        }
    }
}
