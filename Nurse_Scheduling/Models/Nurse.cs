using System;
using System.Collections.Generic;
using System.Linq;

namespace Nurse_Scheduling.Models
{
    public class Nurse
    {
        public Nurse(List<Shift> shifts, int id)
        {
            this.Shifts = new HashSet<Shift>(shifts);
            this.Id = id;
        }

        public static Nurse NurseCopier(Nurse nurse)
        {
            var shifts = nurse.Shifts.Select(shift => new Shift(shift.Id, shift.MinNurses)).ToList();
            var newNurse = new Nurse(shifts, nurse.Id);

            foreach (var shift in newNurse.Shifts)
            {
                shift.Assign(newNurse);
            }

            return newNurse;
        }

        public ISet<Shift> Shifts;
        public int Id;

        /// <summary>
        /// - Each nurse should work at most one shift per day
        /// TODO - Allow admin to define thir own cost functions!
        /// TODO - Allow each nurse to define their own cost function!
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            var weight = 0;

            if (!this.Shifts.Any())
            {
                weight += 1;
            }

            if (this.Shifts.Count >= 3)
            {
                weight += 1;
            }

            if (this.MaxShiftsInRow() > 1)
            {
                weight += 1;
            }

            return weight;
        }

        public List<Shift> UnassignRange(int minId, int maxId)
        {
            var shiftsToUnassign = this.Shifts
                .Where(shift => shift.Id >= minId && shift.Id <= maxId)
                .ToList();

            foreach (var shiftToUnassign in shiftsToUnassign)
            {
                this.Unassign(shiftToUnassign);
            }

            return shiftsToUnassign;
        }

        public void Assign(Shift shift)
        {
            var wasAdded = this.Shifts.Add(shift);

            if (!wasAdded)
            {
                throw new Exception();
            }

            shift.Assign(this);
        }

        public void Unassign(Shift shift)
        {
            var wasRemoved = this.Shifts.Remove(shift);

            if (!wasRemoved)
            {
                throw new Exception();
            }

            shift.Unassign(this);
        }

        public void UnassignAll()
        {
            foreach (var shift in Shifts)
            {
                shift.Unassign(this);
            }
            this.Shifts = new HashSet<Shift>();
        }

        public override string ToString()
        {
            return $@"{this.Id}";
        }

        private int MaxShiftsInRow()
        {
            return this.Shifts.OrderBy(s => s.Id).Aggregate(new
            {
                maxEndingHere = 0,
                max = 0,
                prev = (Shift) null
            }, (memo, curr) => {
                var maxEndingHere = memo.prev != null && memo.prev.Id == curr.Id - 1
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
