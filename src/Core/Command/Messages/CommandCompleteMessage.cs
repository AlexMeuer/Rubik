using Core.TinyMessenger;

namespace Core.Command.Messages
{
    public class CommandCompleteMessage : TinyMessageBase
    {
        public ICommand Command { get; }
        public ICommandHandler Handler { get; }
        
        public CommandCompleteMessage(object sender, ICommand command, ICommandHandler handler) : base(sender)
        {
            Command = command;
            Handler = handler;
        }
    }
}