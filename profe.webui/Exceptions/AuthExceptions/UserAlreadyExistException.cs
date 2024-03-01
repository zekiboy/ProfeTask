using System;
using profe.webui.Bases;

namespace profe.webui.Exceptions.AuthExceptions
{
    public class UserAlreadyExistException : BaseExceptions
    {
        public UserAlreadyExistException() : base("Böyle bir kullanıcı zaten var!") { }
    }
}

