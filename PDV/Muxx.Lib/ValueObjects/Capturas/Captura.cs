using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Capturas
{
   public class Captura
   {
      #region Member Variables
      private bool _capturaCancelada = false;
      private string _valor;
      #endregion

      #region Public Properties

      public bool CapturaCancelada
      {
         get { return _capturaCancelada; }
         set { _capturaCancelada = value; }
      }

      public string Valor
      {
         get { return _valor; }
         set { _valor = value; }
      }

      #endregion

   }
}