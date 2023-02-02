using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //await UploadFile();
        await DownloadFile();
    }

    private static async Task UploadFile()
    {
        var s3client = new AmazonS3Client();

        await using var inputStream = new FileStream("./movies.csv", FileMode.Open, FileAccess.Read);

        var putObjectRequest = new PutObjectRequest
        {
            BucketName = "ldsoft",
            Key = "files/movies.csv",
            ContentType = "text/csv",
            InputStream = inputStream
        };

        await s3client.PutObjectAsync(putObjectRequest);
    }

    private static async Task DownloadFile()
    {
        var s3client = new AmazonS3Client();


        var getObjectRequest = new GetObjectRequest
        {
            BucketName = "ldsoft",
            Key = "files/movies.csv"
        };

        var response = await s3client.GetObjectAsync(getObjectRequest);

        var memoryStream = new MemoryStream();

        response.ResponseStream.CopyTo(memoryStream);

        var text = Encoding.Default.GetString(memoryStream.ToArray());

        Console.WriteLine(text);
    }

}