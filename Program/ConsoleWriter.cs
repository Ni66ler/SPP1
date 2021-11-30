using System;

namespace Program
{
    namespace Writer
    {
        public class ConsoleWriter : IWriter
        {
            public void Write(string str)
            {
                Console.WriteLine(str);
            }
        }
    }
    
}