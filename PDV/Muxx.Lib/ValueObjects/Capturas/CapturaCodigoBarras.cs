using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Capturas
{
   public class CapturaCodigoBarras : Captura
   {
      #region Member Variables
      private string _barCode;
      private string _barCodEntMode;
      #endregion

      #region Public Properties

      public string BarCode
      {
         get { return _barCode; }
         set { _barCode = value; }
      }

      public string BarCodEntMode
      {
         get { return _barCodEntMode; }
         set { _barCodEntMode = value; }
      }

      #endregion
   }
}