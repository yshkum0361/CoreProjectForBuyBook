﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
       ICategoryRepository CategoryRepository { get; }  
        ICoverTypeRepository CoverTypeRepository { get; }

        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get; }

        IAppUserRepository AppUserRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }

        void Save();
    }
}
