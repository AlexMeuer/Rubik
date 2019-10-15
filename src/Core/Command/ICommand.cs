using System;
using System.Collections;

namespace Core.Command
{
    public interface ICommand
    {
        // This empty interface is helpful for clients that want to know the implementation
        // type of the command without allowing them to execute or undo the command.
    }
    
    public interface IInvokableCommand : ICommand
    {
        IEnumerator Execute();
        IEnumerator Undo();
    }
}