namespace SQSPublisher
{
    internal class CustomerCreated
    {
        public required Guid Id { get; init; }
        public required String FullName { get; init; }
        public required String Email { get; init; }
        public required String GitHubUsername { get; init; }
        public required DateTime DateOfBirth { get; init; }
    }
}
