using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace OgSim.Misc
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException()
        {
        }

        public TaskNotFoundException(string message) : base(message)
        {
        }
    }

    internal class Task
    {
        /* Quizas Stopwatch no es tan preciso */
        protected Stopwatch time = new Stopwatch();

        public Task()
        {
            time.Start();
        }

        public void Stop()
        {
            time.Stop();
        }

        public double Elapsed()
        {
            return time.Elapsed.Seconds + (time.Elapsed.Milliseconds / 1000.0);
        }
    }

    public class Profiler
    {
        readonly Dictionary<string, Task> timers = new Dictionary<string, Task>();

        public void StartTask(string key)
        {
            timers.Add(key, new Task());
        }

        public void StopTask(string key)
        {
            bool success = timers.TryGetValue(key, out Task timer);
            if (!success)
            {
                throw new TaskNotFoundException("Key " + key + " not found!");
            }
            timer.Stop();
        }

        public Dictionary<string, double> GetResults()
        {
            Dictionary<string, double> results = new Dictionary<string, double>();

            foreach (KeyValuePair<string, Task> kvp in timers)
            {
                results.Add(kvp.Key, kvp.Value.Elapsed());
            }
            return results;
        }
    }
}
