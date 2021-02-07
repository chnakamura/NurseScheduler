using System;
using System.Collections.Generic;
using System.Linq;
using Nurse_Scheduling.Models;

namespace Nurse_Scheduling
{
    public class Algorithm
    {
        public static int HARD_CONSTRAINT_MULTIPLIER = 100;
        public static int SOFT_CONSTRAINT_MULTIPLIER = 1;

        private static Random random = new Random(0);
        private static double SWAP_PROBABILITY = .5;

        public static void Run(Population population)
        {
            var start = new
            {
                weight = population.BestAssignment.Weight(),
                time = DateTime.Now
            };

            RunPhaseOne(population);

            var sortedAssignments = population.Assignments.OrderBy(assignment => assignment.Weight());

            var bestAssignment = sortedAssignments.First();
            var otherAssignments = sortedAssignments.Skip(1);

            var bestAssignmentNurseMemo = bestAssignment.Nurses.ToDictionary(nurse => nurse.Id, 
                nurse => nurse.Shifts.Values.Select(s => s.Id).ToList());

            foreach (var assignment in otherAssignments)
            {
                var shiftMemo = assignment.Shifts.ToDictionary(shift => shift.Id, shift => shift);

                foreach (var nurse in assignment.Nurses)
                {
                    nurse.UnassignAll();

                    var shiftsIdsToAssign = bestAssignmentNurseMemo[nurse.Id];
                    var shiftsToAssign = shiftsIdsToAssign.Select(id => shiftMemo[id]).ToList();
                    
                    foreach (var shift in shiftsToAssign)
                    {
                        nurse.Assign(shift);
                    }
                }
            }

            RunPhaseTwo(population);

            bestAssignment = population.BestAssignment;
            var numShiftSlots = bestAssignment.Nurses.Select(n => n.Shifts.Count).Sum();

            var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
            Console.WriteLine($@"RunCycle took {elapsed}ms to assign {bestAssignment.Nurses.Count} nurses to {numShiftSlots} shifts!");
        }

        public static void RunPhaseOne(Population population)
        {
            for (var i = 0; i < population.NumCycles; i++)
            {
                var start = new
                {
                    weight = population.BestAssignment.Weight(),
                    time = DateTime.Now
                };

                foreach (var assignment in population.Assignments)
                {
                    SuccessiveSegmentSwapMutation(assignment);
                }

                var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
                Console.WriteLine($@"RunPhaseOne Cycle took {elapsed}ms, and improved weight from {start.weight} to {population.BestAssignment.Weight()}");
            }
        }

        public static void RunPhaseTwo(Population population)
        {
            var numCycles = 0;

            var bestFitness = population.BestAssignment.Weight();
            var numCyclesWithoutImprovment = 0;

            while (IsTerminationCriteriaMet(numCycles, numCyclesWithoutImprovment))
            {
                var start = new
                {
                    weight = population.BestAssignment.Weight(),
                    time = DateTime.Now
                };

                foreach (var assignment in population.Assignments)
                {
                    SelectiveDaySwapMutation(assignment);
                    SuccessiveSegmentSwapMutation(assignment);
                    RandomSegmentSwapMutation(assignment);
                }
                numCycles++;

                var newBestFitness = population.BestAssignment.Weight();
                if (newBestFitness < bestFitness)
                {
                    numCyclesWithoutImprovment = 0;
                    bestFitness = newBestFitness;
                } else
                {
                    numCyclesWithoutImprovment++;
                }

                var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
                Console.WriteLine($@"RunPhaseTwo Cycle took {elapsed}ms, and improved weight from {start.weight} to {population.BestAssignment.Weight()}");
            }
        }

        private static bool IsTerminationCriteriaMet(int numCycles, int numCyclesWithoutImprovment)
        {
            /*If he/she chooses ―the total number of generations‖, next, he/she has to insert this number.
            2. If he/she chooses ―the total number of generations for which the fitness remains the same‖, next,
            he/she has to insert this number*/
            return numCyclesWithoutImprovment < 1 && numCycles < 10;
        }

        private static void SelectiveDaySwapMutation(Assignment assignment)
        {
            var start = new
            {
                weight = assignment.Weight(),
                time = DateTime.Now
            };

            foreach (var n1 in assignment.Nurses.OrderBy(n => random.Next()))
            {
                foreach (var n2 in assignment.Nurses.Where(n => n != n1).OrderBy(n => random.Next()))
                {
                    for (var d = 0; d < 14; d++)
                    {
                        var initScore = assignment.Weight();
                        PerformSwap(n1, n2, d, d);
                        var newScore = assignment.Weight();
                        if (newScore > initScore)
                        {
                            PerformSwap(n1, n2, d, d);
                        }
                    }
                }
            }

            var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
            Console.WriteLine($@"SelectiveDaySwapMutation took {elapsed}ms, and improved weight from {start.weight} to {assignment.Weight()}");
        }

        private static void RandomSegmentSwapMutation(Assignment assignment)
        {
            var start = new
            {
                weight = assignment.Weight(),
                time = DateTime.Now
            };

            var parings = assignment.Nurses.OrderBy(n => random.Next())
                .Join(assignment.Nurses.OrderBy(n => random.Next()), n => true,
                n => true, (n1, n2) => new {
                    n1,
                    n2
                }
            );

            foreach (var paring in parings)
            {
                SelectivePartialSwap(assignment, paring.n1, paring.n2);
            }

            var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
            Console.WriteLine($@"RandomSegmentSwapMutation took {elapsed}ms, and improved weight from {start.weight} to {assignment.Weight()}");
        }

        public static void SuccessiveSegmentSwapMutation(Assignment assignment)
        {
            var start = new
            {
                weight = assignment.Weight(),
                time = DateTime.Now
            };

            for (var n1 = 0; n1 < assignment.Nurses.Count; n1++)
            {
               foreach (var n2 in GetAdjacentNurses(assignment.Nurses, n1)) {
                    SelectivePartialSwap(assignment, assignment.Nurses[n1], assignment.Nurses[n2]);
               }
            }

            var elapsed = (DateTime.Now - start.time).TotalMilliseconds;
            Console.WriteLine($@"SuccessiveSegmentSwapMutation took {elapsed}ms, and improved weight from {start.weight} to {assignment.Weight()}");
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

        public static void SelectivePartialSwap(Assignment assignment, Nurse n1, Nurse n2)
        {
            var start = new
            {
                weight = assignment.Weight(),
                time = DateTime.Now
            };

            for (var d1 = 0; d1 < 14; d1 += 1)
            {
                for (var d2 = d1; d2 < 14; d2 += 1)
                {
                    if (random.NextDouble() < SWAP_PROBABILITY)
                    {
                        var initScore = assignment.Weight();
                        PerformSwap(n1, n2, d1, d2);
                        var newScore = assignment.Weight();
                        if (newScore > initScore)
                        {
                            PerformSwap(n1, n2, d1, d2); 
                        }
                    }
                }
            }
        }

        public static void PerformSwap(Nurse n1, Nurse n2, int d1, int d2)
        {
            var shiftsToAssignToNurse2 = n1.UnassignRange(d1, d2);
            var shiftsToAssignToNurse1 = n2.UnassignRange(d1, d2);

            foreach (var shiftToAssignToNurse1 in shiftsToAssignToNurse1)
            {
                n1.Assign(shiftToAssignToNurse1);
            }

            foreach (var shiftToAssignToNurse2 in shiftsToAssignToNurse2)
            {
                n2.Assign(shiftToAssignToNurse2);
            }
        }
    }
}
