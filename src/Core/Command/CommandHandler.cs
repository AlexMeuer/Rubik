using System;
using System.Collections.Generic;
using Core.Logging;
using Core.TinyMessenger;

namespace Core.Command
{
    internal class CommandHandler : ICommandHandler, IDisposable
    {
        private readonly TinyMessageSubscriptionToken subscriptionToken;
        private readonly ILogger logger;
        private readonly ICommandExecutor executor;
        private readonly Stack<ICommand> executedCommands;
        private readonly Stack<ICommand> undoneCommands;

        public CommandHandler(ITinyMessengerHub messengerHub, ILogger logger, ICommandExecutor executor)
        {
            subscriptionToken = messengerHub.Subscribe<EnqueueCommand>(HandleEnqueueMessage);
            this.logger = PrefixedLogger.ForType<CommandHandler>(logger);
            this.executor = executor;
            executedCommands = new Stack<ICommand>();
            undoneCommands = new Stack<ICommand>();
        }

        public bool CanUndo => executedCommands.Count > 0;
        public bool CanRedo => undoneCommands.Count > 0;
        
        public void UndoLast()
        {
            if (!CanUndo)
            {
                logger.Warn("No command to undo.");
                return;
            }

            var cmd = executedCommands.Pop();
            
            undoneCommands.Push(cmd);
            
            logger.Info("Undoing command: {0}", cmd.GetType().Name);
            
            executor.HandleExecution(cmd.Undo);
        }

        public void RedoLast()
        {
            if (!CanRedo)
            {
                logger.Warn("No command to redo.");
                return;
            }

            var cmd = undoneCommands.Pop();
            
            executedCommands.Push(cmd);
            
            logger.Info("Redoing command: {0}", cmd.GetType().Name);
            
            executor.HandleExecution(cmd.Execute);
        }

        private void HandleEnqueueMessage(EnqueueCommand message)
        {
            logger.Info("Executing fresh command: {0}", message.Command.GetType().Name);
            
            executedCommands.Push(message.Command);
            
            executor.HandleExecution(message.Command.Execute);
        }

        public void Dispose()
        {
            subscriptionToken.Dispose();
        }
    }
}