using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;
using vPref.Remoting;

public class vPrefAgent
{
    [SecurityPermission(SecurityAction.Demand)]
    public static void Main(string[] args)
    {
        // Create the channel.
        TcpChannel clientChannel = new TcpChannel();

        // Register the channel.
        ChannelServices.RegisterChannel(clientChannel, false);

        //// Register as client for remote object.
        //WellKnownClientTypeEntry remoteType = new WellKnownClientTypeEntry(
        //    typeof(IVPrefServiceProvider), "tcp://localhost:6001/vPreServer.rem");
        //RemotingConfiguration.RegisterWellKnownClientType(remoteType);

        //// Create a message sink.
        //string objectUri;
        //System.Runtime.Remoting.Messaging.IMessageSink messageSink =
        //    clientChannel.CreateMessageSink(
        //        "tcp://localhost:6001/vPreServer.rem", null,
        //        out objectUri);
        //Console.WriteLine("The URI of the message sink is {0}.",
        //    objectUri);
        //if (messageSink != null)
        //{
        //    Console.WriteLine("The type of the message sink is {0}.",
        //        messageSink.GetType().ToString());
        //}

        // Create an instance of the remote object.
        IVPrefServiceProvider service = (IVPrefServiceProvider)RemotingServices.Connect(typeof(IVPrefServiceProvider), "tcp://localhost:6001/vPreServer.soap");

        // Invoke a method on the remote object.
        Console.WriteLine("The client is invoking the remote object.");
        Console.WriteLine("count is " + service.returnInterCount());
     
    }
}