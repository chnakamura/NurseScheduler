using System;
using System.Collections.Generic;

namespace Nurse_Scheduling.Models
{
    public class Shift
    {
        public Shift(int id, int minNurses = 1)
        {
            this.id = id;
            this.MinNurses = minNurses;
            this.Nurses = new List<Nurse>();
        }

        public int id;
        public int MinNurses;

        IList<Nurse> Nurses;

        /// <summary>
        /// - All shift type demands during the planning period must be met
        /// - The shift coverage requirements must be fulfilled
        /// </summary>
        /// <returns></returns>
        public double Score()
        {
            return Math.Min(this.MinNurses, this.Nurses.Count - this.MinNurses);
        }

        public override string ToString()
        {
            DayOfWeek dayOfWeek = (DayOfWeek) (id / 2);
            var nightOrDay = id % 2 == 0 ? "Day" : "Night";

            return $@"{dayOfWeek} {nightOrDay}";
        }

        public void Assign(Nurse nurse)
        {
            this.Nurses.Add(nurse);
        }

        public void Unassign(Nurse nurse)
        {
            if (!this.Nurses.Contains(nurse))
            {
                throw new Exception();
            }

            this.Nurses.Remove(nurse);
        }
    }
}
