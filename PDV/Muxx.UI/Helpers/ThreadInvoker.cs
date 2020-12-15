using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Muxx.UI.Helpers
{
   public class ThreadInvoker
   {
      #region Singleton

      private ThreadInvoker()
      {
         if (Application.Current == null)
            return;

         //You have to Init the Dispatcher in the UI thread!
         //Init once per application (if there is only one Dispatcher).
         _dispatcher = Application.Current.Dispatcher;
      }

      public static ThreadInvoker Instance
      {
         get
         {
            return Nested.instance;
         }
      }

      class Nested
      {
         // Explicit static constructor to tell C# compiler
         // not to mark type as beforefieldinit
         static Nested()
         {
         }

         internal static readonly ThreadInvoker instance = new ThreadInvoker();
      }

      #endregion

      #region UI Thread

      private Dispatcher _dispatcher = null;

      public void RunByUiThread(Action action)
      {
         #region UI Thread Safety

         //handle by UI Thread.
         if (_dispatcher != null)
         {
            _dispatcher.Invoke(action, DispatcherPriority.Normal);
            return;
         }

         action();

         #endregion
      }

      public T RunByUiThread<T>(Func<T> function)
      {
         #region UI Thread Safety

         //handle by UI Thread.
         if (_dispatcher != null)
         {
            return (T)_dispatcher.Invoke(DispatcherPriority.Normal, function);
         }

         return function();

         #endregion
      }

      #endregion
   }
}