using System;
using System.Windows.Forms;
using Akka.Actor;
using ChartApp.Actors;

using System.Configuration;
using Akka.Configuration.Hocon;


namespace ChartApp
{
    static class Program
    {
        /// <summary>
        /// ActorSystem we'll be using to publish data to charts
        /// and subscribe from performance counters
        /// </summary>
        public static ActorSystem ChartActors;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var config = section.AkkaConfig;
            ChartActors = ActorSystem.Create("ChartActors", config);
            //ChartActors = ActorSystem.Create("ChartActors");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
