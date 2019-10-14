using System.Collections;

namespace Core.Command
{
    public interface ICommand
    {
        IEnumerator Execute();
        IEnumerator Undo();
    }
}