using System;
using System.Runtime.InteropServices;

namespace Wander.StressTest.App
{
    class Program
    {
        static Starter _starter;
        static void Main(string[] args)
        {
            _starter = new Starter();
            _starter.Start();
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            Console.ReadLine();
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                _starter.Simulation.DeleteAllUsers();
            }
            return false;
        }
        static ConsoleEventDelegate handler;   
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }



    class Starter
    {
        Simulation _simulation;
        bool _useDistantHost = true;
        public Starter()
        {
            Console.WriteLine(
    "Welcome to the Wander Player Stress Test");
            Console.WriteLine("Use localhost ? y/n");
            int port = -1;
            char input = 'Y';
            do
            {
                try
                {
                    input = Console.ReadKey().KeyChar;
                    
                }
                catch (System.FormatException f)
                {

                }
                

                if (input == 'y' || input == 'n')
                {
                    
                    if (input == 'y')
                    {
                        Console.WriteLine(" ");
                        
                        
                        do
                        {
                            Console.WriteLine("Port ? ");
                            try
                            {
                                port = Convert.ToInt32(Console.ReadLine());
                                if (port <= 0)
                                {
                                    port = -1;
                                    Console.WriteLine("Invalid port");
                                }
                            }
                            catch (System.FormatException f)
                            {
                                port = -1;
                                Console.WriteLine("Invalid port");
                            }
                        } while (port == -1);
                    }
                    else
                    {
                        break;
                    }
                    
                }
                else
                {
                    Console.WriteLine("Invalid number, please input a valid one");
                }
            } while (port == -1);


            Console.WriteLine("Please enter the desired number of simulated players");
            int nbr = 0;
            bool valid = true;
            do
            {
                try
                {
                    nbr = Convert.ToInt32(Console.ReadLine());
                    valid = true;
                }
                catch (System.FormatException f)
                {
                    valid = false;
                }
                
                if (nbr > 0 && valid)
                {
                    _simulation = new Simulation(input == 'y' ? "http://localhost:" + port : "http://wander.nightlydev.fr", "GameHub", nbr);
                }
                else
                {
                    Console.WriteLine("Invalid number, please input a valid one");
                }
            } while (_simulation == null);


        }

        public void Start()
        {
            _simulation.Start();
        }

        public Simulation Simulation
        {
            get { return _simulation; }
        }
    }
}
