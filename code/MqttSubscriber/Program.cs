using System.Text;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Serilog;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Initializing...");

try
{
    // Zeebe Client
    var zeebeUrl = "zeebe:26500";
    var zeebeClient = ZeebeClient.Builder()
        .UseGatewayAddress(zeebeUrl)
        .UsePlainText()
        .Build();
    ITopology? topology = await zeebeClient.TopologyRequest()
        .Send();
    Log.Information("Zeebe topology: {@Topology}", topology);

    // MQTT Subscriber
    // Preparation
    var factory = new MqttFactory();
    using var client = factory.CreateMqttClient();

    // Client options
    var mqttServer = "mqtt_broker";
    var options = new MqttClientOptionsBuilder()
        .WithTcpServer(mqttServer)
        .Build();

    // Subscribe to topic and processes the received message
    client.ApplicationMessageReceivedAsync += async e =>
    {
        Log.Information(
            "Received Application Message: {@ApplicationMessage}",
            e.ApplicationMessage);

        string payloadStringified = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
        var payload = Convert.ToDouble(payloadStringified);
        string topic = e.ApplicationMessage.Topic.Split('/').Last();

        var correlationKey = "testing";

        var variables = new {
            topic,
            payload,
            correlationKey,
        };
        Log.Debug("Sending: {Variables}", variables);

        IPublishMessageResponse? response = await zeebeClient.NewPublishMessageCommand()
            .MessageName("message")
            .CorrelationKey(correlationKey)
            .Variables(JsonConvert.SerializeObject(variables))
            .Send();

        Log.Debug("Message response zeebe: {@Response}", response);
        return;
    };

    // Connecting
    MqttClientConnectResult? response = await client.ConnectAsync(options, CancellationToken.None);
    Console.WriteLine("The MQTT client is connected.");

    // Subscribe
    MqttClientSubscribeOptions? subscribeOptions = factory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(f => { f.WithTopic("spaceship/#"); })
        .Build();
    await client.SubscribeAsync(subscribeOptions, CancellationToken.None);

    Console.WriteLine("MQTT client subscribed to topics");

    Console.WriteLine("Press enter to exit...");
    Console.ReadLine();

    await client.DisconnectAsync();
}
catch (Exception ex)
{
    Log.Error(ex, "Something went wrong");
}
finally
{
    await Log.CloseAndFlushAsync();
}
