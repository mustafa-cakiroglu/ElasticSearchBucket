using System;
using System.Collections.Generic;
using System.Text;

namespace HyphenProject.Business.Helper
{
    public class InvalidArgumentException : ApplicationException
    {
        public InvalidArgumentException() : base("Invalid Argument")
        {
        }

        public InvalidArgumentException(string message) : base(message)
        {
        }
    }
}