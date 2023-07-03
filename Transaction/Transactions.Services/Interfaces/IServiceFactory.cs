namespace Transactions.Services.Interfaces
{
    public interface IServiceFactory
    {
        T GetService<T>() where T : class;
    }
}
