using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arukone.Logic.Exceptions
{
    internal class BoardFilledException : Exception
    {
        public BoardFilledException(string? message = "All fields on the board are already filled.") : base(message) { }
    }
}
