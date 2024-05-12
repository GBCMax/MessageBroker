using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MessageBroker.API.Controllers
{
    [ApiController]
    [Route("Rabbit")]
    public class RabbitController : ControllerBase
    {
        [HttpPost("Send")]
        public IActionResult Send(object message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "MyQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                    channel.BasicPublish(exchange: "",
                        routingKey: "MyQueue",
                        basicProperties: null,
                        body: body);
                }
            }
            return Ok(message);
        }
    }
}
