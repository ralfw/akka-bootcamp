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

        private ActorRef _validationActor;

        public ConsoleReaderActor(ActorRef validationActor)
        {
			_validationActor = validationActor;
        }

        protected override void OnReceive(object message)
        {
			if (message.Equals (StartCommand))
				DoPrintInstructions ();

			GetAndValidateInput ();
        }



		private void DoPrintInstructions()
		{
			Console.WriteLine("Write whatever you want into the console!");
			Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
			Console.WriteLine("Type 'exit' to quit this application at any time.\n");
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
			_validationActor.Tell(message);
		}
    }
}