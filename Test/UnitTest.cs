using System;
using System.Threading;
using NUnit.Framework;
using Tracer;

namespace Test
{
    public class Tests
    {
        private TraceResult _traceResult;

        [SetUp]
        public void Setup()
        {
            ITracer tracer = new TimeTracer();
            FirstTestClass _class = new FirstTestClass(tracer);
            _class.InnerMethod1SleepTime = 100;
            _class.InnerMethod2SleepTime = 50;
            _class.AnotherInnerMethod1SleepTime = 150;
            _class.AnotherThreadMethodSleepTime = 100;
            _class.StartMethod();
            _traceResult = tracer.GetTraceResult();
        }
        
        [Test]
        public void ResultNotNullTest()
        {
            Assert.NotNull(_traceResult);
        }
        
        [Test]
        public void ResultHasExactThreadsCountTest()
        {
            int expected = 2;
            int actual = _traceResult.Threads.Count;
            Assert.AreEqual(expected, actual);
        }
        

        [Test]
        public void ResultHasTracedMethodTest()
        {
            string expected = "AnotherThreadMethod";
            Assert.AreEqual(expected, _traceResult.Threads[0].Methods[0].Name);
        }
        
        [Test]
        public void ResultHasCorrectExecutionTime()
        {
            Assert.IsTrue(Math.Abs(100 - _traceResult.Threads[1].Methods[0].Methods[0].Time) < 5);
        }

        
    }

    public class FirstTestClass
    {
        private ITracer _tracer;

        public int InnerMethod1SleepTime { get; set; }

        public int InnerMethod2SleepTime { get; set; }

        public int AnotherInnerMethod1SleepTime { get; set; }
        
        public int AnotherThreadMethodSleepTime { get; set; }

        public FirstTestClass(ITracer tracer)
        {
            _tracer = tracer;
        }
        
        public void StartMethod()
        {
            SecondTestClass testInAnotherThread = new SecondTestClass(_tracer);
            testInAnotherThread.EndChainMethodSleepTime = AnotherThreadMethodSleepTime;
            Thread newThread = new Thread(testInAnotherThread.AnotherThreadMethod);
            newThread.Start();
            newThread.Join();
            SomeMethod();
            AnotherMethod();
        }

        private void SomeMethod()
        {
            _tracer.StartTrace();
            InnerMethod1();
            InnerMethod2();
            _tracer.StopTrace();

        }

        private void AnotherMethod()
        {
            _tracer.StartTrace();
            AnotherInnerMethod1();
            _tracer.StopTrace();
        }

        private void InnerMethod1()
        {
            _tracer.StartTrace();
            Thread.Sleep(InnerMethod1SleepTime);
            _tracer.StopTrace();
        }

        private void InnerMethod2()
        {
            _tracer.StartTrace();
            Thread.Sleep(InnerMethod2SleepTime);
            _tracer.StopTrace();
        }

        private void AnotherInnerMethod1()
        {
            _tracer.StartTrace();
            Thread.Sleep(AnotherInnerMethod1SleepTime);
            _tracer.StopTrace();
        }
        
    }

    public class SecondTestClass
    {
        private readonly ITracer _tracer;
        
        public int EndChainMethodSleepTime { get; set;  }

        public SecondTestClass(ITracer tracer)
        {
            _tracer = tracer;
        }
        
        public void AnotherThreadMethod()
        {
            _tracer.StartTrace();
            AnotherThreadInnerMethod();
            _tracer.StopTrace();
        }

        private void AnotherThreadInnerMethod()
        {
            _tracer.StartTrace();
            EndChainMethod();
            _tracer.StopTrace();
        }

        private void EndChainMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(EndChainMethodSleepTime);
            _tracer.StopTrace();
        }
    }
}