namespace Corban.Crm.Infrastructure.Constants;

public static class Documents
{
    public static class Lead
    {
        public const string Identifier = "_id";

        public const string CustomerId = "CustomerId";
        public const string PipelineId = "PipelineId";

        public const string Name = "Name";
        public const string PhoneNumber = "PhoneNumber";
        public const string Document = "Document";

        public const string Stage = "Stage";
        public const string Metadata = "Metadata";
    }

    public static class Customer
    {
        public const string Identifier = "_id";

        public const string Name = "Name";
        public const string DocumentNumber = "Document.Number";
        public const string PhoneNumber = "Phones.Number";

        public const string Source = "Marketing.Source";
        public const string Gender = "Gender";
        public const string BirthDate = "BirthDate";
        public const string Metadata = "Metadata";
    }

    public static class Pipeline
    {
        public const string Identifier = "_id";

        public const string Name = "Name";
        public const string Description = "Description";
    }
}
