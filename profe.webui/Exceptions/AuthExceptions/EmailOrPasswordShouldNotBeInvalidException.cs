using System;
using profe.webui.Bases;

namespace profe.webui.Exceptions.AuthExceptions
{
    public class EmailOrPasswordShouldNotBeInvalidException : BaseExceptions
    {
        public EmailOrPasswordShouldNotBeInvalidException() : base("Kullanıcı adı veya şifre yanlış.") { }
    }

}

