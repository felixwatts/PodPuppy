using System;
using System.Collections.Generic;
using System.Text;

namespace PodPuppy
{
    class FeedException : Exception
    {
        public FeedException(string message) : base(message) { }
    }
}
