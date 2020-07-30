using NLog;
using NovAtelLogReader.LogRecordFormats;
using NovAtelLogReader.PInvoke;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovAtelLogReader.Readers
{
    class ComPortReader : IReader
    {
        public event EventHandler<ReceiveEventArgs> DataReceived;
        public event EventHandler<ErrorEventArgs> ReadError;

        private Logger _logger = LogManager.GetCurrentClassLogger();

#if USE_PI_SERIAL_PORT
        private PISerialPort _serialPort;
#else
        private SerialPort _serialPort;
#endif

        private CancellationTokenSource _cts;
        ILogRecordFormat _recordFormat;

        private volatile int _messageCounter = 0;

        private readonly int HeaderLength = 28;
        private readonly byte[] HeaderStart = new byte[] { 0xaa, 0x44, 0x12 };

        public int MessageCounter
        {
            get { return _messageCounter;  }
        }

        public void Close()
        {
            _logger.Info("Закрытие COM-порта");

            DataReceived = null;
            ReadError = null;

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;
            }            
        }

        public void Open(ILogRecordFormat recordFormat)
        {
            _recordFormat = recordFormat;
            _messageCounter = 0;

            InitReceiver();

            Task.Run(() => Read(), _cts.Token).ContinueWith(_ => ReadError?.Invoke(this, new ErrorEventArgs(_.Exception)), TaskContinuationOptions.OnlyOnFaulted);
        }

        private void InitReceiver()
        {
            var portName = Properties.Settings.Default.ComPortName;
            _logger.Info("Открытие COM-порта {0}", portName);
            _cts = new CancellationTokenSource();

#if USE_PI_SERIAL_PORT
            _serialPort = new PISerialPort();
            _serialPort.Open(portName, Properties.Settings.Default.ComPortSpeed);
#else
            _serialPort = new SerialPort(portName)
            {
                ReadTimeout = 1500,
                WriteTimeout = 1500,
                DtrEnable = true,
                RtsEnable = true,
                BaudRate = Properties.Settings.Default.ComPortSpeed,
            };
            _serialPort.Open();
#endif

            _logger.Info("Инициализация приемника");
            foreach (var name in Properties.Settings.Default.InitCommands.Cast<string>())
            {
                Console.WriteLine(name);
                _serialPort.WriteLine(name);
            }

            Thread.Sleep(1500);

            _serialPort.DiscardInBuffer();
            _serialPort.DiscardOutBuffer();
        }

        public void Read()
        {
            byte[] buffer = new byte[HeaderLength];
            byte[] crc = new byte[4];
            int dataLength = 0;

            while (!_cts.IsCancellationRequested)
            {
                //_logger.Debug("Start read cycle");
                
                Array.Resize(ref buffer, HeaderLength);
                Array.Clear(buffer, 0, HeaderLength);

                if (Check(buffer, 0, 0xaa)) continue;
                if (Check(buffer, 1, 0x44)) continue;
                if (Check(buffer, 2, 0x12)) continue;
                if (Check(buffer, 3, HeaderLength)) continue;

                _messageCounter++;

                _serialPort.BlockingRead(buffer, 4, HeaderLength - 4);
                dataLength = BitConverter.ToUInt16(buffer, 8);

                Array.Resize(ref buffer, HeaderLength + dataLength);

                _serialPort.BlockingRead(buffer, HeaderLength, dataLength);
                _serialPort.BlockingRead(crc, 0, 4);

                if (Util.CalculateBlockCRC32(buffer) == BitConverter.ToUInt32(crc, 0))
                {
                    DataReceived?.Invoke(this, new ReceiveEventArgs() { Data = buffer });
                }
                else
                {
                    _logger.Warn("Невверная контрольная сумма пакета данных");
                }
            }
        }


        private bool Check(byte[] buffer, int offset, int symbol)
        {
            _serialPort.BlockingRead(buffer, offset, 1);
            
            if (buffer[offset] == symbol) return false;
            //_logger.Debug($"Wait {symbol} but received {buffer[offset]}");
            return true;
        }

        public static void GetAvailablePortNames()
        {
            Console.WriteLine("The following serial ports were found:");
            foreach (var port in SerialPort.GetPortNames())
            {
                Console.WriteLine(port);
            }
        }
    }

    internal static class SerialPortReader
    {
        public static void BlockingRead(this SerialPort port, byte[] buffer, int offset, int count)
        {
            while (count > 0)
            {
                // SerialPort.Read() blocks until at least one byte has been read, or SerialPort.ReadTimeout milliseconds
                // have elapsed. If a timeout occurs a TimeoutException will be thrown.
                // Because SerialPort.Read() blocks until some data is available this is not a busy loop,
                // and we do NOT need to issue any calls to Thread.Sleep().

                int bytesRead = port.Read(buffer, offset, count);
                offset += bytesRead;
                count -= bytesRead;
            }
        }
    }
}