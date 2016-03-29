using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ecommerce.Data.Infrastructure;

namespace Ecommerce.Service.Infrastructure
{
    public abstract class RepositoryService<T> where T : class
    {
        protected IRepository<T> GenericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RepositoryService(IRepository<T> genericRepository, IUnitOfWork unitOfWork)
        {
            this.GenericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
        }

        public RepositoryService()
        {
            this._unitOfWork = new UnitOfWork(new DatabaseFactory());
        }

        #region IRepositoryService<T> Members

        public IEnumerable<T> GetAll()
        {
            return GenericRepository.GetAll();
        }

        public T GetById(int id)
        {
            return GenericRepository.GetById(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return GenericRepository.GetMany(where);
        }

        public void Update(T Entity)
        {
            // throw new NotImplementedException();

            GenericRepository.Update(Entity);
            Save();
        }

        public void Insert(T Entity)
        {
            GenericRepository.Add(Entity);
            Save();
        }

        public void Delete(int id)
        {
            var student = GenericRepository.GetById(id);
            GenericRepository.Delete(student);
            Save();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        #endregion
    }
}