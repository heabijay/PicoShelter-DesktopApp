using PicoShelter_DesktopApp.Models;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;

namespace PicoShelter_DesktopApp.Services
{
    public class NamedPipeManager
    {
        public string NamedPipeName { get; set; }
        public event Action<PipeCommand> ReceivedCommand;

        private bool _isRunning = false;
        private Thread _thread;

        public NamedPipeManager(string name)
        {
            NamedPipeName = name;
        }

        /// <summary>
        /// Starts a new Pipe server on a new thread
        /// </summary>
        public void StartServer()
        {
            _thread = new Thread((pipeName) =>
            {
                _isRunning = true;

                while (true)
                {
                    using (var server = new NamedPipeServerStream(pipeName as string, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
                    {
                        server.BeginWaitForConnection(new AsyncCallback(NamedPipeServerAsyncCallback), server);
                    }

                    if (_isRunning == false)
                        break;
                }
            });
            _thread.Start(NamedPipeName);
        }

        private void NamedPipeServerAsyncCallback(IAsyncResult ar)
        {
            PipeCommand command = null;
            using (StreamReader reader = new StreamReader(ar.AsyncState as NamedPipeServerStream))
            {
                string text = reader.ReadToEnd();

                try
                {
                    command = JsonSerializer.Deserialize<PipeCommand>(text);
                }
                catch (Exception) { }
            }

            if (command != null)
            {
                if (command.Type == PipeCommand.CommandType.Exit)
                    StopServer();

                OnReceiveCommand(command);
            }
        }

        /// <summary>
        /// Called when data is received.
        /// </summary>
        /// <param name="text"></param>
        protected virtual void OnReceiveCommand(PipeCommand command) => ReceivedCommand?.Invoke(command);


        /// <summary>
        /// Shuts down the pipe server
        /// </summary>
        public void StopServer()
        {
            _isRunning = false;
            Write(JsonSerializer.Serialize(new PipeCommand() { Type = PipeCommand.CommandType.Exit }));
        }

        /// <summary>
        /// Write a client message to the pipe
        /// </summary>
        /// <param name="text"></param>
        /// <param name="connectTimeout"></param>
        public bool Write(string text, int connectTimeout = 3000)
        {
            using (var client = new NamedPipeClientStream(NamedPipeName))
            {
                try
                {
                    client.Connect(connectTimeout);
                }
                catch
                {
                    return false;
                }

                if (!client.IsConnected)
                    return false;

                using (StreamWriter writer = new StreamWriter(client))
                {
                    writer.Write(text);
                    writer.Flush();
                }
            }
            return true;
        }
    }
}
