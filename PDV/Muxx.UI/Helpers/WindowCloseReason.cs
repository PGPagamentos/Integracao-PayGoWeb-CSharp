using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace Muxx.UI.Helpers
{
   public enum CloseReason
   {
      None,
      WindowsShutDown,
      UserClosing
   }

   public class WindowCloseReason
   {
      #region Public Properties

      public CloseReason CloseReason
      {
         get;
         set;
      }

      #endregion

      #region Constructors

      public WindowCloseReason(Window window)
      {
         HwndSource source = (HwndSource)PresentationSource.FromDependencyObject(window);
         source.AddHook(RetornaCloseReason);

         CloseReason = CloseReason.None;
      }

      #endregion

      #region Private Methods

      private IntPtr RetornaCloseReason(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
      {
         if (CloseReason == CloseReason.None)
         {
            switch (msg)
            {
               case 0x16:

                  CloseReason = CloseReason.WindowsShutDown;
                  break;

               case 0x112:

                  if ((LOWORD((int)wParam) & 0XFFF0) == 0XF060)
                     CloseReason = CloseReason.UserClosing;
                  break;
            }
         }

         return IntPtr.Zero;
      }

      private static int LOWORD(int n)
      {
         return (n & 0XFFFF);
      }

      #endregion
   }
}