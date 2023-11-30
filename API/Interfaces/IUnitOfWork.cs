namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }
        IOrdersRepository OrdersRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
