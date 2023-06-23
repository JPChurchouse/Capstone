using System.Runtime.CompilerServices;
using XKarts;
using XKarts.Mqtt;

string client_name = "ConsoleApp";
string broker_address = "192.168.1.72";
string will_topic = "status/server";
string will_payload = "ERROR";



Mqtt client = new Mqtt(client_name, broker_address, will_topic, will_payload);
client.OnReceived += OnReceive;
client.OnConnected += OnConnect;
client.OnDisonnected += OnDisonnect;

Packet pinger = new Packet("ping/server", "ping");

void OnReceive(Packet packet)
{
    Console.WriteLine("RECEIVED");
    Console.WriteLine(packet.topic + " " + packet.payload);
}

void OnConnect()
{
    Console.WriteLine("CONNECTED");
    client.Subscibe("test");
    client.Send(new Packet("status/server", "ACTIVE"));
}

void OnDisonnect()
{
    Console.WriteLine("DISONNECTED");
}

while (true)
{
    int count = 0;
    while (!client.IsConnected())
    {
        client.TryConnect();
        Task.Delay(3000).Wait();
    }
    while (client.IsConnected())
    {

        client.Send(pinger);
        Task.Delay(3000).Wait();

        if (count++ > 10)
        {
            client.Send(new Packet("status/server", "DISCONNECTED"));
            client.Disconnect();
            return 0;
        }
    }
}
