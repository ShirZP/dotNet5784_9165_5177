using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Milestone
    {
        /// <summary>
        /// Personal unique ID for the milestone (automatic number)
        /// </summary>
        public int ID { get; init; }

        /// <summary>
        /// Nickname of the milestone
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// A description of the tasks included in this milestone
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Milestone creation date
        /// </summary>
        public DateTime CreateAtDate { get; set; }

        /// <summary>
        /// The status of the milestone
        /// </summary>
        public BO.Status Status { get; set; }

        /// <summary>
        /// The latest possible date to complete the milestone
        /// </summary>
        public DateTime? DeadlineDate { get; set; }

        /// <summary>
        /// Estimated date for the completion of the milestone
        /// </summary>
        public DateTime? ForcastDate { get; set; }

        /// <summary>
        /// Actual milestone completion date
        /// </summary>
        public DateTime? CompleteDate { get; set; }

        /// <summary>
        /// The percentage of completed assignments (of the assignments on which the milestone depends)
        /// </summary>
        public double? CompletionPercentage { get; set; }

        /// <summary>
        /// Option to add remarks on the milestone
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// A list of tasks that the current milestone depends on
        /// </summary>
        public List<TaskInList>? Dependencies { get; set; }
    }
}
