using System;
using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            // YOU NEED TO FILL IN HERE
			MyActorSystem = ActorSystem.Create ("MyActorSystem");

			var consoleWriterProps = Props.Create<ConsoleWriterActor>();
			var consoleWriterActor = MyActorSystem.ActorOf (consoleWriterProps, "consoleWriterActor");

			var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
			MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

			var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
		    MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

			var consoleReaderProps = Props.Create<ConsoleReaderActor> ();
			var consoleReaderActor = MyActorSystem.ActorOf (consoleReaderProps, "consoleReaderActor");


            // tell console reader to begin
            //YOU NEED TO FILL IN HERE
			consoleReaderActor.Tell (ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.AwaitTermination();
        }

    }
    #endregion
}
