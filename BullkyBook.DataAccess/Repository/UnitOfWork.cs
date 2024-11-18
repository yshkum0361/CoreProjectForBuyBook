using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BullkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private AppDbContext _db;
        public UnitOfWork(AppDbContext db) 
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            CoverTypeRepository = new CoverTypeRepository(_db);
            ProductRepository = new ProductRepository(_db);
            CompanyRepository = new CompanyRepository(_db);
            ShoppingCartRepository = new ShoppingCartRepository(_db);
            AppUserRepository = new AppUserRepository(_db);
            OrderHeaderRepository = new OrderHeaderRepository(_db);
            OrderDetailsRepository = new OrderDetailsRepository(_db);
        }
        public ICategoryRepository CategoryRepository { get; private set; }

        public ICoverTypeRepository CoverTypeRepository { get; private set; }

        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public  IAppUserRepository AppUserRepository { get; private set; }
        public  IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        public  IOrderHeaderRepository OrderHeaderRepository { get; private set; }

       

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
