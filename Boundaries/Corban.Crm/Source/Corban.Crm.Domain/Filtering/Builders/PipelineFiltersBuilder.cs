namespace Corban.Crm.Domain.Filtering.Builders;

public sealed class PipelineFiltersBuilder : FiltersBuilderBase<PipelineFilters, PipelineFiltersBuilder>
{
    public PipelineFiltersBuilder WithName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            _filters.Name = name.Trim().Normalize(NormalizationForm.FormC);

        return this;
    }

    public PipelineFiltersBuilder WithDescription(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            _filters.Description = description.Trim().Normalize(NormalizationForm.FormC);

        return this;
    }
}