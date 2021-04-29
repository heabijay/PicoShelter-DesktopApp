using PicoShelter_DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PicoShelter_DesktopApp.Services
{
    public class NamedPipeService : IDisposable
    {
        public readonly string PipeName;
        public NamedPipeService(string pipeName)
        {
            PipeName = pipeName;
        }

        public event Action<PipeCommand> CommandReceived;
        private CancellationTokenSource CTS { get; set; }
        private NamedPipeServerStream serverStream { get; set; }
        public async void StartServer()
        {
            CTS = new CancellationTokenSource();
            CTS.Token.Register(() => serverStream.Disconnect());


            try
            {
                while (!CTS.IsCancellationRequested)
                {
                    try
                    {
                        serverStream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                        await serverStream.WaitForConnectionAsync(CTS.Token);

                        using (StreamReader sr = new StreamReader(serverStream))
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
                CTS.Cancel();
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
            CTS?.Cancel();
            serverStream.Dispose();
        }

        public void Dispose()
        {
            CTS?.Cancel();
        }
    }
}
