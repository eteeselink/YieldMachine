using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieldMachine
{
    class InvalidTriggerException : Exception
    {
        public InvalidTriggerException() { }
        public InvalidTriggerException(string msg) : base(msg) { }
    }
}
