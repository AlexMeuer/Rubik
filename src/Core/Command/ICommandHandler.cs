using System;
using System.Collections;

namespace Core.Command
{   
    public interface ICommandHandler
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void UndoLast();
        void RedoLast();
    }
}