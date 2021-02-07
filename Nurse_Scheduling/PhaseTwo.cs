using System;
using System.Linq;
using Nurse_Scheduling.Models;

namespace Nurse_Scheduling
{
    public class PhaseTwo
    {
        private static Random random = new Random(0);

        public static void Run(ref Assignment assignment)
        {
            var numCycles = 0;

            while (IsTerminationCriteriaMet(numCycles))
            {
                foreach (var nurse in assignment.Nurses)
                {
                    SelectiveDaySwapMutation(ref assignment);
                    PhaseOne.SuccessiveSegmentSwapMutation(ref assignment);
                    RandomSegmentSwapMutation(ref assignment);
                }
                numCycles++;
            }
        }

        private static bool IsTerminationCriteriaMet(int numCycles)
        {
            /*If he/she chooses ―the total number of generations‖, next, he/she has to insert this number.
            2. If he/she chooses ―the total number of generations for which the fitness remains the same‖, next,
            he/she has to insert this number*/
            return numCycles < 10;
        }

        private static void SelectiveDaySwapMutation(ref Assignment assignment)
        {
            foreach(var n1 in assignment.Nurses.OrderBy(n => random.Next()))
            {
                foreach(var n2 in assignment.Nurses.Where(n => n != n1).OrderBy(n => random.Next()))
                {
                    for (var d = 0; d < 14; d++)
                    {
                        var initScore = assignment.Score();
                        PhaseOne.PerformSwap(n1, n2, d, d);
                        var newScore = assignment.Score();
                        if (newScore <= initScore)
                        {
                            PhaseOne.PerformSwap(n1, n2, d, d);
                        }
                    }
                }
            }
        }

        private static void RandomSegmentSwapMutation(ref Assignment assignment)
        {
            var parings = assignment.Nurses.OrderBy(n => random.Next())
                .Join(assignment.Nurses.OrderBy(n => random.Next()), n => true,
                n => true,(n1, n2) => new {
                    n1,
                    n2
                }
            );

            foreach (var paring in parings)
            {
                PhaseOne.SelectivePartialSwap(ref assignment, paring.n1, paring.n2);
            }
        }
    }
}
