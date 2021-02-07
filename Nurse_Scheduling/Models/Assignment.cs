using System;
using System.Collections.Generic;
using System.Linq;

namespace Nurse_Scheduling.Models
{
    public class Assignment
    {
        public Assignment(List<Nurse> nurses, List<Shift> shifts, double swapProbability)
        {
            this.Nurses = nurses;
            this.Shifts = shifts;
            this.SwapProbability = swapProbability;
        }

        public IReadOnlyList<Nurse> Nurses;
        public IReadOnlyList<Shift> Shifts;
        public double SwapProbability;

        /// <summary>
        /// TODO - Switch to score?
        /// TODO - Add the option to do average, minmax, or other?
        /// </summary>
        /// <returns></returns>
        public double Score()
        {
            var nurseScore = this.Nurses.Sum(nurse => nurse.Score());
            var shiftScore = this.Shifts.Sum(shift => shift.Score());
            return nurseScore + shiftScore;
        }

        public override string ToString()
        {
            return $@"{string.Join('\n', this.Nurses)}{'\n'}";
        }
    }
}
