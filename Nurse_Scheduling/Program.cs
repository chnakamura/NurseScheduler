using System;

namespace Nurse_Scheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = InputGenerator.GererateInputs(2);
            Console.WriteLine("Input:");
            Console.WriteLine(population.BestAssignment);
            Console.WriteLine("------");
            Algorithm.Run(population);
            Console.WriteLine("Output:");
            Console.WriteLine(population.BestAssignment);
        }
    }
}
