using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Muxx.Lib.Helpers;
using Muxx.Lib.Services;
using Muxx.Lib.ValueObjects.Enums;
using Muxx.UI.Windows;

namespace PDV
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }

      private async void Admin_Click(object sender, RoutedEventArgs e)
      {
         await NewTransacExecute(PWOPER.PWOPER_ADMIN);
      }

      private async void Sale_Click(object sender, RoutedEventArgs e)
      {
         await NewTransacExecute(PWOPER.PWOPER_SALE);
      }

      private bool Cancelar()
      {
         return false;
      }

      private async Task NewTransacExecute(PWOPER pwOper)
      {
         Admin.IsEnabled = false;
         Sale.IsEnabled = false;

         Log.PrintThread("Iniciando...");

         TefWindow.Instance.TimeOut = null;
         TefWindow.Instance.BindMuxxLib();

         PGWebLib.DebugType = DebugType.Json;
         //PGWebLib.DebugCallback = Log.PrintThread;

         Fluxos.CancelarOperacaoFunc = Cancelar;
         Fluxos.Clear();

         Fluxos.ParamsAdd(PWINFO.PWINFO_AUTNAME, "PDV");
         Fluxos.ParamsAdd(PWINFO.PWINFO_AUTVER, "1.0.0.0");
         Fluxos.ParamsAdd(PWINFO.PWINFO_AUTDEV, "PayGo");
         Fluxos.ParamsAdd(PWINFO.PWINFO_AUTCAP, (
            (int)PWINFO_AUTCAP.PWINFO_AUTCAP_DSP_CHECKOUT +
            (int)PWINFO_AUTCAP.PWINFO_AUTCAP_DSP_QRCODE
            ).ToString());
         //QRCode
         Fluxos.ParamsAdd(PWINFO.PWINFO_DSPQRPREF,
            ((int)PWINFO_DSPQRPREF.PWINFO_DSPQRPREF_EXIBE_CHECKOUT).ToString());

         Log.PrintThread(
            string.Format("Operação: [{0}]", pwOper.ToString()));

         bool status = await Fluxos.FluxoInitAsync();
         if (!status)
         {
            Log.PrintThread("Não foi possível inicializar a biblioteca");
            return;
         }

         PWCNF pwCnf;
         status = await Fluxos.FluxoPrincipalAsync(pwOper);
         if (status)
         {
            Log.PrintThread("Transação: realizada com sucesso");
            pwCnf = PWCNF.PWCNF_CNF_AUTO;
         }
         else
         {
            Log.PrintThread("Transação: Não foi possível concluir sua transação");
            pwCnf = PWCNF.PWCNF_REV_MANU_AUT;
         }

         if (Fluxos.PossuiPendencia())
         {
            Log.PrintThread("Existe alguma transação pendente de confirmação no PayGoWeb...");

            //Nesse exemplo estou confirmando, mas o correto é verificar o status 
            //dessa transação na sua automação, para confirmar ou desfazer a mesma.
            if (Fluxos.FluxoConfirmacaoPendencia(PWCNF.PWCNF_CNF_AUTO))
            {
               Log.PrintThread("Confirmada!!!");
            }
            else
            {
               Log.PrintThread("Não Confirmada!!!");
            }
         }

         TefWindow.Instance.BindDisplayAguarde();

         Log.PrintThread("Resultados:");

         await Fluxos.FluxoGetResultPwInfosAsync();
         foreach (var info in Fluxos.ResultsEnviadosComSucesso)
         {
            Log.PrintThread(info.ToString());
         }

         if (Fluxos.RequerConfirmacao())
         {
            Log.PrintThread("Confirmando a transação...");

            if (Fluxos.FluxoConfirmacao(pwCnf))
            {
               Log.PrintThread("Confirmada!!!");
            }
            else
            {
               Log.PrintThread("Não Confirmada!!!");
            }
         }

         Log.PrintThread("Operação Finalizada!");

         TefWindow.Instance.Hide();

         Admin.IsEnabled = true;
         Sale.IsEnabled = true;
      }

   }
}