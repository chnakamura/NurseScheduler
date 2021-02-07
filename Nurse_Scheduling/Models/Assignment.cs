using System;
using System.Collections.Generic;
using System.Linq;

namespace Nurse_Scheduling.Models
{
    public class Assignment
    {
        public Assignment(List<Nurse> nurses, List<Shift> shifts)
        {
            this.Nurses = nurses;
            this.Shifts = shifts;
        }

        public Assignment(Assignment assignment)
        {
            this.Nurses = assignment.Nurses.Select(nurse => Nurse.NurseCopier(nurse)).ToList();
        }

        public IReadOnlyList<Nurse> Nurses;
        public IReadOnlyList<Shift> Shifts;

        /// <summary>
        /// TODO - Switch to score?
        /// TODO - Add the option to do average, minmax, or other?
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            var nurseScore = this.Nurses.Sum(nurse => nurse.Weight());
            var shiftScore = this.Shifts.Sum(shift => shift.Weight());
            return nurseScore + shiftScore;
        }

        public override string ToString()
        {
            return $@"ASSIGNMENT ({this.Weight()}):" + "\n" +
                string.Join('\n', this.Shifts.OrderBy(shift => shift.Id).ToList());
        }
    }
}
