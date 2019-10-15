using System;
using Core.TinyMessenger;

namespace Core.Command.Messages
{
    public class EnqueueCommandMessage : TinyMessageBase
    {
        public ICommand Command => ExecutableCommand;
        
        internal IExecutableCommand ExecutableCommand { get; }
        
        /// <summary>
        /// If true, this command should not be added to the history.
        /// </summary>
        internal bool Transient { get; }
        
        public EnqueueCommandMessage(object sender, IExecutableCommand command, bool transient = false) : base(sender)
        {
#if DEBUG
            // ReSharper disable once JoinNullCheckWithUsage
            if (command == null) throw new ArgumentNullException(nameof(command));
#endif
            ExecutableCommand = command;
            Transient = transient;
        }
    }
}