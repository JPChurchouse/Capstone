using System.Runtime.CompilerServices;
using XKarts;
using XKarts.Mqtt;

string name = "ConsoleApp";
string address = "192.168.1.108";


Mqtt client = new Mqtt(name, address);
client.OnReceived += MsgReceived;

Packet pinger = new Packet("test/topic", "ping");

static void MsgReceived(Packet packet)
{
    Console.WriteLine("main func");
    Console.WriteLine(packet.ToString());
}

while (true)
{
    while (!client.IsConnected())
    {
        client.TryConnect();
        Task.Delay(3000).Wait();
    }
    while (client.IsConnected())
    {

        client.Send(pinger);
        Task.Delay(3000).Wait();
    }
}
