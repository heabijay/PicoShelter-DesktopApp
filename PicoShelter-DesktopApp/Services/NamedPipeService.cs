using PicoShelter_DesktopApp.Models;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;

namespace PicoShelter_DesktopApp.Services
{
    public class NamedPipeService : IDisposable
    {
        public readonly string PipeName;
        public event Action<PipeCommand> CommandReceived;

        private CancellationTokenSource _CTS { get; set; }
        private NamedPipeServerStream _serverStream { get; set; }

        public NamedPipeService(string pipeName)
        {
            PipeName = pipeName;
        }
        public async void StartServer()
        {
            _CTS = new CancellationTokenSource();
            _CTS.Token.Register(() => _serverStream.Disconnect());


            try
            {
                while (!_CTS.IsCancellationRequested)
                {
                    try
                    {
                        _serverStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        await _serverStream.WaitForConnectionAsync(_CTS.Token);

                        using (StreamReader sr = new StreamReader(_serverStream))
                        {
                            string msg = await sr.ReadToEndAsync();

                            if (msg != null)
                            {
                                PipeCommand command = JsonSerializer.Deserialize<PipeCommand>(msg);

                                if (command.Type == PipeCommand.CommandType.Exit)
                                {
                                    break;
                                }
                                OnCommandReceived(command);
                            }
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                _CTS.Cancel();
            }
        }

        public void ClientWrite(string message)
        {
            try
            {
                using (NamedPipeClientStream clientStream = new NamedPipeClientStream(PipeName))
                {
                    clientStream.Connect(3000);

                    using (StreamWriter sw = new StreamWriter(clientStream) { AutoFlush = true })
                    {
                        sw.Write(message);
                    }

                    clientStream.Close();
                }
            }
            catch { }
        }

        protected virtual void OnCommandReceived(PipeCommand command) => CommandReceived?.Invoke(command);

        public void StopServer()
        {
            _CTS?.Cancel();
            _serverStream.Dispose();
        }

        public void Dispose()
        {
            _CTS?.Cancel();
        }
    }
}
