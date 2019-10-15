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
        private readonly TinyMessageSubscriptionToken subscriptionToken;
        private readonly ILogger logger;
        private readonly ICommandExecutor executor;
        private readonly Stack<IInvokableCommand> executedCommands;
        private readonly Stack<IInvokableCommand> undoneCommands;

        public CommandHandler(ITinyMessengerHub messengerHub, ILogger logger, ICommandExecutor executor)
        {
            this.logger = PrefixedLogger.ForType<CommandHandler>(logger);
            this.messengerHub = messengerHub;
            this.executor = executor;
            
            executedCommands = new Stack<IInvokableCommand>();
            undoneCommands = new Stack<IInvokableCommand>();
            
            subscriptionToken = messengerHub.Subscribe<EnqueueCommandMessage>(HandleEnqueueMessage);
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
            
            executor.HandleExecution(PublishMessageWhenFinished(cmd.Undo(), cmd));
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
            
            executor.HandleExecution(PublishMessageWhenFinished(cmd.Execute(), cmd));
        }

        public void Dispose()
        {
            subscriptionToken.Dispose();
        }

        private void HandleEnqueueMessage(EnqueueCommandMessage message)
        {
            var cmd = message.InvokableCommand;

            if (message.Transient)
            {
                logger.Info("Executing transient command: {0}", cmd.GetType().Name);
            }
            else
            {
                logger.Info("Executing fresh command: {0}", cmd.GetType().Name);

                executedCommands.Push(cmd);
            }

            executor.HandleExecution(PublishMessageWhenFinished(cmd.Execute(), cmd));
        }

        private IEnumerator PublishMessageWhenFinished(IEnumerator execution, ICommand command)
        {
            yield return execution;
            
            logger.Info("Completed command: {0}", command.GetType().Name);
            
            messengerHub.Publish(new CommandCompleteMessage(this, command));
        }
    }
}