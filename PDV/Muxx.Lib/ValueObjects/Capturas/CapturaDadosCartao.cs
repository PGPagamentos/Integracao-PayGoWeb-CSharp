using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Capturas
{
   public class CapturaDadosCartao : Captura
   {
      #region Member Variables
      private string _cardFullPan;
      private string _cardExpDate;
      #endregion

      #region Public Properties

      public string CardFullPan
      {
         get { return _cardFullPan; }
         set { _cardFullPan = value; }
      }

      public string CardExpDate
      {
         get { return _cardExpDate; }
         set { _cardExpDate = value; }
      }

      #endregion
   }
}