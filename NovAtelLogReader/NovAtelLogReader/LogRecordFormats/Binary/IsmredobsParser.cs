using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "ISMREDOBS", Id = 1393, Fromat = ParserFromat.Binary)]
    class IsmredobsParser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "ISMREDOBS";
            var nOfObservations = BitConverter.ToUInt32(data, HeaderLength);

            for (int idx = 0; idx < nOfObservations; idx++)
            {
                var offset = HeaderLength + idx * 56;
                var navigationSystem = (NavigationSystem)data[offset + 6];

                record.Data.Add(new LogDataIsmredobs()
                {
                    NavigationSystem = navigationSystem,
                    Prn = data[offset + 4],
                    GloFreq = (sbyte) data[offset + 5],
                    SignalType = Util.GetSignalTypeIsm(navigationSystem, data[offset + 7]),
                    AverageCmc = BitConverter.ToSingle(data, offset + 24),
                    CmcStdDev = BitConverter.ToSingle(data, offset + 28),
                    TotalS4 = BitConverter.ToSingle(data, offset + 32),
                    CorrS4 = BitConverter.ToSingle(data, offset + 36),
                    PhaseSigma1Second = BitConverter.ToSingle(data, offset + 40),
                    PhaseSigma30Second = BitConverter.ToSingle(data, offset + 52),
                    PhaseSigma60Second = BitConverter.ToSingle(data, offset + 56)
                });
            }
        }
    }
}
