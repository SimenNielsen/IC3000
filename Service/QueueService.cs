using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace IC3000.Service
{
    public class QueueService
    {
        private ServiceBusClient _client;
        private protected string _queueName;
        /// <summary>
        /// Establish connection to Service bus.
        /// </summary>
        /// <param name="connectionString"></param>
        public QueueService(string connectionString)
        {
            _client = new ServiceBusClient(connectionString);
        }
        /// <summary>
        /// Send message to service bus queue.
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string _message)
        {

            ServiceBusSender sender = _client.CreateSender("Claim");

            // create a message that we can send
            ServiceBusMessage message = new ServiceBusMessage(_message);

            // send the message
            await sender.SendMessageAsync(message);
            Console.WriteLine($"Sent a single message to the queue: {_queueName}");
        }
    }
}
