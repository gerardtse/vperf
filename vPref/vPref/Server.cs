using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using vPref.Remoting;



namespace vPref
{
    public class Server : MarshalByRefObject, IVPrefServiceProvider
    {
        public const int PORT = 6001;
        public const string URI = "vPreServer.soap";

        public static IVPrefServiceProvider Instance()
        {
            if (mInstance == null)
            {
                mInstance = new Server();
            }
            return mInstance;
        }

        private Server()
        {
            RegisterRemoteServer();
        }



        #region IVPrefServiceProvider

        public int returnInterCount()
        {
            return 100;
        }

        #endregion

        #region Private Members

        private static IVPrefServiceProvider mInstance = null;

        private void RegisterRemoteServer()
        {
            string configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            try
            {
                Console.WriteLine(configFilePath);
                RemotingConfiguration.Configure(configFilePath, false);
                RegisterForRemoting();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("vPref will run remoting with default configuration.");
                RegisterForRemoting();
            }
        }
        private void RegisterForRemoting()
        {
            RemotingServices.Marshal(this, URI);
            foreach (IChannel channel in ChannelServices.RegisteredChannels)
            {
                Console.WriteLine("Registered channel: " + channel.ChannelName);

                if (channel is IChannelReceiver)
                {
                    foreach (string url in ((IChannelReceiver)channel).GetUrlsForUri(URI))
                    {
                        Console.WriteLine("vPref Server: Listening on url: " + url);
                    }
                }
            }
        }
        #endregion


        public static void Main()
        {
            //    TcpChannel serverChannel = new TcpChannel(9090);

            //    // Register the server channel.
            //    ChannelServices.RegisterChannel(serverChannel);

            //    // Show the name of the channel.
            //    Console.WriteLine("The name of the channel is {0}.",
            //        serverChannel.ChannelName);

            //    // Show the priority of the channel.
            //    Console.WriteLine("The priority of the channel is {0}.",
            //        serverChannel.ChannelPriority);

            //    // Show the URIs associated with the channel.
            //    ChannelDataStore data = (ChannelDataStore)serverChannel.ChannelData;
            //    foreach (string uri in data.ChannelUris)
            //    {
            //        Console.WriteLine("The channel URI is {0}.", uri);
            //    }

            //    // Expose an object for remote calls.
            //    RemotingConfiguration.RegisterWellKnownServiceType(
            //        typeof(RemoteObject), "RemoteObject.rem",
            //        WellKnownObjectMode.SingleCall);

            //    // Parse the channel's URI.
            //    string[] urls = serverChannel.GetUrlsForUri("RemoteObject.rem");
            //    if (urls.Length > 0)
            //    {
            //        string objectUrl = urls[0];
            //        string objectUri;
            //        string channelUri = serverChannel.Parse(objectUrl, out objectUri);
            //        Console.WriteLine("The object URL is {0}.", objectUrl);
            //        Console.WriteLine("The object URI is {0}.", objectUri);
            //        Console.WriteLine("The channel URI is {0}.", channelUri);
            //    }

            //    // Wait for the user prompt.
            //    Console.WriteLine("Press ENTER to exit the server.");
            //    Console.ReadLine();
            //}
            Server.Instance();
            Console.Read();

        }
    }
}