using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
		public const string StartCommand = "start";



        protected override void OnReceive(object message)
        {
<<<<<<< HEAD
			if (message.Equals (StartCommand))
				DoPrintInstructions ();

			GetAndValidateInput ();

			Self.Tell (new ContinueProcessing ());
=======
            var read = Console.ReadLine();
            if (!string.IsNullOrEmpty(read) && String.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the system (acquire handle to system via
                // this actors context)
                Context.System.Shutdown();
                return;
            }

            // send input to the console writer to process and print
            // YOU NEED TO FILL IN HERE

            // continue reading messages from the console
            // YOU NEED TO FILL IN HERE
>>>>>>> master
        }



		private void DoPrintInstructions()
		{
			Console.WriteLine("Please provide the URI of a log file on disk.\n");
		}


		private void GetAndValidateInput()
		{
			var message = Console.ReadLine();
			if (!string.IsNullOrEmpty(message) && message.ToLowerInvariant().Equals(ExitCommand))
			{
				// if user typed ExitCommand, shut down the entire actor system (allows the process to exit)
				Context.System.Shutdown();
				return;
			}

			// otherwise, just hand message off to validation actor (by telling its actor ref)
			Context.ActorSelection("akka://MyActorSystem/user/validationActor").Tell(message);
		}
    }
}