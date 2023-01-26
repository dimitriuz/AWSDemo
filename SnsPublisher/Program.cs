using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;

namespace SnsPublisher
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var customer = new CustomerCreated
            {
                Id = Guid.NewGuid(),
                FullName = "Dmitrii Leshchenko",
                Email = "dimitriuz@gmail.com",
                GitHubUsername = "dimitriuz",
                DateOfBirth = new DateTime(1985, 4, 20)
            };

            var snsClient = new AmazonSimpleNotificationServiceClient();

            var topicArnResponse = await snsClient.FindTopicAsync("customers");

            var publishRequest = new PublishRequest
            {
                TopicArn = topicArnResponse.TopicArn,
                Message = JsonSerializer.Serialize(customer),
                MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = nameof(CustomerCreated)
                        }
                    } }
            };

            var publishResponse = await snsClient.PublishAsync(publishRequest);


        }
    }
}