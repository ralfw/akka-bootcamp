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
			if (message.Equals (StartCommand))
				DoPrintInstructions ();

			GetAndValidateInput ();

			Self.Tell (new ContinueProcessing ());
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