using System;
using System.Collections.Generic;

namespace Nurse_Scheduling.Models
{
    public class Shift
    {
        public Shift(int id, int minNurses = 1)
        {
            this.Id = id;
            this.MinNurses = minNurses;
            this.Nurses = new HashSet<Nurse>();
        }

        public int Id;
        public int MinNurses;

        ISet<Nurse> Nurses;

        /// <summary>
        /// - All shift type demands during the planning period must be met
        /// - The shift coverage requirements must be fulfilled
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            return this.Nurses.Count < this.MinNurses ? 1 : 0; 
        }

        public override string ToString()
        {
            DayOfWeek dayOfWeek = (DayOfWeek) (Id / 2);
            var nightOrDay = Id % 2 == 0 ? "Day" : "Night";
            return $@"{dayOfWeek} {nightOrDay}: {string.Join(',', this.Nurses)}";
        }

        public void Assign(Nurse nurse)
        {
            var wasAdded = this.Nurses.Add(nurse);

            if (!wasAdded)
            {
                throw new Exception();
            }
        }

        public void Unassign(Nurse nurse)
        {
            var wasRemoved = this.Nurses.Remove(nurse);

            if (!wasRemoved)
            {
                throw new Exception();
            }
        }
    }
}
