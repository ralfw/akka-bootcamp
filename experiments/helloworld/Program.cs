using System;
using Akka.Actor;

public class MainClass
{
	public static void Main()
	{
		using(var akka = ActorSystem.Create("sys")) {
			var sh = akka.ActorOf(Props.Create(() => new SayHello()));

			sh.Tell("Peter");

			//System.Threading.Thread.Sleep (5000);
			Console.Write ("Waiting for ENTER...");
			Console.ReadLine ();
		}
	}
}


class SayHello : ReceiveActor {
	public SayHello() {
		Receive<string>(name => Console.WriteLine("Hello, {0}!", name));
	}
}