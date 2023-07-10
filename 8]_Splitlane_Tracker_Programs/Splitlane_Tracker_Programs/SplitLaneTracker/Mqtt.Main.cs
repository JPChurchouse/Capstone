using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

namespace SplitlaneTracker.Services.Mqtt
{
  public partial class Mqtt
  {
    // Create a new MQTT client.
    private IMqttClient client = new MqttFactory().CreateMqttClient();
    private string client_name, broker_address, will_topic, will_payload;

    private Logging.Logger log = new Logging.Logger();

    // OnReceived user handler
    public delegate Task OnReceivedEventHandler(Packet packet);
    public event OnReceivedEventHandler? OnReceived;

    // OnConnected user handler
    public delegate Task OnConnectedEventHandler();
    public event OnConnectedEventHandler? OnConnected;

    // OnDisonnected user handler
    public delegate Task OnDisonnectedEventHandler();
    public event OnDisonnectedEventHandler? OnDisonnected;

    private bool stay_connected = true;

    public Mqtt(Logging.Logger l, string client_name, string broker_address, string will_topic, string will_payload, bool stay_connected = true)
    {
      log = l;
      log.log("Initalising MQTT");

      this.client_name = client_name;
      this.broker_address = broker_address;
      this.will_topic = will_topic;
      this.will_payload = will_payload;
      this.stay_connected = stay_connected;
    }

    public void OpenLog()
    {
      log.open();
    }

    // Event handler - On Client Disconnect
    private async Task On_Disconnect(MqttClientDisconnectedEventArgs args)
    {
      log.log("Mqtt.On_Disconnect()");

      if (stay_connected)
      {
        log.log($"Stay connected true");
        bool connected = await Connect();
        log.log($"Reconnection status: {connected}");
      }

      OnDisonnected?.Invoke();

      log.log("On_Disconnect() complete");
      return;
    }

    // Event handler - On Message Received
    private Task On_Receive(MqttApplicationMessageReceivedEventArgs args)
    {
      log.log("Mqtt.On_Receive()");

      OnReceived?.Invoke(
        new Packet(
          args.ApplicationMessage.Topic,
          Encoding.UTF8.GetString(args.ApplicationMessage.Payload)));

      log.log("On_Receive() complete");
      return Task.CompletedTask;
    }

    // Event handler - On Connected
    private Task On_Connected(MqttClientConnectedEventArgs args)
    {
      log.log("Mqtt.On_Connected()");

      OnConnected?.Invoke();

      log.log("On_Connected complete");
      return Task.CompletedTask;
    }

  }
}
