using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoShop.Shared.Exceptions
{
    public class InvalidLinkException : Exception
    {
        public InvalidLinkException(string message) : base(message)
        {
        }
    }
}
