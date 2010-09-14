using System;
using System.Data.Linq;
using System.Threading;
using System.Web.Compilation;
using InfoControl.Data;

namespace InfoControl.Web.ScheduledTasks
{
    public interface IScheduledTaskWorker : IDataAccessor
    {
        DataManager SourceDataManager { get; }
        void DoWork();
    }

    public partial class ScheduledTask
    {
        private IScheduledTaskWorker _workerInstance;

        public IScheduledTaskWorker WorkerInstance
        {
            get
            {
                if (_workerInstance == null)
                {
                    Type type = BuildManager.GetType(TypeFullName, true, true);

                    _workerInstance = (IScheduledTaskWorker)Activator.CreateInstance(type);

                    //assembly.GetType(typeName));
                }
                return _workerInstance;
            }
        }

        public void Start(AsyncCallback callback)
        {
            Action action = WorkerInstance.DoWork;
            action.BeginInvoke(ar =>
            {
                var t = (ScheduledTask)ar.AsyncState;
                try
                {
                    action.EndInvoke(ar);
                    t.LastRunStatus = "Sucess";
                    t.StartTime = DateTime.Now.AddMinutes(t.Period);
                }
                catch (Exception ex)
                {
                    t.LastRunStatus = HandleException(ex);
                }
                finally
                {
                    callback(ar);
                }
            }, this.Duplicate());
        }

        private string HandleException(Exception ex)
        {
            return "<h2> <i>" + ex.Message + "</i> </h2><br /></span><code><pre>" + ex.StackTrace + "</pre></code>";
        }
    }
}