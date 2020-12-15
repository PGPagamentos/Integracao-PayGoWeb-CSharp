using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Muxx.Lib.Helpers
{
   public static class Log
   {
      public static void PrintThread(string mensagem)
      {
         Debug.Print(string.Format("[Thread {0,2}][{1:HH:mm:ss.fff}] {2}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now, mensagem));
      }

      public static void PrintThread(params string[] mensagens)
      {
         mensagens = mensagens.SelectMany(m => m.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)).ToArray();
         PrintThread(
            string.Join(
            Environment.NewLine,
            mensagens.Select(msg => string.Format("{0}", msg))));
      }

   }
}