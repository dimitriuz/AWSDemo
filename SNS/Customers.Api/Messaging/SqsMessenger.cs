using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging
{
    public class SqsMessenger : ISnsMessenger
    {
        private readonly IAmazonSimpleNotificationService _sns;
        private readonly IOptions<TopicSettings> _topicSettings;
        private string? _topicArn;

        public SqsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
        {
            _sns = sns;
            _topicSettings = topicSettings;
        }

        public async Task<PublishResponse> PublishMessageAsync<T>(T message)
        {
            var topicArn = await GetTopicArnAsync();

            var sendMessageRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue> {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    } }
            };

            var ct = new CancellationTokenSource();
            return await _sns.PublishAsync(sendMessageRequest, ct.Token);
        }

        private async ValueTask<string> GetTopicArnAsync()
        {
            if (_topicArn is null) {
                var findTopicResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
                _topicArn = findTopicResponse.TopicArn;
            };

            return _topicArn;
        }
    }
}
