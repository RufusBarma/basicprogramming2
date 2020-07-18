using System.Collections.Generic;

namespace func.brainfuck
{
    public class Loop
    {
        public int Start { get; set; }
        public int End { get; set; }
        public Loop(int start)
        {
            this.Start = start;
        }
    }

	public class BrainfuckLoopCommands
	{
        private static Dictionary<int, Loop> startLoops;
        private static Dictionary<int, Loop> endLoops;
        public static void RegisterTo(IVirtualMachine vm)
		{
            var halfLoops = new Stack<Loop>();
            startLoops = new Dictionary<int, Loop>();
            endLoops = new Dictionary<int, Loop>();
            for (int position = 0; position < vm.Instructions.Length; position++)
                if (vm.Instructions[position] == '[')
                {
                    halfLoops.Push(new Loop(position));
                }else if (vm.Instructions[position] == ']')
                {
                    var loop = halfLoops.Pop();
                    loop.End = position;
                    startLoops.Add(loop.Start, loop);
                    endLoops.Add(loop.End, loop);
                }
			vm.RegisterCommand('[', b => {
                if (vm.Memory[vm.MemoryPointer] == 0)
                    vm.InstructionPointer = startLoops[vm.InstructionPointer].End;
            });
			vm.RegisterCommand(']', b => {
                if (vm.Memory[vm.MemoryPointer] != 0)
                    vm.InstructionPointer = endLoops[vm.InstructionPointer].Start;
            });
		}
	}
}