using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muxx.Lib.ValueObjects.Enums;

namespace Muxx.Lib.Entities
{
   public class Param
   {
      #region Member Variables
      private PWINFO _param;
      private string _value;
      private short? _ret;
      #endregion

      #region Public Properties
      public virtual PWINFO PwInfo
      {
         get { return _param; }
         set { _param = value; }
      }

      public virtual string Value
      {
         get { return _value; }
         set { _value = value; }
      }

      public short? Ret
      {
         get { return _ret; }
         set { _ret = value; }
      }
      #endregion

      #region Constructors

      public Param() { }

      public Param(ushort pwInfo, string value)
      {
         //TODO: Testar se vier algo não previsto no Enum...
         Enum.TryParse(pwInfo.ToString(), out _param);
         _value = value;
      }

      public Param(PWINFO pwInfo, string value)
      {
         _param = pwInfo;
         _value = value;
      }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return string.Format("{0} = {1} | RET = {2}", _param.ToString(), _value, _ret);
      }

      #endregion

      #region Public Static Methods

      public static Param New(ushort pwInfo, string value)
      {
         return new Param(pwInfo, value);
      }

      public static Param New(PWINFO pwInfo, string value)
      {
         return new Param(pwInfo, value);
      }

      #endregion

   }
}