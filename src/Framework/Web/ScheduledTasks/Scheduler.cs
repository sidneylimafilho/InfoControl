using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using InfoControl.Configuration;

namespace InfoControl.Web.ScheduledTasks
{
    public class Scheduler
    {
        #region Properties

        private static readonly Scheduler _instance = new Scheduler();
        private readonly Thread _thread;

        private List<ScheduledTask> _tasks;

        public static Scheduler Instance
        {
            get { return _instance; }
        }

        public List<ScheduledTask> Tasks
        {
            get
            {
                if (_tasks == null)
                    _tasks = new List<ScheduledTask>();
                return _tasks;
            }

            private set { _tasks = value; }
        }

        /// <summary>
        /// Scheduler Configuration
        /// </summary>
        public SchedulerSection SchedulerSection
        {
            get { return AppConfig.GetSection<SchedulerSection>("InfoControl/ScheduledTasks"); }
        }

        #endregion

        private Scheduler()
        {
            _thread = new Thread(CheckTimer);
        }

        #region Methods

        /// <summary>
        /// Verifies each second whether is to do work schedule task
        /// </summary>
        private void CheckTimer()
        {
            while (SchedulerSection.Enabled)
                using (var manager = new SchedulerManager(null))
                {
                    Tasks = manager.GetAllScheduleTasks().ToList();
                    foreach (ScheduledTask task in Tasks)
                        if (task.Enabled)
                        {
                            var start = task.StartTime;
                            var now = DateTime.Now;

                            if (start.Date == now.Date && start.Hour == now.Hour && start.Minute == now.Minute)
                                task.Start(ar => manager.SaveScheduleTask((ScheduledTask)ar.AsyncState));
                            else
                                while (task.StartTime < DateTime.Now.AddMinutes(-15))
                                    task.StartTime = task.StartTime.AddMinutes(task.Period);


                        }

                    Thread.Sleep(60000);
                }
        }

        /// <summary>
        /// Start the Scheduler
        /// </summary>
        public void Start()
        {
            if (_thread.ThreadState == ThreadState.Stopped ||
                _thread.ThreadState == ThreadState.Unstarted)
                _thread.Start();
        }

        /// <summary>
        /// Pause the scheduler
        /// </summary>
        public void Pause()
        {
            _thread.Suspend();
        }

        /// <summary>
        /// Resume the paused scheduler
        /// </summary>
        public void Resume()
        {
            if (_thread.ThreadState == ThreadState.Suspended)
                _thread.Resume();
        }

        /// <summary>
        /// Stop the scheduler
        /// </summary>
        public void Stop()
        {
            _thread.Abort();
        }

        #endregion
    }
}