using System;
using System.Collections.Generic;
using System.Linq;
using Nurse_Scheduling.Models;

namespace Nurse_Scheduling
{
    public class PhaseOne
    {
        private static Random random = new Random(0);

        public static void Run(ref Assignment assignment, int numCycles)
        {
            for (var i = 0; i < numCycles; i++)
            {
                SuccessiveSegmentSwapMutation(ref assignment);
            }
        }

        public static void SuccessiveSegmentSwapMutation(ref Assignment assignment)
        {
            for (var n1 = 0; n1 < assignment.Nurses.Count; n1++)
            {
               foreach (var n2 in GetAdjacentNurses(assignment.Nurses, n1)) {
                    SelectivePartialSwap(ref assignment, assignment.Nurses[n1], assignment.Nurses[n2]);
               }
            }
        }

        private static List<int> GetAdjacentNurses(IReadOnlyList<Nurse> nurses, int i)
        {
            var adjacentNurses = new List<int>();
            if (i > 0)
            {
                adjacentNurses.Add(i); 
            }
            if (i < nurses.Count - 1)
            {
                adjacentNurses.Add(i + 1);
            }
            return adjacentNurses;
        }

        public static void SelectivePartialSwap(ref Assignment assignment, Nurse n1, Nurse n2)
        {

            for (var d1 = 0; d1 < 14; d1 += 1)
            {
                for (var d2 = d1; d2 < 14; d2 += 1)
                {
                    if (random.NextDouble() < assignment.SwapProbability)
                    {
                        var initScore = assignment.Score();
                        PerformSwap(n1, n2, d1, d2);
                        var newScore = assignment.Score();
                        if (newScore <= initScore)
                        {
                            PerformSwap(n1, n2, d1, d2); 
                        }
                    }
                }
            }
        }

        public static void PerformSwap(Nurse n1, Nurse n2, int d1, int d2)
        {
            var n1UnAssinged = n1.UnAssign(d1, d2);
            var n2UnAssigned = n2.UnAssign(d1, d2);

            n1.Assign(n2UnAssigned);
            n2.Assign(n1UnAssinged);
        }
    }
}
