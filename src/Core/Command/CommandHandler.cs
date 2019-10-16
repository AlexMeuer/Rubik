using System;
using System.Collections;
using System.Collections.Generic;
using Core.Command.Messages;
using Core.Logging;
using Core.TinyMessenger;

namespace Core.Command
{
    internal class CommandHandler : ICommandHandler, IDisposable
    {
        private readonly ITinyMessengerHub messengerHub;
        private readonly TinyMessageSubscriptionToken enqueueSubscriptionToken;
        private readonly TinyMessageSubscriptionToken undoSubscriptionToken;
        private readonly TinyMessageSubscriptionToken redoSubscriptionToken;
        private readonly ILogger logger;
        private readonly ICommandExecutor executor;
        private readonly Stack<IUndoableCommand> commandHistory;
        private readonly Stack<IUndoableCommand> undoHistory;

        public CommandHandler(ITinyMessengerHub messengerHub, ILogger logger, ICommandExecutor executor)
        {
            this.logger = PrefixedLogger.ForType<CommandHandler>(logger);
            this.messengerHub = messengerHub;
            this.executor = executor;
            
            commandHistory = new Stack<IUndoableCommand>();
            undoHistory = new Stack<IUndoableCommand>();
            
            enqueueSubscriptionToken = messengerHub.Subscribe<EnqueueCommandMessage>(HandleEnqueueMessage);
            undoSubscriptionToken = messengerHub.Subscribe<UndoCommandMessage>(m => UndoLast());
            redoSubscriptionToken = messengerHub.Subscribe<RedoCommandMessage>(m => RedoLast());
        }

        public bool CanUndo => commandHistory.Count > 0;
        public bool CanRedo => undoHistory.Count > 0;
        
        public void UndoLast()
        {
            if (!CanUndo)
            {
                logger.Warn("No command to undo.");
                return;
            }

            var cmd = commandHistory.Pop();
            
            undoHistory.Push(cmd);
            
            logger.Info("Undoing command: {0}", cmd.GetType().Name);
            
            executor.HandleExecution(PublishMessageWhenFinished(cmd.Undo(), cmd));
        }

        public void RedoLast()
        {
            if (!CanRedo)
            {
                logger.Warn("No command to redo.");
                return;
            }

            var cmd = undoHistory.Pop();
            
            commandHistory.Push(cmd);
            
            logger.Info("Redoing command: {0}", cmd.GetType().Name);
            
            executor.HandleExecution(PublishMessageWhenFinished(cmd.Execute(), cmd));
        }

        public void Dispose()
        {
            enqueueSubscriptionToken.Dispose();
            undoSubscriptionToken.Dispose();
            redoSubscriptionToken.Dispose();
        }

        private void HandleEnqueueMessage(EnqueueCommandMessage message)
        {
            var cmd = message.ExecutableCommand;

            if (!message.Transient && message.Command is IUndoableCommand undoableCommand)
            {
                logger.Info("Executing undoable command: {0}", cmd.GetType().Name);

                commandHistory.Push(undoableCommand);
            }
            else if (message.Transient)
            {
                logger.Info("Executing command: {0}", cmd.GetType().Name);
            }

            executor.HandleExecution(PublishMessageWhenFinished(cmd.Execute(), cmd));
        }

        private IEnumerator PublishMessageWhenFinished(IEnumerator execution, ICommand command)
        {
            yield return execution;
            
            logger.Info("Completed command: {0}", command.GetType().Name);
            
            messengerHub.Publish(new CommandCompleteMessage(this, command, this));
        }
    }
}