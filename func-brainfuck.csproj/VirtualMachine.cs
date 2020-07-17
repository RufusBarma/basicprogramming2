using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
        private byte[] memory;

        public byte[] Memory { get { return memory; } }

        public int MemorySize { get { return memory.Length; } }

        public int MemoryPointer { get; set; }

        private Dictionary<char, Action<IVirtualMachine>> commands;

        private List<char> instructions;

        public string Instructions { get { return new string(instructions.ToArray()); } }

        public int InstructionPointer { get; set; }

        public VirtualMachine(string program, int memorySize)
		{
            memory = new byte [memorySize];
            instructions = new List<char>(program.ToCharArray());
            commands = new Dictionary<char, Action<IVirtualMachine>>();
            MemoryPointer = 0;
            InstructionPointer = 0;
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
            commands.Add(symbol, execute);
		}

		public void Run()
		{
			for(; InstructionPointer < Instructions.Length; InstructionPointer++)
                if (commands.ContainsKey(Instructions[InstructionPointer]))
                    commands[Instructions[InstructionPointer]](this);            
		}
	}
}