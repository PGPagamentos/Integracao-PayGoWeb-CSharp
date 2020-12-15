using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.Exceptions
{
   public class EventNotDefinedExceptions : Exception
   {
      public EventNotDefinedExceptions(string message)
         : base(message)
      {
      }
   }
}