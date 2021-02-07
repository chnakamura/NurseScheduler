using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Nurse_Scheduling.Models
{
    public class Population
    {
        public Population(List<Assignment> assignments)
        {
            this.Assignments = assignments;
            this.NumCycles = 1;
        }

        public List<Assignment> Assignments;
        public int PopulationSize => this.Assignments.Count;
        public int NumCycles;
        public Assignment BestAssignment => this.Assignments.OrderBy(assignment => assignment.Weight()).First();

        public override string ToString()
        {
            var minWeight = this.Assignments.Select(a => a.Weight()).Min();
            return $@"Min Weight: {minWeight}{'\n'}" +
                $@"{string.Join('\n', this.Assignments)}";
        }
    }
}
