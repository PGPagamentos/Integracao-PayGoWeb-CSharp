using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muxx.Lib.Services;
using Muxx.Lib.ValueObjects.Enums;

namespace Muxx.Lib.Entities
{
   public class Info
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

      public virtual string ValueFormatado
      {
         get
         {
            if (_value == null)
               return "";

            string[] values =
               _value
               .Split(new string[] { " ", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            return
               values.Length == 0 ?
                  "" :
                  values
                  .Aggregate((current, next) => current + " " + next)
                  .Trim();
         }
      }

      public short? Ret
      {
         get { return _ret; }
         set { _ret = value; }
      }
      #endregion

      #region Constructors

      public Info() { }

      public Info(PWINFO pwInfo)
      {
         _param = pwInfo;
      }

      public Info(ushort pwInfo, string value)
      {
         //TODO: Testar se vier algo não previsto no Enum...
         Enum.TryParse(pwInfo.ToString(), out _param);
         _value = value;
      }

      public Info(PWINFO pwInfo, string value)
         : this(pwInfo)
      {
         _value = value;
      }

      public Info(PWINFO pwInfo, string value, short ret)
         : this(pwInfo, value)
      {
         _ret = ret;
      }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         switch (PwInfo)
         {
            case PWINFO.PWINFO_RCPTFULL:
            case PWINFO.PWINFO_RCPTMERCH:
            case PWINFO.PWINFO_RCPTCHOLDER:
            case PWINFO.PWINFO_RCPTCHSHORT:
               return
                  string.Format("{0}:{1}{1}{2}",
                     PwInfo,
                     Environment.NewLine,
                     Fluxos.FormatarComprovante(Value)
                     );
            default:
               return
                  string.Format("{0}: [{1}]", PwInfo, ValueFormatado);
         }
      }

      #endregion

      #region Public Static Methods

      public static Info New(ushort pwInfo, string value)
      {
         return new Info(pwInfo, value);
      }

      public static Info New(PWINFO pwInfo, string value)
      {
         return new Info(pwInfo, value);
      }

      public static Info New(PWINFO pwInfo, string value, short ret)
      {
         return new Info(pwInfo, value, ret);
      }

      #endregion

   }
}