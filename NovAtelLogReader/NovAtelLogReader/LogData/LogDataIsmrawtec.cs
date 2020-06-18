namespace NovAtelLogReader.LogData
{
    class LogDataIsmrawtec : LogDataBase
    {
        public SignalType PrimarySignal { get; set; }
        public SignalType SecondarySignal { get; set; }
        public double Tec { get; set; }
    }
}
