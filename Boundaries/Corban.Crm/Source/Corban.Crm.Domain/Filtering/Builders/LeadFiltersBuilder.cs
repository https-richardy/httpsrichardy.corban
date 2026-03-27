namespace Corban.Crm.Domain.Filtering.Builders;

public sealed class LeadFiltersBuilder : FiltersBuilderBase<LeadFilters, LeadFiltersBuilder>
{
    public LeadFiltersBuilder WithCustomerId(string customerId)
    {
        if (!string.IsNullOrWhiteSpace(customerId))
            _filters.CustomerId = customerId;

        return this;
    }

    public LeadFiltersBuilder WithPipelineId(string pipelineId)
    {
        if (!string.IsNullOrWhiteSpace(pipelineId))
            _filters.PipelineId = pipelineId;

        return this;
    }

    public LeadFiltersBuilder WithStage(string stage)
    {
        if (!string.IsNullOrWhiteSpace(stage))
            _filters.Stage = stage.Trim().Normalize(NormalizationForm.FormC);

        return this;
    }

    public LeadFiltersBuilder WithName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            _filters.Name = name.Trim().Normalize(NormalizationForm.FormC);

        return this;
    }

    public LeadFiltersBuilder WithPhoneNumber(string phoneNumber)
    {
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            _filters.PhoneNumber = phoneNumber.Trim().SanitizeNumbers();

        return this;
    }

    public LeadFiltersBuilder WithDocument(string document)
    {
        if (!string.IsNullOrWhiteSpace(document))
            _filters.Document = document.Trim().SanitizeNumbers();

        return this;
    }
}