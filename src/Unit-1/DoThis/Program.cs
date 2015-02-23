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

			var validatonActorProps = Props.Create (() => new ValidationActor(consoleWriterActor));
			var validationActor = MyActorSystem.ActorOf (validatonActorProps, "validationActor");

			var consoleReaderProps = Props.Create<ConsoleReaderActor> (validationActor);
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
