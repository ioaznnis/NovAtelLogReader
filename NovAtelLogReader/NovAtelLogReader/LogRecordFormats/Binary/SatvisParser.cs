using NovAtelLogReader.LogData;
using System;

namespace NovAtelLogReader.LogRecordFormats.Binary
{
    [Parser(Name = "SATVIS", Id = 48, Fromat = ParserFromat.Binary)]
    class SatvisParser : AbstractBinaryParser
    {
        public override void Parse(byte[] data, LogRecord record)
        {
            record.Header.Name = "SATVIS";
            var nOfObservations = BitConverter.ToUInt32(data, HeaderLength + 8);
            for (int idx = 0; idx < nOfObservations; idx++)
            {
                var offset = HeaderLength + idx * 40;
                LogDataSatvis satvis = new LogDataSatvis()
                {
                    SatVis = BitConverter.ToInt32(data, HeaderLength) == 1,
                    Prn = BitConverter.ToUInt16(data, offset + 12),
                    GloFreq = BitConverter.ToUInt16(data, offset + 14),
                    Health = BitConverter.ToUInt32(data, offset + 16),
                    Elev = BitConverter.ToDouble(data, offset + 20),
                    Az = BitConverter.ToDouble(data, offset + 28)
                };
                satvis.NavigationSystem = Util.GetNavigationSystemByPrn(satvis.Prn);

                if (satvis.NavigationSystem == NavigationSystem.GLONASS)
                {
                    satvis.Prn = Util.GetActualPrn(satvis.Prn);
                    satvis.GloFreq = Util.GetActualGlonassFrequency(satvis.GloFreq);
                }

                record.Data.Add(satvis);
            }
        }
    }
}
