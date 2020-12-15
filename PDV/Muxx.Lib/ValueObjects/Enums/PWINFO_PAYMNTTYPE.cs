using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   public enum PWINFO_PAYMNTTYPE : ushort
   {
      PWINFO_PAYMNTTYPE_CARTAO = 1,
      PWINFO_PAYMNTTYPE_DINHEIRO = 2,
      PWINFO_PAYMNTTYPE_CHEQUE = 4,
      PWINFO_PAYMNTTYPE_CARTEIRA_VIRTUAL = 8,
   }
}