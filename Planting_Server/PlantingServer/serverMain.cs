using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace PlantingServer
{
    class serverMain
    {
        public static bool keepAlive = true;

        static void Main()
        {
            // start server
            serverTCP startTCP = new serverTCP();

            // keep alive
            while (keepAlive)
            {
                doCMD(Console.ReadLine().ToLower());
            }
        }

        private static void doCMD(string p)
        {
            serverTCP TCP = new serverTCP();
            switch (p)
            {
                case "exit":
                    keepAlive = false;
                    break;
                case "tdb":
                    output.outToScreen("The database is " + (database.isDatabaseRead() ? "available." : "not available."));
                    break;

                case "test":
                    TCP.send();
                    break;

                case "showclientsinfo":
                    TCP.show_clientsinfo();
                    break;

                case "initializeclientsinfo":
                    TCP.initialize_clientsinfo();
                    break;

                case "disconnect":
                    TCP.Disconnect();
                    break;

                default:
                    output.outToScreen("There are no commands named " + p);
                    break;
            }
        }
    }
}
