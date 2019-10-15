using System;
using Core.TinyMessenger;

namespace Core.Command.Messages
{
    public class EnqueueCommandMessage : TinyMessageBase
    {
        public ICommand Command => InvokableCommand;
        
        internal IInvokableCommand InvokableCommand { get; }
        
        /// <summary>
        /// If true, this command should not be added to the history.
        /// </summary>
        internal bool Transient { get; }
        
        public EnqueueCommandMessage(object sender, IInvokableCommand command, bool transient = false) : base(sender)
        {
#if DEBUG
            // ReSharper disable once JoinNullCheckWithUsage
            if (command == null) throw new ArgumentNullException(nameof(command));
#endif
            InvokableCommand = command;
        }
    }
}