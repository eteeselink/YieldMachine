using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieldMachine
{
    public class InvalidStateMachineException : Exception
    {
        public InvalidStateMachineException() { }
        public InvalidStateMachineException(string msg) : base(msg) { }
    }
}
