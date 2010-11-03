using System.Linq;
using System.Data.Linq.Mapping;

namespace Vivina.Erp.DataClasses
{
    public partial class TaskStatus
    {
        public const int Concluded = 3;
        public const int InProcess = 2;
        public const int Proposed = 1;
    }

    public partial class Task
    {
        private bool? _hasChildTasks;
                
        public bool HasChildTasks
        {
            get { return _hasChildTasks ?? (_hasChildTasks = Tasks.Any()).Value; }
            set { _hasChildTasks = value; }
        }
    }
}