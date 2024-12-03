using System.Collections.Generic;

namespace Core.Network
{
    public class ServerRequestInvoker
    {
        private readonly Queue<IServerCommand> _commandQueue = new();
        private bool _isProcessing;

        public void EnqueueCommand(IServerCommand serverCommand)
        {
            _commandQueue.Enqueue(serverCommand);

            if (!_isProcessing)
            {
                _isProcessing = true;
                ProcessQueue();
            }
        }

        private async void ProcessQueue()
        {
            while (_commandQueue.Count > 0)
            {
                var command = _commandQueue.Dequeue();
                await command.Execute();
            }
            _isProcessing = false;
        }

        public void CancelAllCommands()
        {
            foreach (var command in _commandQueue)
            {
                command.Cancel();
            }
            
            _commandQueue.Clear();
        }
        
        public void CancelCommand(IServerCommand serverCommand)
        {
            
            serverCommand.Cancel();
        }
    }
}