using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationB
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientEntities client = new ClientEntities();

            if (args.Length > 0)
            {
                try
                {
                    var cedula = args[0];
                    var result = (from el in client.clients
                                  where el.cedula.Equals(cedula)
                                  select el).ToList();

                    if (result.Count == 1)
                    {
                        Console.Write(result[0].path);
                        Environment.ExitCode = 0;
                    } else
                    {
                        Console.Write("Not found");
                        Environment.ExitCode = 1;
                    }
                }
                catch (Exception e) {
                    Console.Write("Error");
                    Environment.ExitCode = 1;
                }
                
            } else {
                Console.WriteLine("Missing cedula parameter");
                Environment.ExitCode = 1;
            }
            
        }
    }
}
