using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "ISMRAWTEC", Id = 1390, Fromat = ParserFromat.Binary)]
    class IsmrawtecParser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "ISMRAWTEC";
            var nOfObservations = BitConverter.ToUInt32(data, HeaderLength);
            for (int idx = 0; idx < nOfObservations; idx++)
            {
                var offset = HeaderLength + idx * 16;
                var system = (NavigationSystem)data[offset + 6];

                record.Data.Add(new LogDataIsmrawtec()
                {
                    Prn = data[offset + 4],
                    GloFreq = (sbyte) data[offset + 5],
                    NavigationSystem = system,
                    PrimarySignal = Util.GetSignalTypeIsm(system, data[offset + 7]),
                    SecondarySignal = Util.GetSignalTypeIsm(system, data[offset + 8]),
                    Tec = BitConverter.ToSingle(data, offset + 12)
                });
            }
        }
    }
}
