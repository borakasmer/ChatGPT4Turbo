using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTMicroservice
{
    public class RabbitMQClient : IDisposable
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        public EventingBasicConsumer GetConsumer(string channelName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //Check is RabbitMQ Channel Exists            
            if (QueueExists(channel, channelName))
            {

                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(channelName, true, consumer);

                return consumer;
            }
            else
            {
                return null;
            }
        }

        private bool QueueExists(IModel channel, string channelName)
        {
            try
            {
                // Passive declare to check if the queue exists
                channel.QueueDeclarePassive(channelName);
                return true;
            }
            catch (OperationInterruptedException ex)
            {
                // If the queue does not exist, an OperationInterruptedException will be thrown
                if (ex.ShutdownReason.ReplyCode == 404)
                {
                    return false;
                }

                // Re-throw the exception if it's not a 404 error
                throw;
            }
        }
    }
}