using System;
using System.Collections.Generic;
using System.Linq;
using Nurse_Scheduling.Models;
using static Nurse_Scheduling.Models.Shift;

namespace Nurse_Scheduling
{
    public class InputGenerator
    {
        public static Assignment GererateInputs()
        {
            var nurses = GetNurses(7);

            // Assign each nurse subsequent shifts
            for (var i = 0; i < 7; i++)
            {  
                nurses[i].Assign(new List<Shift>
                {
                    new Shift(2*i),
                    new Shift(2*i+1)
                });
            }

            var shifts = nurses.SelectMany(nurse => nurse.Shifts).ToList();

            return new Assignment(nurses, shifts, 1.0);
        }

        private static List<Nurse> GetNurses(int numNurses)
        {
            var nurses = new List<Nurse>();
            for (var i = 0; i < numNurses; i++)
            {
                nurses.Add(new Nurse(new List<Shift>()));
            }
            return nurses;
        }
    }
}
