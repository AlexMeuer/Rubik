using System;
using System.Collections;

namespace Core.Command
{
    public interface ICommandExecutor
    {
        void HandleExecution(Func<IEnumerator> execution);
    }
}