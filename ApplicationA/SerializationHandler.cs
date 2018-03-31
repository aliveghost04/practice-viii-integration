using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationA
{
    class SerializationHandler
    {
        private String filename = "config.dat";
        public void serialize(String executablePath) {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write(executablePath);
            sw.Close();
        }

        public String deserialize() {
            StreamReader sr = new StreamReader(filename);
            return sr.ReadToEnd();
        }
    }
}
