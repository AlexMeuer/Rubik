using Core.TinyMessenger;

namespace Core.Command.Messages
{
    public class CommandCompleteMessage : TinyMessageBase
    {
        public ICommand Command { get; }
        public CommandCompleteMessage(object sender, ICommand command) : base(sender)
        {
            Command = command;
        }
    }
}