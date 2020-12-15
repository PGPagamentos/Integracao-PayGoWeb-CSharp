using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Muxx.Lib.Helpers;
using Muxx.Lib.Services;
using Muxx.Lib.ValueObjects.Capturas;
using Muxx.Lib.ValueObjects.Enums;
using Muxx.Lib.ValueObjects.Structs;
using Muxx.UI.Controls;
using Muxx.UI.Controls.Extend;
using Muxx.UI.Helpers;

namespace Muxx.UI.Windows
{
   /// <summary>
   /// Interaction logic for TefWindow.xaml
   /// </summary>
   public partial class TefWindow : Window
   {

      #region Enum

      public enum State : byte
      {
         Created,
         Showing,
         Hidding
      }

      #endregion

      #region Singleton

      private static TefWindow _instance;

      public static TefWindow Instance
      {
         get
         {
            return
               ThreadInvoker.Instance.RunByUiThread<TefWindow>(() =>
               {
                  return _instance == null ? _instance = new TefWindow() : _instance;
               });
         }
      }

      #endregion

      #region Member Variables

      private bool _forceClose = false;
      private bool _emCaptura = false;
      private int? _timeOut = null;

      private string _senhaLojista = "1111";
      private string _senhaTecnica = "314159";

      private State _state;
      private CapturaControl _capturaControl;
      private WindowCloseReason _windowCloseReason;
      private DispatcherTimer _dispatcherTimerTimeOut = new DispatcherTimer();

      #endregion

      #region Public Properties

      /// <summary>
      /// TimeOut para a captura do dado, em milissegundos.
      /// </summary>
      public int? TimeOut
      {
         get { return _timeOut; }
         set { _timeOut = value; }
      }

      /// <summary>
      /// Senha do lojista.
      /// </summary>
      public string SenhaLojista
      {
         get { return _senhaLojista; }
         set { _senhaLojista = value; }
      }

      /// <summary>
      /// Senha técnica.
      /// </summary>
      public string SenhaTecnica
      {
         get { return _senhaTecnica; }
         set { _senhaTecnica = value; }
      }

      #endregion

      #region Constructors

      public TefWindow()
      {
         InitializeComponent();
         BindMuxxLib();
         Topmost = true;
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Faz o bind dos delegates da classe <see cref="Fluxos"/>, 
      /// para a exibição do display e captura dos dados.
      /// </summary>
      public void BindMuxxLib()
      {
         Fluxos.DisplayCallback = BindDisplay;
         Fluxos.CapturaMenuFunc = BindCapturaMenu;
         Fluxos.CapturaDigitadaFunc = BindCapturaDigitada;
         Fluxos.CapturaSenhaFunc = BindCapturaSenha;
         Fluxos.CapturaQRCodeFunc = BindCapturaQRCode;
      }

      /// <summary>
      /// Exibe o display com a mensagem "AGUARDE..."
      /// </summary>
      public void BindDisplayAguarde()
      {
         BindDisplay("\r     AGUARDE...     \r", true);
      }

      /// <summary>
      /// Exibe o display
      /// </summary>
      /// <param name="mensagem"></param>
      /// <param name="padrao"></param>
      public void BindDisplay(string mensagem, bool padrao)
      {
         Log.PrintThread("Fluxos_DisplayCallback");

         ThreadInvoker.Instance.RunByUiThread(() =>
         {
            if (_emCaptura)
            {
               CapturaDigitadaControl capturaDigitadaControl = cttPrincipal.Content as CapturaDigitadaControl;
               if (capturaDigitadaControl != null)
               {
                  capturaDigitadaControl.Display(mensagem, !_emCaptura);
               }
               return;
            }

            DisplayControl displayControl = new DisplayControl();
            cttPrincipal.Content = displayControl;
            displayControl.Bind(mensagem, true, !padrao);
            if (_forceClose)
            {
               CancelCaptura(true);
               return;
            }
            Show();
         });
      }

      /// <summary>
      /// Exibe e captura o item de menu selecionado
      /// </summary>
      /// <param name="getData"></param>
      /// <returns></returns>
      public Captura BindCapturaMenu(PW_GetData getData)
      {
         Log.PrintThread("Fluxos_CapturaMenuFunc");

         return
            ThreadInvoker.Instance.RunByUiThread<Captura>(() =>
            {
               return BindCaptura(getData, new CapturaMenuControl());
            });
      }

      /// <summary>
      /// Exibe e captura o dado digitado
      /// </summary>
      /// <param name="getData"></param>
      /// <returns></returns>
      public Captura BindCapturaDigitada(PW_GetData getData)
      {
         Log.PrintThread("Fluxos_CapturaDigitadaFunc");

         return
            ThreadInvoker.Instance.RunByUiThread<Captura>(() =>
            {
               return BindCaptura(getData, new CapturaDigitadaControl());
            });
      }

      /// <summary>
      /// Exibe e captura a senha
      /// </summary>
      /// <param name="getData"></param>
      /// <returns></returns>
      public Captura BindCapturaSenha(PW_GetData getData)
      {
         Log.PrintThread("Fluxos_CapturaSenhaFunc");

         string senha = null;
         switch (getData.wIdentificador)
         {
            case (byte)PWINFO.PWINFO_AUTHMNGTUSER:
               getData.szPrompt = "SENHA LOJISTA:";
               senha = _senhaLojista;
               break;
            case (byte)PWINFO.PWINFO_AUTHTECHUSER:
               getData.szPrompt = "SENHA TECNICA:";
               senha = _senhaTecnica;
               break;
            default:
               break;
         }

         getData.bOcultarDadosDigitados = 1; //Mascarar o dado
         getData.bTamanhoMinimo = 0;
         getData.bTamanhoMaximo = 6;

         return BindCapturaSenha(getData, senha);
      }

      /// <summary>
      /// Exibe e captura a senha
      /// </summary>
      /// <param name="getData"></param>
      /// <param name="senha"></param>
      /// <returns></returns>
      public Captura BindCapturaSenha(PW_GetData getData, string senha)
      {
         return
            ThreadInvoker.Instance.RunByUiThread<Captura>(() =>
            {
               return BindCaptura(getData, new CapturaDigitadaControl() { Senha = senha });
            });
      }

      /// <summary>
      /// Exibe e captura o QR Code
      /// </summary>
      /// <param name="getData"></param>
      /// <param name="qrCode"></param>
      /// <returns></returns>
      private Captura BindCapturaQRCode(PW_GetData getData, string qrCode)
      {
         Log.PrintThread("Fluxos_CapturaQRCodeFunc");

         return
            ThreadInvoker.Instance.RunByUiThread<Captura>(() =>
            {
               bool atualiza = true;
               CapturaQRCodeControl capturaQRCodeControl = _capturaControl as CapturaQRCodeControl;
               if (capturaQRCodeControl == null)
               {
                  atualiza = false;
                  capturaQRCodeControl = new CapturaQRCodeControl();
               }
               capturaQRCodeControl.QRCode = qrCode;

               CapturaControl capturaControl = capturaQRCodeControl;
               _capturaControl = capturaControl;
               cttPrincipal.Content = capturaControl;
               capturaControl.Bind(getData);
               //capturaControl.OnParamCapturado += (Captura captura) =>
               //{
               //   DialogResult = true;
               //};
               if (!atualiza)
               {
                  Hide();
                  Show();
               }
               return capturaControl.Captura;
            });
      }

      /// <summary>
      /// Cancela a captura
      /// </summary>
      /// <param name="userCanceling"></param>
      public void CancelCaptura(bool userCanceling)
      {
         ThreadInvoker.Instance.RunByUiThread(() =>
         {
            if (userCanceling)
               Fluxos.CancelarOperacaoFunc = () => { return true; };

            if (_capturaControl != null)
               _capturaControl.Captura.CapturaCancelada = true;

            Hide();
         });
      }

      /// <summary>
      /// Mostra a janela
      /// </summary>
      public new void Show()
      {
         Log.PrintThread("Show()");

         _state = State.Showing;
         ThreadInvoker.Instance.RunByUiThread(() =>
         {
            base.Show();
         });
      }

      /// <summary>
      /// Mostra a janela para capturar um dado
      /// </summary>
      public new void ShowDialog()
      {
         Log.PrintThread("ShowDialog()");

         if (_state == State.Showing)
            Hide();

         _state = State.Showing;
         ThreadInvoker.Instance.RunByUiThread(() =>
         {
            base.ShowDialog();
         });
      }

      /// <summary>
      /// Esconde a janela
      /// </summary>
      public new void Hide()
      {
         Log.PrintThread("Hide()");

         _state = State.Hidding;
         ThreadInvoker.Instance.RunByUiThread(() =>
         {
            base.Hide();
         });
      }

      /// <summary>
      /// Fecha a janela
      /// </summary>
      public void ForceClose()
      {
         Log.PrintThread("ForceClose()");

         _instance = null;
         _forceClose = true;

         base.Close();
      }

      #endregion

      #region Private Methods

      /// <summary>
      /// Exibe e captura o controle
      /// </summary>
      /// <param name="getData"></param>
      /// <param name="capturaControl"></param>
      /// <returns></returns>
      private Captura BindCaptura(PW_GetData getData, CapturaControl capturaControl)
      {
         _capturaControl = capturaControl;
         cttPrincipal.Content = capturaControl;
         capturaControl.Bind(getData);
         capturaControl.ParamCapturadoCallback = (Captura captura) =>
         {
            DialogResult = true;
         };
         Hide();
         ShowDialogCaptura();
         return capturaControl.Captura;
      }

      private void ShowDialogCaptura()
      {
         if (_timeOut.HasValue)
         {
            if (_timeOut.Value == 0)
            {
               CancelCaptura(true);
               return;
            }

            _dispatcherTimerTimeOut.Interval = TimeSpan.FromMilliseconds(_timeOut.Value);
            _dispatcherTimerTimeOut.Tick += (sender, e) =>
            {
               CancelCaptura(true);
            };
            _dispatcherTimerTimeOut.Start();
         }

         _emCaptura = true;
         ShowDialog();
         _emCaptura = false;

         if (_timeOut.HasValue)
         {
            _dispatcherTimerTimeOut.Stop();
         }
      }

      /// <summary>
      /// Permite mover a janela
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Window_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
      }

      /// <summary>
      /// Verifica se o ESC foi pressionado, para cancelar a captura.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         Fluxos.FluxoPinPadKeyDown(() => SomenteTeclaNumerica(e.Key));

         if (e.Key == Key.Escape)
            CancelCaptura(true);
      }

      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         _windowCloseReason = new WindowCloseReason(this);
      }

      /// <summary>
      /// Se o usuário fechar a tela, cancela a captura
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         Hide();

         if ((_windowCloseReason != null &&
              _windowCloseReason.CloseReason == CloseReason.UserClosing) ||
             _forceClose)
            CancelCaptura(true);

         if (!_forceClose)
            e.Cancel = true;
      }

      private void Window_Closed(object sender, EventArgs e)
      {

      }

      private bool SomenteTeclaNumerica(Key key)
      {
         return ((key >= Key.D0 && key <= Key.D9) ||
                 (key >= Key.NumPad0 && key <= Key.NumPad9));
      }

      #endregion

      #region Custom Input

      public PW_GetData MenuSimNao(string szPrompt)
      {
         return new PW_GetData()
         {
            wIdentificador = (ushort)PWINFO.PWINFO_MENU1,
            bTipoDeDado = (byte)PWDAT.PWDAT_MENU,
            szPrompt = szPrompt,
            bNumOpcoesMenu = 2,
            vszTextoMenu = new TextoMenu[]
            {
               new TextoMenu() { szTextoMenu = "SIM" },
               new TextoMenu() { szTextoMenu = "NAO" },
            },
            vszValorMenu = new ValorMenu[]
            {
               new ValorMenu() { szValorMenu = "1" },
               new ValorMenu() { szValorMenu = "2" },
            },
            szMascaraDeCaptura = "",
            bTiposEntradaPermitidos = 0,
            bTamanhoMinimo = 0,
            bTamanhoMaximo = 0,
            ulValorMinimo = 0,
            ulValorMaximo = 0,
            bOcultarDadosDigitados = 0,
            bValidacaoDado = 0,
            bAceitaNulo = 0,
            szValorInicial = "",
            bTeclasDeAtalho = 1,
            szMsgValidacao = "",
            szMsgConfirmacao = "",
            szMsgDadoMaior = "",
            szMsgDadoMenor = "",
            bCapturarDataVencCartao = 0,
            ulTipoEntradaCartao = 0,
            bItemInicial = 0,
            bNumeroCapturas = 1,
            szMsgPrevia = "",
            bTipoEntradaCodigoBarras = 0,
            bOmiteMsgAlerta = 0,
            bStartFromLeft = 0,
            bNotificarCancelamento = 0
         };
      }

      #endregion

      #region Templates

      public PW_GetData MenuTemplate()
      {
         return new PW_GetData()
         {
            wIdentificador = (ushort)PWINFO.PWINFO_VIRTMERCH,
            bTipoDeDado = (byte)PWDAT.PWDAT_MENU,
            szPrompt = "",
            bNumOpcoesMenu = 8,
            vszTextoMenu = new TextoMenu[]
            {
               new TextoMenu() { szTextoMenu = "CIELO" },
               new TextoMenu() { szTextoMenu = "CIELO" },
               new TextoMenu() { szTextoMenu = "REDE" },
               new TextoMenu() { szTextoMenu = "REDE" },
               new TextoMenu() { szTextoMenu = "BIN" },
               new TextoMenu() { szTextoMenu = "CIELO" },
               new TextoMenu() { szTextoMenu = "REDE" },
               new TextoMenu() { szTextoMenu = "STONE" },
            },
            vszValorMenu = new ValorMenu[]
            {
               new ValorMenu() { szValorMenu = "10" },
               new ValorMenu() { szValorMenu = "14" },
               new ValorMenu() { szValorMenu = "22" },
               new ValorMenu() { szValorMenu = "23" },
               new ValorMenu() { szValorMenu = "36" },
               new ValorMenu() { szValorMenu = "37" },
               new ValorMenu() { szValorMenu = "39" },
               new ValorMenu() { szValorMenu = "42" },
            },
            szMascaraDeCaptura = "",
            bTiposEntradaPermitidos = 0,
            bTamanhoMinimo = 0,
            bTamanhoMaximo = 0,
            ulValorMinimo = 0,
            ulValorMaximo = 0,
            bOcultarDadosDigitados = 0,
            bValidacaoDado = 0,
            bAceitaNulo = 0,
            szValorInicial = "",
            bTeclasDeAtalho = 1,
            szMsgValidacao = "",
            szMsgConfirmacao = "",
            szMsgDadoMaior = "",
            szMsgDadoMenor = "",
            bCapturarDataVencCartao = 0,
            ulTipoEntradaCartao = 0,
            bItemInicial = 0,
            bNumeroCapturas = 1,
            szMsgPrevia = "",
            bTipoEntradaCodigoBarras = 0,
            bOmiteMsgAlerta = 0,
            bStartFromLeft = 0,
            bNotificarCancelamento = 0
         };
      }

      public PW_GetData TipoValorTemplate()
      {
         return new PW_GetData()
         {
            wIdentificador = (ushort)PWINFO.PWINFO_TOTAMNT,
            bTipoDeDado = (byte)PWDAT.PWDAT_TYPED,
            szPrompt = "VALOR",
            bNumOpcoesMenu = 0,
            vszTextoMenu = null,
            vszValorMenu = null,
            szMascaraDeCaptura = "@.@@@.@@@.@@@,@@",
            bTiposEntradaPermitidos = 1,
            bTamanhoMinimo = 1,
            bTamanhoMaximo = 16,
            ulValorMinimo = 1,
            ulValorMaximo = -1,
            bOcultarDadosDigitados = 0,
            bValidacaoDado = 1,
            bAceitaNulo = 0,
            szValorInicial = "",
            bTeclasDeAtalho = 0,
            szMsgValidacao = "\r VALOR NAO PODE\r SER VAZIO",
            szMsgConfirmacao = "CONFIRME O DADO:",
            szMsgDadoMaior = "\r VALOR ACIMA DO\r LIMITE",
            szMsgDadoMenor = "\r VALOR ABAIXO DO\r LIMITE",
            bCapturarDataVencCartao = 0,
            ulTipoEntradaCartao = 0,
            bItemInicial = 0,
            bNumeroCapturas = 1,
            szMsgPrevia = "",
            bTipoEntradaCodigoBarras = 0,
            bOmiteMsgAlerta = 0,
            bStartFromLeft = 0,
            bNotificarCancelamento = 0
         };
      }

      #endregion

   }
}