using NovAtelLogReader.LogData;
using System;
using System.Runtime.Serialization;

namespace NovAtelLogReader.DataPoints
{
    [DataContract]
    [Serializable]
    [DataPoint(Name = "ISMRAWTEC", Queue = "datapoint-raw-ismrawtec")]
    public class DataPointIsmrawtec
    {
        [DataMember]
        public long Timestamp { get; set; }
        [DataMember]
        public NavigationSystem NavigationSystem { get; set; }
        [DataMember]
        public string Satellite { get; set; }
        [DataMember]
        public uint Prn { get; set; }
        [DataMember]
        public int GloFreq { get; set; }
        [DataMember]
        public SignalType PrimarySignal { get; set; }
        [DataMember]
        public SignalType SecondarySignal { get; set; }
        [DataMember]
        public double Tec { get; set; }
    }
}
