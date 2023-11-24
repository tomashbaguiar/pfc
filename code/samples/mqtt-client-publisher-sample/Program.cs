// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

// Preparation
var factory = new MqttFactory();
using var client = factory.CreateMqttClient();

// Client options
var options = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost")
    .WithProtocolVersion(MqttProtocolVersion.V500)
    .Build();

// Connecting
MqttClientConnectResult? response = await client.ConnectAsync(options, CancellationToken.None);
Console.WriteLine("The MQTT client is connected.");

// Publish message
var payload = new { field = 19.5 };
var message = new MqttApplicationMessageBuilder()
    .WithTopic("spaceship/monitoring")
    // .WithPayload(payload)
    .WithPayload(JsonConvert.SerializeObject(payload))
    .Build();

MqttClientPublishResult? publishResponse = await client.PublishAsync(message, CancellationToken.None);
Console.WriteLine($"publish response: {publishResponse?.IsSuccess}");

await client.DisconnectAsync();

Console.WriteLine("MQTT application message is published");

