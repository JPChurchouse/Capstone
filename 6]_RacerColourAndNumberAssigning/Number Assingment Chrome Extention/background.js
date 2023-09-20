// MQTT broker configuration
var brokerUrl = "192.168.1.20";
var clientId = "chrome-extension-" + chrome.runtime.id;

// Load Paho MQTT library
var script = document.createElement("script");
script.src = chrome.runtime.getURL("lib/mqttws31.js");
script.onload = initializeMQTTClient;
document.head.appendChild(script);

// Initialize MQTT client
function initializeMQTTClient() {
  // Create a new MQTT client
  var client = new Paho.MQTT.Client(brokerUrl, 9001, clientId);

  // Set the callback handlers for connection lost and message arrivals
  client.onConnectionLost = onConnectionLost;
  client.onMessageArrived = onMessageArrived;

  // Connect to the MQTT broker
  client.connect({
    onSuccess: onConnect,
    onFailure: onFailure
  });

  // Callback function for successful connection
  function onConnect() {
    console.log("Connected to MQTT broker");
    // Subscribe to MQTT topics if needed
    client.subscribe("topic1");
    client.subscribe("topic2");
    publishMessage("World","Hello");
  }

  // Callback function for connection failure
  function onFailure(error) {
    console.log("Failed to connect to MQTT broker: " + error.errorMessage);
  }

  // Callback function for lost connection
  function onConnectionLost(responseObject) {
    if (responseObject.errorCode !== 0) {
      console.log("Connection lost: " + responseObject.errorMessage);
    }
  }

  // Callback function for received messages
  function onMessageArrived(message) {
    console.log("Received message: " + message.payloadString);
    publishMessage("Testing","Received a message");
  }

  function publishMessage() {
    var message = new Paho.MQTT.Message("Test Message");
    message.destinationName = "topic1";
    client.send(message);
  }
}
