namespace Ecommerce.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private EcommerceContext _dataContext;

        public EcommerceContext Get()
        {
            return _dataContext ?? (_dataContext = new EcommerceContext());
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}