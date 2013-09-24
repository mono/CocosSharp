using System;
#if WINDOWS_PHONE
using System.ComponentModel;
#else
using System.Threading.Tasks;
#endif

namespace Cocos2D
{
    public static class CCTask
    {
        private class TaskSelector : ICCSelectorProtocol
        {
            public void Update(float dt)
            {
            }
        }

        private static ICCSelectorProtocol _taskSelector = new TaskSelector();

        public static object RunAsync(Action action)
        {
            return RunAsync(action, null);
        }
		
        public static object RunAsync(Action action, Action<object> taskCompleted)
        {
#if WINDOWS_PHONE
            var worker = new BackgroundWorker();
            
            worker.DoWork +=
                (sender, args) =>
                {
                    action();
                };

            if (taskCompleted != null)
            {
                worker.RunWorkerCompleted +=
                    (sender, args) =>
                    {
                        var scheduler = CCDirector.SharedDirector.Scheduler;
                        scheduler.ScheduleSelector(f => taskCompleted(worker), _taskSelector, 0, 0, 0, false);
                    };
            }

            worker.RunWorkerAsync();

            return worker;
#else
            var task = new Task(
                () =>
                {
                    action();

                    if (taskCompleted != null)
                    {
                        var scheduler = CCDirector.SharedDirector.Scheduler;
                        scheduler.ScheduleSelector(f => taskCompleted(null), _taskSelector, 0, 0, 0, false);
                    }
                }
                );
                    
            task.Start();

            return task;
#endif
        }
    }
}
