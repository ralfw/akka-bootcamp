using System;
using Akka.Actor;

namespace FlowDesignExperiment
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var actorSys = ActorSystem.Create ("experiment");

			ActorRef writer = actorSys.ActorOf(Props.Create<Actors.Writer>());
			var validator = actorSys.ActorOf (Props.Create(() => new Actors.Validator(writer, writer)));
			var reader = actorSys.ActorOf (Props.Create(() => new Actors.Reader(validator)));
		
			reader.Tell (new Actors.Messages.Start ());

			actorSys.AwaitTermination ();
		}
	}


	namespace Actors {
		namespace Messages {
			class Error {
				public string Explanation;
			}

			class Continue {}

			class Start {}
		}


		class Reader : UntypedActor {
			protected override void OnReceive (object message)
			{
				if (message is Messages.Start)
					Console.WriteLine("Enter text or leave blank for exit");

				var data = Console.ReadLine ();

				if (data == "") {
					Context.System.Shutdown ();
					return;
				}

				DataRead.Tell (data);

				Self.Tell (new Messages.Continue ());
			}

			public Reader(ActorRef dataRead) {
				this.DataRead = dataRead;
			}
			private ActorRef DataRead;
		}


		class Validator : UntypedActor {
			protected override void OnReceive (object message)
			{
				var text = (string)message;
				if (text.Length % 2 == 0)
					Success.Tell (text);
				else
					Failure.Tell (new Messages.Error{ Explanation = "Text must have even number of chars!" });
			}
				
			public Validator(ActorRef success, ActorRef failure) {
				this.Success = success;
				this.Failure = failure;
			}
			private ActorRef Success;
			private ActorRef Failure;
		}


		class Writer : UntypedActor {
			protected override void OnReceive (object message)
			{
				if (message is string)
					Console.WriteLine ("{0}", message);
				else if (message is Messages.Error)
					Console.WriteLine ("  Failure: {0}", (message as Messages.Error).Explanation);
			}
		}
	}
}
