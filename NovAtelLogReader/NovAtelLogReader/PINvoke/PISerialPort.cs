using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace NovAtelLogReader.PInvoke
{
    class PISerialPort : IDisposable
    {
        private IntPtr _hPort = IntPtr.Zero;

        public void Open(string name, int speed)
        {
            var portDcb = new DCB();
            var commTimeouts = new COMMTIMEOUTS();

            _hPort = Win32.CreateFile($@"\\.\{name}", Win32.GENERIC_READ | Win32.GENERIC_WRITE, 0, IntPtr.Zero, Win32.OPEN_EXISTING, 0, IntPtr.Zero);

            if (_hPort == (IntPtr) Win32.INVALID_HANDLE_VALUE)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            commTimeouts.ReadIntervalTimeout = 0;
            commTimeouts.ReadTotalTimeoutConstant = 1500;
            commTimeouts.ReadTotalTimeoutMultiplier = 15;
            commTimeouts.WriteTotalTimeoutConstant = 1500;
            commTimeouts.WriteTotalTimeoutMultiplier = 15;

            Win32.GetCommState(_hPort, ref portDcb);
            portDcb.BaudRate = speed;

            if (!Win32.SetCommState(_hPort, ref portDcb)) throw new IOException("Bad COM settings");
            if (!Win32.SetCommTimeouts(_hPort, ref commTimeouts)) throw new IOException("Bad timeout settings");
        }

        public void Close()
        {
            if (_hPort != IntPtr.Zero && _hPort != (IntPtr)Win32.INVALID_HANDLE_VALUE)
            {
                Win32.CloseHandle(_hPort);
                _hPort = IntPtr.Zero;
            }
        }

        public uint Read(byte[] buffer, int offset, int count)
        {
            byte[] readBuffer = new byte[count];

            if (!Win32.ReadFile(_hPort, readBuffer, (uint)count, out uint gotBytes, IntPtr.Zero))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            Array.Copy(readBuffer, 0, buffer, offset, gotBytes);

            return gotBytes;
        }

        public void Write(byte[] data)
        {
            if (!Win32.WriteFile(_hPort, data, (uint)data.Length, out uint sent, IntPtr.Zero))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void WriteLine(string line)
        {
            Write(Encoding.ASCII.GetBytes(line + "\r\n"));
        }

        public void Dispose()
        {
            Close();
        }

        public void DiscardInBuffer()
        {
            Win32.PurgeComm(_hPort, Win32.PURGE_RXCLEAR);
        }

        public void DiscardOutBuffer()
        {
            Win32.PurgeComm(_hPort, Win32.PURGE_TXCLEAR);
        }
    }
}
