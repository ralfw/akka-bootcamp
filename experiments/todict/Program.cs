using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

using Akka;
using Akka.Actor;


namespace todict
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (var akka = ActorSystem.Create ("sys")) {
				var result = Inbox.Create (akka);
				var todict = akka.ActorOf (Props.Create (() => new ToDictionary (result.Receiver)));

				while (true) {
					Console.Write ("config: ");
					var config = Console.ReadLine ();
					if (config == "") break;

					todict.Tell (config);
					var dict = result.Receive () as Dictionary<string,string>;

					foreach (var k in dict.Keys) {
						Console.WriteLine ("  {0}:{1}", k, dict[k]);
					}
				}

				akka.Shutdown ();
			}
		}
	}


	class ToDictionary : ReceiveActor {
		public ToDictionary(ActorRef onResult) {
			var builddict = Context.ActorOf (Props.Create (() => new BuildDict(onResult)));
			var splitassign = Context.ActorOf (Props.Create(() => new SplitAssignments(builddict)));
			var splitcfg = Context.ActorOf (Props.Create (() => new SplitConfig (splitassign)));

			Receive<string> (config => splitcfg.Tell(config));
		}
	}

	
	class SplitConfig : TypedActor, IHandle<string> {
		#region IHandle implementation

		public void Handle (string message)
		{
			var parts = message.Split (';');
			onAssignments.Tell (parts);
		}

		#endregion


		ActorRef onAssignments;

		public SplitConfig(ActorRef onAssignments) {
			this.onAssignments = onAssignments;
		}
	}


	class SplitAssignments : TypedActor, IHandle<string[]> {
		#region IHandle implementation

		public void Handle (string[] assignments)
		{
			var keyValues = assignments.Select (a => a.Split ('='));
			onKeyValues.Tell (keyValues);
		}

		#endregion


		ActorRef onKeyValues;

		public SplitAssignments(ActorRef onKeyValues) {
			this.onKeyValues = onKeyValues;
		}
	}


	class BuildDict : TypedActor, IHandle<IEnumerable<string[]>> {
		#region IHandle implementation

		public void Handle (IEnumerable<string[]> keyValues)
		{
			var dict = keyValues.Aggregate(new Dictionary<string,string>(),
										   (result, kv) => {
												result[kv[0]] = kv[1]; 
												return result;
										   });
			onDict.Tell (dict);
		}

		#endregion


		ActorRef onDict;

		public BuildDict(ActorRef onDict) {
			this.onDict = onDict;
		}
	}
}
