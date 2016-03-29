namespace Ecommerce.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}