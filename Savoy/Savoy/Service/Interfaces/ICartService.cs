namespace Savoy.Service.Interfaces
{
    public interface ICartService 
    {
        Task<int> GetCartProductBasket(string Id);
    }
}
