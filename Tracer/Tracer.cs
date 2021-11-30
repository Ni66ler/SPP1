using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using System.Reflection;

namespace Tracer
{
    public class TimeTracer : ITracer
    {
        private TraceResult traceResult = new ();
        private ConcurrentDictionary<int, Stack<(MethodResult, Stopwatch)>> threadDictionary = new ();
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            Stopwatch stopwatch = new Stopwatch();
            StackFrame frame = new StackFrame(1);
            MethodBase frameMethod = frame.GetMethod();
            MethodResult methodResult = new MethodResult();
            methodResult.ClassName = frameMethod.DeclaringType.Name;
            methodResult.Name = frameMethod.Name;
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (threadDictionary.TryAdd(ThreadId, new Stack<(MethodResult, Stopwatch)>()))
            {
                traceResult.Threads.Add(new ThreadResult { Id = ThreadId });
            }
            stopwatch.Start();
            threadDictionary[ThreadId].Push((methodResult, stopwatch));
        }

        public void StopTrace()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            (MethodResult ThisMethod, Stopwatch stopwatch) = threadDictionary[ThreadId].Pop();
            stopwatch.Stop();
            ThisMethod.Time = stopwatch.ElapsedMilliseconds;

            if (threadDictionary[ThreadId].Count != 0)
            {
                (MethodResult PreMethod, _) = threadDictionary[ThreadId].Peek();
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<MethodResult>();
                }
                PreMethod.Methods.Add(ThisMethod);
            }
            else
            {
                int ThreadIndex = traceResult.Threads.FindIndex(_thread => _thread.Id == ThreadId);
                if (traceResult.Threads[ThreadIndex].Methods == null)
                {
                    traceResult.Threads[ThreadIndex].Methods = new List<MethodResult>();
                }
                traceResult.Threads[ThreadIndex].Methods.Add(ThisMethod);
                traceResult.Threads[ThreadIndex].Time += ThisMethod.Time;
            }
        }
    }
}
