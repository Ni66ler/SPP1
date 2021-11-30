using System.Threading;
using Program.Serializer;
using Program.Writer;
using Tracer;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ITracer tracer = new TimeTracer();
            TraceResult result;
            Example example = new Example(tracer);
            example.StartTest();
            result = tracer.GetTraceResult();
            ISerializer serializer = new JsonResultSerializer();
            IWriter writer = new ConsoleWriter();
            writer.Write(serializer.Serialize(result));
            serializer = new XmlResultSerializer();
            writer.Write(serializer.Serialize(result));
            
            writer = new FileWriter("res.xml");
            writer.Write(serializer.Serialize(result));
        }
    }
    
    class Example
    {
        private ITracer tracer;

        public Example(ITracer tracer)
        {
            this.tracer = tracer;
        }

        public void StartTest()
        {
            FirstMethod();
        }

        public void FirstMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(100);
            SecondMethod();
            ThirdMethod();
            tracer.StopTrace();
        }

        private void SecondMethod()
        {
            tracer.StartTrace();
            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(FifthMethod);
                thread.Start();
                thread.Join();
            }
            FourthMethod();
            tracer.StopTrace();
        }

        private void ThirdMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(300);
            tracer.StopTrace();
        }

        private void FourthMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(400);
            tracer.StopTrace();
        }

        private void FifthMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(500);
            SixthMethod();
            tracer.StopTrace();
        }

        private void SixthMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(600);
            tracer.StopTrace();
        }
    }
}