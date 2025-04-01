namespace Brewserve.Data.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IBarRepository Bars { get; }
        IBeerRepository Beers { get; }
        IBreweryRepository Breweries { get; }
        Task<int> SaveAsync();
    }
}
