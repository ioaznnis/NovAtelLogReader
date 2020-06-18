using System;
using System.Runtime.Serialization;
using NovAtelLogReader.LogData;

namespace NovAtelLogReader.DataPoints
{
    [DataContract]
    [Serializable]
    [DataPoint(Name = "RANGE", Queue = "datapoint-raw-range")]
    public class DataPointRange
    {
        [DataMember]
        public long Timestamp { get; set;  }
        [DataMember]
        public NavigationSystem NavigationSystem { get; set; }
        [DataMember]
        public SignalType SignalType { get; set; }
        [DataMember]
        public string Satellite { get; set; }
        [DataMember]
        public uint Prn { get; set; }
        [DataMember]
        public int GloFreq { get; set; }
        [DataMember]
        public double Psr { get; set; }
        [DataMember]
        public double Adr { get; set; }
        [DataMember]
        public double CNo { get; set; }
        [DataMember]
        public double LockTime { get; set; }
        [DataMember]
        public double Power { get; set; }

        public bool IsValid()
        {
            switch (NavigationSystem)
            {
                case NavigationSystem.GLONASS:
                    return Prn > 0 && Prn <= 27;
                case NavigationSystem.GPS:
                    return Prn > 0 && Prn <= 32;
                default:
                    return false;

            }
        }
    }
}
