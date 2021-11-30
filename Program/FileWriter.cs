using System.IO;

namespace Program
{
    namespace Writer
    {
        public class FileWriter : IWriter
        {
            private readonly string _path;
            public FileWriter(string path)
            {
                _path = path;
            }
            public void Write(string str)
            {
                using StreamWriter streamWriter = new StreamWriter(_path);
                streamWriter.Write(str);
            }
        }
    }
    
}