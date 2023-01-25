using Amazon.SQS;
using MediatR;

namespace Customers.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<QueueSettings>(builder.Configuration.GetSection(QueueSettings.Key));
            builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
            builder.Services.AddHostedService<QueueConsumerService>();
            builder.Services.AddMediatR(typeof(Program));

            var app = builder.Build();

            app.Run();
        }
    }
}