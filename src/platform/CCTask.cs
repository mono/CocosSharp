using System;
#if WINDOWS_PHONE
using System.ComponentModel;
#else
using System.Threading.Tasks;
#endif

namespace CocosSharp
{
    public static class CCTask
    {
        class TaskSelector : ICCUpdatable
        {
            public void Update(float dt)
            {
            }
        }

        static ICCUpdatable taskSelector = new TaskSelector();

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
                        var scheduler = CCApplication.SharedApplication.MainWindowDirector.Scheduler;
                        scheduler.Schedule (f => taskCompleted(worker), taskSelector, 0, 0, 0, false);
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
                        var scheduler = CCApplication.SharedApplication.Scheduler;
                        scheduler.Schedule (f => taskCompleted(null), taskSelector, 0, 0, 0, false);
                    }
                }
                );
                    
            task.Start();

            return task;
#endif
        }
    }
}
