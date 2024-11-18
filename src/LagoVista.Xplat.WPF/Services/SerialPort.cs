using LagoVista.Core.PlatformSupport;
using System;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using System.IO;
using System.Threading;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class SerialPort : ISerialPort
    {
        System.IO.Ports.SerialPort _serialPort;
        SerialPortInfo _portInfo;

        public SerialPort(SerialPortInfo portInfo)
        {
            _portInfo = portInfo;
        }

        public bool IsConnected
        {
            get
            {
                lock (this)
                {
                    if (_serialPort == null)
                    {
                        return false;
                    }

                    return _serialPort.IsOpen;
                }
            }
        }

        public Stream InputStream
        {
            get
            {
                if (_serialPort == null)
                    throw new InvalidOperationException("Serial Port Not Open");

                return _serialPort.BaseStream;
            }
        }

        public Stream OutputStream
        {
            get
            {
                if (_serialPort == null)
                    throw new InvalidOperationException("Serial Port Not Open");

                return _serialPort.BaseStream;
            }
        }

        public Task CloseAsync()
        {
            Dispose();
            return Task.FromResult(default(object));
        }

        public void Dispose()
        {
            lock (this)
            {
                try
                {
                    if (_serialPort != null)
                    {
                        if (_serialPort.IsOpen)
                            _serialPort.Close();

                        _serialPort.Dispose();
                        _serialPort = null;
                    }
                }
                catch(Exception ex)
                {
                    /* NOP */
                    _serialPort = null;
                }
            }
        }

        public Task OpenAsync()
        {
            try
            {
                lock (this)
                {
                    _serialPort = new System.IO.Ports.SerialPort(_portInfo.Id, _portInfo.BaudRate);
                    _serialPort.Open();

                    return Task.FromResult(default(object));
                }
            }
            catch (Exception ex)
            {
                _serialPort = null;
                throw new Exception("Could not open serial port.",ex);
            }
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken token = default)
        {
            var bytesAvailable =_serialPort.BytesToRead;
            if (bytesAvailable > 0)
            {
                _serialPort.Read(buffer, offset, bytesAvailable);
            }

            return Task.FromResult(bytesAvailable);
        }

        public Task WriteAsync(string msg)
        {
            _serialPort.Write(msg);
            return Task.CompletedTask;
        }

        public Task WriteAsync(byte[] buffer)
        {
            _serialPort.Write(buffer, 0, buffer.Length);
            return Task.CompletedTask;
        }
    }
}
