using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "RANGE", Id = 43, Fromat = ParserFromat.Binary)]
    class RangeParser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "RANGE";
            var nOfObservations = BitConverter.ToUInt32(data, HeaderLength);

            for (int idx = 0; idx < nOfObservations; idx++)
            {
                var offset = HeaderLength + idx * 44;
                var tracking = BitConverter.ToUInt32(data, offset + 44);

                var range = new LogDataRange()
                {
                    Prn = BitConverter.ToUInt16(data, offset + 4),
                    GloFreq = BitConverter.ToInt16(data, offset + 6),
                    Psr = BitConverter.ToDouble(data, offset + 8),
                    PsrStd = BitConverter.ToSingle(data, offset + 16),
                    Adr = BitConverter.ToDouble(data, offset + 20),
                    AdrStd = BitConverter.ToSingle(data, offset + 28),
                    CNo = BitConverter.ToSingle(data, offset + 36),
                    LockTime = BitConverter.ToSingle(data, offset + 40),
                    NavigationSystem = Util.GetNavigationSystem(tracking),
                    SignalType = Util.GetSignalType(tracking),
                    Tracking = tracking
                };

                if (range.NavigationSystem == NavigationSystem.GLONASS)
                {
                    range.Prn = Util.GetActualPrn(range.Prn);
                    range.GloFreq = Util.GetActualGlonassFrequency(range.GloFreq);
                }

                record.Data.Add(range);
            }
        }
    }
}
