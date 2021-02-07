using System;
using System.Collections.Generic;
using System.Linq;
using Nurse_Scheduling.Models;
using static Nurse_Scheduling.Models.Shift;

namespace Nurse_Scheduling
{
    public class InputGenerator
    {
        public static Population GererateInputs(int minNursesPerShift, int populationSize = 2)
        {
            var asssignments = new List<Assignment>();

            for (var i = 0; i < populationSize; i++)
            {
                asssignments.Add(GetAssignment(minNursesPerShift));
            }

            return new Population(asssignments);
        }

        private static Assignment GetAssignment(int minNursesPerShift)
        {
            var nurses = GetNurses(7 * minNursesPerShift);

            var shifts = Enumerable.Range(0, 14).Select(id => new Shift(id, minNursesPerShift)).ToList();

            var shiftId = 0;
            
            foreach (var nurse in nurses)
            {
                nurse.Assign(shifts[shiftId++ % shifts.Count]);
                nurse.Assign(shifts[shiftId++ % shifts.Count]);
            }

            return new Assignment(nurses, shifts);
        }

        private static List<Nurse> GetNurses(int numNurses)
        {
            var nurses = new List<Nurse>();
            for (var i = 0; i < numNurses; i++)
            {
                nurses.Add(new Nurse(new List<Shift>(), i));
            }
            return nurses;
        }
    }
}
