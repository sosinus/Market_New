using Models;
using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnitsOfWork;

namespace Service
{
    public interface ICustomerService
    {
        Customer GetCustomer(string userId);

        CustomerResult CreateCustomer(FrontCustomer frontCustomer, string userId);

        int GetDiscount(string userId);
    }
   
}
