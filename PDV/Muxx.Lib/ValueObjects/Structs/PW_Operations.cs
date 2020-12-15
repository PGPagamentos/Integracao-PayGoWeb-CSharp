using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Structs
{
   [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
   public struct PW_Operations
   {
      byte bOperType;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
      string szText;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
      string szValue;
   }
}