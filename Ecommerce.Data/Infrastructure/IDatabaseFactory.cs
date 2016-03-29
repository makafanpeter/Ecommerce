using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecommerce.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        EcommerceContext Get();
    }
}
