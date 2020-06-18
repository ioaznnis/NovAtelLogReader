using NovAtelLogReader.LogData;
using System;
using System.Globalization;

namespace NovAtelLogReader.LogRecordFormats.Ascii
{
    [Parser(Name = "ISMREDOBS", Id = 1393, Fromat = ParserFromat.Ascii)]
    class IsmredobsParser : AbstractAsciiParser
    {
        public override void Parse(string[] body, LogRecord record)
        {
            long nOfObservations = Int64.Parse(body[0]);

            long offset = 1;
            long rangeFields = 17;
            long maxIndex = rangeFields * nOfObservations + offset;

            while (offset < maxIndex)
            {
                var navigationSystem = (NavigationSystem)UInt32.Parse(body[offset + 2]);

                record.Data.Add(new LogDataIsmredobs()
                {
                    NavigationSystem = navigationSystem,
                    Prn = UInt32.Parse(body[offset]),
                    GloFreq = Int32.Parse(body[offset + 1]),
                    SignalType = Util.GetSignalTypeIsm(navigationSystem, UInt32.Parse(body[offset + 3])),
                    AverageCmc = Double.Parse(body[offset + 8], CultureInfo.InvariantCulture),
                    CmcStdDev = Double.Parse(body[offset + 9], CultureInfo.InvariantCulture),
                    TotalS4 = Double.Parse(body[offset + 10], CultureInfo.InvariantCulture),
                    CorrS4 = Double.Parse(body[offset + 11], CultureInfo.InvariantCulture),
                    PhaseSigma1Second = Double.Parse(body[offset + 12], CultureInfo.InvariantCulture),
                    PhaseSigma30Second = Double.Parse(body[offset + 15], CultureInfo.InvariantCulture),
                    PhaseSigma60Second = Double.Parse(body[offset + 16], CultureInfo.InvariantCulture)
                });

                offset += rangeFields;
            }
        }
    }
}
