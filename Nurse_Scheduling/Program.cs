using System;

namespace Nurse_Scheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            var assignment = InputGenerator.GererateInputs();
            Console.Write("Input:\n");
            Console.Write(assignment);
            Console.Write("------");
            PhaseOne.Run(ref assignment, 20);
            PhaseTwo.Run(ref assignment);
            Console.Write("Output:\n");
            Console.Write(assignment);
        }
    }
}
