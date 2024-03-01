using System;
using profe.webui.Bases;

namespace profe.webui.Exceptions.AuthExceptions
{
    public class EmailAddressShouldBeValidException : BaseExceptions
    {
        public EmailAddressShouldBeValidException() : base("Böyle bir email adresi bulunmamaktadır.") { }
    }
}

