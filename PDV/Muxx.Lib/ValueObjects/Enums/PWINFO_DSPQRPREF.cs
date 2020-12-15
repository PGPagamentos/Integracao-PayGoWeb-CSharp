using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   /// <summary>
   /// Caso a exibição de QR Code seja suportada pela
   /// Automação Comercial e pelo PIN-Pad, indica a
   /// preferência do local de exibição:
   /// 1: exibe no PIN-Pad;
   /// 2: exibe no checkout;
   /// OBS: Caso esse campo não seja informado pela
   /// automação e o ponto de captura esteja configurado
   /// como autoatendimento, o QR Code é exibido no
   /// checkout.Caso contrário, é exibido no pinpad.
   /// </summary>
   public enum PWINFO_DSPQRPREF : ushort
   {
      PWINFO_DSPQRPREF_EXIBE_PINPAD = 1,
      PWINFO_DSPQRPREF_EXIBE_CHECKOUT = 2
   }
}