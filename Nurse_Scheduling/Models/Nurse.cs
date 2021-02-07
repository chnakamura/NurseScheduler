using System;
using System.Collections.Generic;
using System.Linq;

namespace Nurse_Scheduling.Models
{
    public class Nurse
    {
        private static Random random = new Random(0);

        public Nurse(Dictionary<int, Shift> shifts, int id, HashSet<int> unwantedShifts)
        {
            this.Shifts = shifts;
            this.Id = id;
            this.UnwantedShifts = unwantedShifts;
        }

        public static Nurse NurseCopier(Nurse nurse)
        {
            var shifts = nurse.Shifts.ToDictionary(shift => shift.Key, shift => new Shift(shift.Value.Id, shift.Value.MinNurses));
            var newNurse = new Nurse(shifts, nurse.Id, nurse.UnwantedShifts);

            foreach (var shift in newNurse.Shifts.Values)
            {
                shift.Assign(newNurse);
            }

            return newNurse;
        }

        public HashSet<int> UnwantedShifts;
        public Dictionary<int, Shift> Shifts;
        public int Id;

        /// <summary>
        /// - Each nurse should work at most one shift per day
        /// TODO - Allow admin to define thir own cost functions!
        /// TODO - Allow each nurse to define their own cost function!
        /// </summary>
        /// <returns></returns>
        public int Weight()
        {
            var weight = 0;

            if (this.TwoShiftsInARow())
            {
                weight += Algorithm.HARD_CONSTRAINT_MULTIPLIER;
            }

            if (!this.Shifts.Any())
            {
                weight += Algorithm.SOFT_CONSTRAINT_MULTIPLIER;
            }

            if (this.Shifts.Count >= 3)
            {
                weight += Algorithm.SOFT_CONSTRAINT_MULTIPLIER;
            }

            if (this.Shifts.Keys.Intersect(this.UnwantedShifts).Any())
            {
                weight += Algorithm.SOFT_CONSTRAINT_MULTIPLIER;
            }

            return weight;
        }

        public List<Shift> UnassignRange(int minId, int maxId)
        {
            var shiftsToUnassign = this.Shifts.Values
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
            this.Shifts.Add(shift.Id, shift);

            shift.Assign(this);
        }

        public void Unassign(Shift shift)
        {
            var wasRemoved = this.Shifts.Remove(shift.Id);

            if (!wasRemoved)
            {
                throw new Exception();
            }

            shift.Unassign(this);
        }

        public void UnassignAll()
        {
            foreach (var shift in Shifts.Values)
            {
                shift.Unassign(this);
            }
            this.Shifts = new Dictionary<int, Shift>();
        }

        public override string ToString()
        {
            return $@"{this.Id}";
        }

        private bool TwoShiftsInARow()
        {
            foreach (var shift in this.Shifts)
            {
                if (this.Shifts.ContainsKey(shift.Key + 1))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
