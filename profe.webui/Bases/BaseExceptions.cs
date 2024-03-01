using System;
namespace profe.webui.Bases
{
    public class BaseExceptions : ApplicationException
    {
        public BaseExceptions() { }

        public BaseExceptions(string message) : base(message) { }

    }
}

