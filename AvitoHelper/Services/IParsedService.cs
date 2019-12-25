using AvitoHelper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Services
{
    interface IParsedService
    {
        public List<Product> Parse(Order order, int pages, bool isAnalize);
    }
}
