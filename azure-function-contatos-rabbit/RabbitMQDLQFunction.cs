using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using System.Text;
using RabbitMQ.Client;

namespace azure_function_contatos_rabbit
{
    public class RabbitMQDLQFunction
    {
        private readonly string _rabbitMqHost = "192.168.1.15";  // IP do RabbitMQ
        private readonly string _rabbitMqUsername = "guest";
        private readonly string _rabbitMqPassword = "guest";

        [FunctionName("RabbitMqFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqHost,
                UserName = _rabbitMqUsername,
                Password = _rabbitMqPassword
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var dlq = "contatosQueue.dlq"; // Dead Letter Queue
                var queue = "contatosQueue";   // Fila destino

                // Tenta consumir mensagens da DLQ
                var result = channel.BasicGet(dlq, false); // Lê uma mensagem da fila sem declarar
                if (result != null)
                {
                    var messageBody = Encoding.UTF8.GetString(result.Body.ToArray());

                    // Reenvia para a fila principal
                    channel.BasicPublish("", queue, null, result.Body);

                    // Marca como processada
                    channel.BasicAck(result.DeliveryTag, false);
                }
            }
        }
    }
}
