using System;
using System.Collections.Generic;
using System.Linq;

namespace Nurse_Scheduling.Models
{
    public class Nurse
    {
        public Nurse(List<Shift> shifts)
        {
            this.Shifts = shifts;
        }

        public List<Shift> Shifts;

        /// <summary>
        /// - Each nurse should work at most one shift per day
        /// TODO - Allow admin to define thir own cost functions!
        /// TODO - Allow each nurse to define their own cost function!
        /// </summary>
        /// <returns></returns>
        public double Score()
        {
            var score = 0;

            score += this.Shifts.Any() ? 1 : -1;

            score += this.Shifts.Count < 3 ? 1 : -1;

            var maxShiftsInRow = this.MaxShiftsInRow();
            score += maxShiftsInRow == 1 ? 1 : -maxShiftsInRow;

            return score;
        }

        public List<Shift> UnAssign(int minId, int maxId)
        {
            var unAssignedShifts = this.Shifts
                .Where(shift => shift.id >= minId && shift.id <= maxId)
                .ToList();

            unAssignedShifts.ForEach(shift => shift.Unassign(this));

            this.Shifts = this.Shifts
                .Where(shift => !(shift.id >= minId && shift.id <= maxId))
                .ToList();

            return unAssignedShifts;
        }

        public void Assign(List<Shift> shifts)
        {
            shifts.ForEach(shift => shift.Assign(this));
            this.Shifts.AddRange(shifts);
        }

        public override string ToString()
        {
            return $@"Nurse Shifts: {string.Join(',', this.Shifts)}";
        }

        private int MaxShiftsInRow()
        {
            return this.Shifts.OrderBy(s => s.id).Aggregate(new
            {
                maxEndingHere = 0,
                max = 0,
                prev = (Shift) null
            }, (memo, curr) => {
                var maxEndingHere = memo.prev != null && memo.prev.id == curr.id - 1
                    ? memo.maxEndingHere + 1
                    : 1;

                var max = Math.Max(memo.max, maxEndingHere);

                return new
                {
                    maxEndingHere,
                    max,
                    prev = curr
                };
            }).max;
        }
    }
}
