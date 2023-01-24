using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSConsumer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var sqsClient = new AmazonSQSClient();
            
            var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrlResponse.QueueUrl,
                AttributeNames = new List<string> { "All" }
            };

            var cts = new CancellationTokenSource();

            while (!cts.IsCancellationRequested)
            {
                var response = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);
               
                response.Messages.ForEach(async message =>
                {
                    Console.WriteLine($"Message Body : {message.Body}");
                    Console.WriteLine($"Message ID: {message.MessageId}");
                    await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
                });

                await Task.Delay(3000);
            }

        }
    }
}