using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;


namespace CreateGalaxyExample.DataConnection
{
    public static class GalaxyConnection
    {
        public static void Connect()
        {

            string Galaxy = ConfigurationManager.AppSettings.Get("Galaxy");
            IGalaxies gals = grAccess.QueryGalaxies(nodeName);
            Console.WriteLine("Accessing Galaxy...");
            if (gals == null || grAccess.CommandResult.Successful == false)
            {
                Console.WriteLine(grAccess.CommandResult.CustomMessage + grAccess.CommandResult.Text);
                return;
            }

            IGalaxy galaxy = gals[galaxyName];


            ICommandResult cmd;
        }
        
        public static void Login()
        {
            galaxy.Login("", "");
            Console.WriteLine("Logging into Galaxy...");
            cmd = galaxy.CommandResult;
            if (!cmd.Successful)
            {
                Console.WriteLine("Login to galaxy Example1 Failed :" +
                                  cmd.Text + " : " +
                                  cmd.CustomMessage);
                return;
            }
        }
    }
}
