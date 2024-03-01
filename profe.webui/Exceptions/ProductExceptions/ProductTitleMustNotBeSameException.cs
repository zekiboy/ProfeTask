using System;
using profe.webui.Bases;

namespace profe.webui.Exceptions.ProductExceptions
{
    public class ProductTitleMustNotBeSameException : BaseExceptions
    {

        public ProductTitleMustNotBeSameException() : base("Ürün başlığı zaten var!")
        {

        }
    }
}

