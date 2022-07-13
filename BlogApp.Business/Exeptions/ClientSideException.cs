using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Exeptions
{
    public class ClientSideException : Exception
    {
        public ClientSideException(string message) : base(message)
        {

        }
    }
}
