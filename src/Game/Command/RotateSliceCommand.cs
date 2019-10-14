using System.Collections;
using Core.Command;
using Game.Cube;

namespace Game.Command
{
    public class RotateSliceCommand : ICommand
    {
        private readonly Slice slice;
        private readonly bool reverse;
        public RotateSliceCommand(Slice slice, bool reverse)
        {
            this.slice = slice;
            this.reverse = reverse;
        }
        
        public IEnumerator Execute()
        {
            yield return slice.Rotate90Degrees(reverse);
        }

        public IEnumerator Undo()
        {
            yield return slice.Rotate90Degrees(!reverse);
        }
    }
}