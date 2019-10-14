using System;
using Core.TinyMessenger;

namespace Core.Command
{
    public class EnqueueCommand : TinyMessageBase
    {
        public ICommand Command { get; }
        
        public EnqueueCommand(object sender, ICommand command) : base(sender)
        {
#if DEBUG
            // ReSharper disable once JoinNullCheckWithUsage
            if (command == null) throw new ArgumentNullException(nameof(command));
#endif
            Command = command;
        }
    }
}