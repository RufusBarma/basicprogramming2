using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
			vm.RegisterCommand('+', b => {
                if (b.Memory[b.MemoryPointer] < 255)
                    b.Memory[b.MemoryPointer]++;
                else
                    b.Memory[b.MemoryPointer] = 0;
            });
			vm.RegisterCommand('-', b => {
                if (b.Memory[b.MemoryPointer] >0)
                    b.Memory[b.MemoryPointer]--;
                else
                    b.Memory[b.MemoryPointer] = 255;
            });
			vm.RegisterCommand(',', b => { b.Memory[b.MemoryPointer] = (byte)read(); });
            vm.RegisterCommand('>', b => {
                if (b.MemoryPointer < b.Memory.Length - 1)
                    b.MemoryPointer++;
                else
                    b.MemoryPointer = 0;
            });
            vm.RegisterCommand('<', b => {
                if (b.MemoryPointer > 0)
                    b.MemoryPointer--;
                else
                    b.MemoryPointer = b.Memory.Length - 1;
            });
            foreach (var c in "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890".ToCharArray())
            {
                var symbol = c;
                vm.RegisterCommand(symbol, b => { b.Memory[b.MemoryPointer] = (byte)symbol; });
            }
        }
	}
}