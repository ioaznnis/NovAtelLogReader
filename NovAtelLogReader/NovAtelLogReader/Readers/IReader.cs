using NovAtelLogReader.LogRecordFormats;
using System;
using System.IO;

namespace NovAtelLogReader.Readers
{
    interface IReader
    {
        void Open(ILogRecordFormat recordFormat);
        void Close();
        event EventHandler<ReceiveEventArgs> DataReceived;
        event EventHandler<ErrorEventArgs> ReadError;
        int MessageCounter { get; }
    }
}
