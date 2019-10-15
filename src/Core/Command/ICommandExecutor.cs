using System.Collections;

namespace Core.Command
{
    public interface ICommandExecutor
    {
        void HandleExecution(IEnumerator execution);
    }
}