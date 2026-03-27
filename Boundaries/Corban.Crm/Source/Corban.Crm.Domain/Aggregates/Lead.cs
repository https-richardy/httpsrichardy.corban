namespace Corban.Crm.Domain.Aggregates;

public sealed class Lead : Aggregate
{
    public string CustomerId { get; set; } = default!;
    public string PipelineId { get; set; } = default!;

    /* a snapshot of the customer's data at the time the lead is created or updated. */
    /* used to avoid additional queries and optimize read operations */

    public string Name { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Document { get; set; } = default!;

    public Stage Stage { get; set; } = Stage.Undefined;
    public Dictionary<string, string> Metadata { get; set; } = [];

    public ICollection<History> History { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}
