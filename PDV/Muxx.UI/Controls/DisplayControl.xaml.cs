using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Muxx.UI.Controls
{
   /// <summary>
   /// Interaction logic for SplashWindow.xaml
   /// </summary>
   public partial class DisplayControl : UserControl
   {
      #region Const
      private const int LINHAS = 4;
      private const int COLUNAS = 20;
      #endregion

      #region Member Variables

      #endregion

      #region Public Properties

      public string Mensagem
      {
         get { return txbMensagem.Text; }
      }

      #endregion

      #region Constructors

      public DisplayControl()
      {
         InitializeComponent();
      }

      #endregion

      #region Public Methods

      public void ShowProgressBar()
      {
         pgbBar.Visibility = Visibility.Visible;
      }

      public void HideProgressBar()
      {
         pgbBar.Visibility = Visibility.Collapsed;
      }

      //public void Bind(string mensagem)
      //{
      //   Bind(mensagem, true);
      //}

      public void Bind(string mensagem, bool centralizar, bool? hideProgressBar)
      {
         List<string> linhas = mensagem.Split('\r').ToList<string>();
         //Se fixar sempre em 4 Linhas os Menus de seleção e pra digitar são exibidos de maneira estranha,
         //ou seja, o titulo fica em cima e a informação bem abaixo...
         //bool centralizar = linhas.Any(linha => linha.StartsWith(" ") | string.IsNullOrEmpty(linha));
         if (centralizar)
         {
            while (linhas.Count < LINHAS)
               linhas.Add("");
         }
         mensagem = string.Join("\r", linhas.Select(l => l.PadRight(COLUNAS, ' ')));

         txbMensagem.Text = mensagem;

         if (hideProgressBar.HasValue)
         {
            if (hideProgressBar.Value)
               HideProgressBar();
            else
               ShowProgressBar();
         }
      }

      #endregion

   }
}