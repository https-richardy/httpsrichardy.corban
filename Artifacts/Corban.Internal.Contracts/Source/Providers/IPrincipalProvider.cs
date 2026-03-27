namespace Corban.Internal.Contracts.Providers;

public interface IPrincipalProvider
{
    public User GetCurrentPrincipal();

    public void SetPrincipal(User user);
    public void Clear();
}
