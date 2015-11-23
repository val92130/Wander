using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public Starter()
        {
            Console.WriteLine(
    "Welcome to the Wander Player Stress Test, please enter the desired ammount of simulated players ");
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
                    _simulation = new Simulation("http://wander.nightlydev.fr", "GameHub", nbr);
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
