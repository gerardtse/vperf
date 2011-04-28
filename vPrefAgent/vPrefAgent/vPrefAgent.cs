using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;
using vPref.Remoting;
using System.Management;
using IOStat;
using System.Collections.Generic;

public class vPrefAgent
{
    private static String uuid = null;

    [SecurityPermission(SecurityAction.Demand)]
    public static void Main(string[] args)
    {
        // Generate unique system ID
        ManagementClass mc = new ManagementClass("Win32_Processor");
        ManagementObjectCollection moc = mc.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            uuid = mo.Properties["processorID"].Value.ToString();
            Console.WriteLine("Unique ID for agent: " + uuid);
            break;
        }

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

        while (true)
        {
            System.Threading.Thread.Sleep(2000);
            Dictionary<String, String> dict = IOStatManager.getInstance.getDiskInformation();
            service.reportIO(uuid, dict);
        }
    }
}