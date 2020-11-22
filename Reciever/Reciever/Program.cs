using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                var queueName = "multiple-channels-and-workers";

                for (var i = 1; i < 3; i++)
                {
                    var channel = CreateChannel(connection);

                    channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                    for (var j = 1; j < 6; j++)
                    {
                        BuildWorker(channel, queueName, $"Worker {i}:{j}");
                    }
                }

                Console.ReadLine();
            }
        }

        public static IModel CreateChannel(IConnection connection) =>
            connection.CreateModel();

        public static void BuildWorker(IModel channel, string queueName, string worker)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{channel.ChannelNumber} - {worker} - Recebeu {message}");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
