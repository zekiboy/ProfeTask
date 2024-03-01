using System;
using profe.webui.Bases;
using profe.webui.Entities;
using profe.webui.Exceptions.ProductExceptions;

namespace profe.webui.Rules
{
    public class ProductRules : BaseRules
    {
        public Task ProductTitleMustNotBeSame(IList<Product> products, string requestName)
        {
            if (products.Any(x => x.productName == requestName)) throw new ProductTitleMustNotBeSameException();

            return Task.CompletedTask;
        }

    }
}

