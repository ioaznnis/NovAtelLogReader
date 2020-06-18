using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "SATXYZ2", Id = 1451, Fromat = ParserFromat.Binary)]
    class Satxyz2Parser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "SATXYZ2";
            var nOfObservations = BitConverter.ToUInt32(data, HeaderLength);

            for (int idx = 0; idx < nOfObservations; idx++)
            {
                var offset = HeaderLength + idx * 72;
                record.Data.Add(new LogDataSatxyz2()
                {
                    NavigationSystem = (NavigationSystem)BitConverter.ToUInt32(data, offset + 4),
                    Prn = BitConverter.ToUInt16(data, offset + 8),
                    X = BitConverter.ToDouble(data, offset + 12),
                    Y = BitConverter.ToDouble(data, offset + 20),
                    Z = BitConverter.ToDouble(data, offset + 28)
                });
            }
        }
    }
}
