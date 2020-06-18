namespace NovAtelLogReader.LogData
{
    public class LogDataBase
    {
        public NavigationSystem NavigationSystem { get; set; }
        public SignalType SignalType { get; set; }
        public uint Prn { get; set; }
        public int GloFreq { get; set; }
    }
}
