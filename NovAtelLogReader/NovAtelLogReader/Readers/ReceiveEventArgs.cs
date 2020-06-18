using System;

namespace NovAtelLogReader.Readers
{
    class ReceiveEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
