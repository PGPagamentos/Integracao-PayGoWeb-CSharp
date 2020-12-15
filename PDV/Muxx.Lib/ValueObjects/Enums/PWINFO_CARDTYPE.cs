using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   public enum PWINFO_CARDTYPE : ushort
   {
      PWINFO_CARDTYPE_CREDITO = 1,
      PWINFO_CARDTYPE_DEBITO = 2,
      PWINFO_CARDTYPE_VOUCHER_PAT = 4,
      PWINFO_CARDTYPE_OUTROS = 8
   }
}