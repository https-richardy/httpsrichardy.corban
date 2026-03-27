namespace Corban.Crm.Domain.Filtering.Builders;

public sealed class CustomerFiltersBuilder : FiltersBuilderBase<CustomerFilters, CustomerFiltersBuilder>
{
    public CustomerFiltersBuilder WithName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            _filters.Name = name.Trim().Normalize(NormalizationForm.FormC);

        return this;
    }

    public CustomerFiltersBuilder WithCnpj(string cnpj)
    {
        if (!string.IsNullOrWhiteSpace(cnpj))
            _filters.Cnpj = cnpj.Trim().SanitizeNumbers();

        return this;
    }

    public CustomerFiltersBuilder WithCpf(string cpf)
    {
        if (!string.IsNullOrWhiteSpace(cpf))
            _filters.Cpf = cpf.Trim().SanitizeNumbers();

        return this;
    }

    public CustomerFiltersBuilder WithPhoneNumber(string phoneNumber)
    {
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            _filters.PhoneNumber = phoneNumber.Trim().SanitizeNumbers();

        return this;
    }

    public CustomerFiltersBuilder WithSource(LeadSource source)
    {
        if (source != LeadSource.Undefined && Enum.IsDefined(source))
            _filters.Source = source;

        return this;
    }
}