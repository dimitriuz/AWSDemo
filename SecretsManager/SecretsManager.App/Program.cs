using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace SecretsManager.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var smClient = new AmazonSecretsManagerClient();
           
            var secretValueRequest = new GetSecretValueRequest
            {
                SecretId = "ApiKey",
                VersionStage = "AWSCURRENT" //"AWSPREVIOUS"
            };

            var secretValueResponse = await smClient.GetSecretValueAsync(secretValueRequest);

            Console.WriteLine(secretValueResponse.SecretString);

            var describeSecretRequest = new DescribeSecretRequest
            {
                SecretId = "ApiKey"
            };

            var describeSecretResponse = await smClient.DescribeSecretAsync(describeSecretRequest);

            Console.WriteLine(describeSecretResponse.CreatedDate);
        }
    }
}