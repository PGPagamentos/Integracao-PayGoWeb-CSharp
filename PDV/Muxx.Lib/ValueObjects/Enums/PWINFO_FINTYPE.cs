using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   public enum PWINFO_FINTYPE : ushort
   {
      PWINFO_FINTYPE_VISTA = 1,
      PWINFO_FINTYPE_PARCELADO_EMISSOR = 2,
      PWINFO_FINTYPE_PARCELADO_ESTABELECIMENTO = 4,
      PWINFO_FINTYPE_PRE_DATADO = 8
   }
}