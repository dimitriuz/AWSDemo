using Amazon.SQS;
using Amazon.SQS.Model;
using SQSPublisher;
using System.Text.Json;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var sqsClient = new AmazonSQSClient();

        var customer = new CustomerCreated
        {
            Id = Guid.NewGuid(),
            FullName = "Dmitrii Leshchenko",
            Email = "dimitriuz@gmail.com",
            GitHubUsername = "dimitriuz",
            DateOfBirth = new DateTime(1985, 4, 20)
        };

        var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageBody = JsonSerializer.Serialize(customer),
            MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = nameof(CustomerCreated)
                    }
                } 
            }
         
        };

        var response = await sqsClient.SendMessageAsync(sendMessageRequest);
}
}