// See https://aka.ms/new-console-template for more information
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

Console.WriteLine("Hello, World!");

// Preparation
var factory = new MqttFactory();
using var client = factory.CreateMqttClient();

// Client options
var options = new MqttClientOptionsBuilder()
    .WithTcpServer("localhost")
    //.WithProtocolVersion(MqttProtocolVersion.V500)
    .Build();

// Subscribe to topic and processes the received message
client.ApplicationMessageReceivedAsync += async e =>
{
    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
    Console.WriteLine();

    // return Task.CompletedTask;
    return;
};

// Connecting
MqttClientConnectResult? response = await client.ConnectAsync(options, CancellationToken.None);
Console.WriteLine("The MQTT client is connected.");

// Subscribe
MqttClientSubscribeOptions? subscribeOptions = factory.CreateSubscribeOptionsBuilder()
    .WithTopicFilter(f =>
    {
        f.WithTopic("spaceship/heartrate");
    })
    .Build();
await client.SubscribeAsync(subscribeOptions, CancellationToken.None);

Console.WriteLine("MQTT client subscribed to topic");

Console.WriteLine("Press enter to exit...");
Console.ReadLine();

await client.DisconnectAsync();

