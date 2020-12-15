using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using System.Windows.Threading;
using Muxx.Lib.Helpers;
using Muxx.Lib.ValueObjects.Enums;
using Muxx.Lib.ValueObjects.Structs;
using Muxx.UI.Controls;
using Muxx.UI.Controls.Extend;

namespace Muxx.UI.Controls
{
   /// <summary>
   /// Interaction logic for CapturaDigitadaControl.xaml
   /// </summary>
   public partial class CapturaDigitadaControl : CapturaControl
   {
      #region Member Variables

      private string _senha;
      private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

      #endregion

      #region Public Properties

      public string Senha
      {
         set { _senha = value; }
      }

      #endregion

      #region Constructors

      public CapturaDigitadaControl()
      {
         InitializeComponent();
      }

      #endregion

      #region Public Methods

      public void ShowDisplay()
      {
         dspDisplay.Visibility = Visibility.Visible;
      }

      public void HideDisplay()
      {
         dspDisplay.Visibility = Visibility.Collapsed;
      }

      public void HideCaptura()
      {
         stkInput.Visibility = Visibility.Hidden;
      }

      public void ShowCaptura()
      {
         stkInput.Visibility = Visibility.Visible;
      }

      public void Display(string mensagem)
      {
         Display(mensagem, null);
      }

      public void Display(string mensagem, bool? hideCaptura)
      {
         ShowDisplay();

         if (hideCaptura.HasValue)
         {
            if (hideCaptura.Value)
               HideCaptura();
            else
               ShowCaptura();
         }

         dspDisplay.Bind(mensagem, false, true);
         stkPrincipal.UpdateLayout();
      }

      public void DisplayPrompt()
      {
         if (string.IsNullOrEmpty(Prompt))
            HideDisplay();
         else
            Display(Prompt, false);
      }

      public override void Bind(PW_GetData pw_GetData)
      {
         base.Bind(pw_GetData);

         btnEnviar.Visibility = System.Windows.Visibility.Collapsed;

         DisplayPrompt();

         //Indica se o dado a ser capturado, digitado pelo operador, será preenchido na tela a partir da esquerda ou da direita: 
         //0: a partir da direita (default); 
         //1: a partir da esquerda
         if (IniciaPelaEsquerda)
         {
            txtValor.TextAlignment = TextAlignment.Left;
            //txtValor.FlowDirection = FlowDirection.LeftToRight;
            txtValorPassword.FlowDirection = FlowDirection.LeftToRight;
         }
         else
         {
            txtValor.TextAlignment = TextAlignment.Right;
            //txtValor.FlowDirection = FlowDirection.RightToLeft;
            txtValorPassword.FlowDirection = FlowDirection.RightToLeft;
         }

         //Indica se os caracteres digitados devem ser apresentados (ecoados) na interface com o usuário, ou se devem ser mascarados 
         //0: mostrar o dado enquanto é digitado
         //1: mascarar o dado
         if (OcultarDadosDigitados)
         {
            txtValor.Visibility = Visibility.Collapsed;
            txtValorPassword.Visibility = Visibility.Visible;
         }
         else
         {
            txtValorPassword.Visibility = Visibility.Collapsed;
            txtValor.Visibility = Visibility.Visible;
            txtValor.TextChanged += txtValor_TextChanged;
            txtValor.PreviewTextInput += txtValor_PreviewTextInput;
            DataObject.AddPastingHandler(txtValor, new DataObjectPastingEventHandler(txtValor_Pasting));
            txtValor.Text = FormataInput("");
            txtValor.UpdateLayout();
         }
         FocusInput();
      }

      public void Capturar()
      {
         string texto = Input();
         if (ValidaInput(texto))
         {
            texto = RemoveMascaraDeCaptura(texto);
            texto = RemoveCaracteresNaoAceitos(texto);
            base.ParamCapturado(texto);
         }
      }

      #endregion

      #region Events

      [ObfuscationAttribute(Exclude = true)]
      private void txtValor_TextChanged(object sender, TextChangedEventArgs e)
      {
         txtValor.TextChanged -= txtValor_TextChanged;
         txtValor.Text = FormataInput(txtValor.Text);
         txtValor.TextChanged += txtValor_TextChanged;

         if (!IniciaPelaEsquerda)
         {
            txtValor.CaretIndex = txtValor.Text.Length;
         }
      }

      [ObfuscationAttribute(Exclude = true)]
      private void txtValor_PreviewTextInput(object sender, TextCompositionEventArgs e)
      {
         if (!string.IsNullOrEmpty(MascaraDeCaptura))
         {
            int inputMascaraLength = MascaraDeCaptura.Count(c => c.Equals('@'));
            int inputLength = RemoveCaracteresNaoAceitos(RemoveMascaraDeCaptura(txtValor.Text)).Length;
            if (inputLength >= inputMascaraLength)
            {
               e.Handled = true;
               return;
            }
         }
         e.Handled = !PermitidoInput(e.Text);
      }

      [ObfuscationAttribute(Exclude = true)]
      private void txtValor_Pasting(object sender, DataObjectPastingEventArgs e)
      {
         if (e.DataObject.GetDataPresent(typeof(string)))
         {
            string texto = (string)e.DataObject.GetData(typeof(string));
            if (!PermitidoInput(texto))
               e.CancelCommand();
         }
         else
            e.CancelCommand();
      }

      [ObfuscationAttribute(Exclude = true)]
      private void btnEnviar_Click(object sender, RoutedEventArgs e)
      {
         Capturar();
      }

      [ObfuscationAttribute(Exclude = true)]
      private void CapturaControl_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
         {
            Capturar();
            e.Handled = true;
         }
      }

      #endregion

      private string Input()
      {
         if (OcultarDadosDigitados)
            return txtValorPassword.Password;
         else
            return txtValor.Text;
      }

      private void FocusInput()
      {
         if (OcultarDadosDigitados)
            Focus(txtValorPassword);
         else
            Focus(txtValor);
      }

      private string RemoveMascaraDeCaptura(string texto)
      {
         string mascara = PW_GetData.szMascaraDeCaptura;

         //Remove a mascara do texto
         if (!string.IsNullOrEmpty(mascara))
            texto = string.Join("", texto.Split(mascara.Replace("@", "").ToCharArray()));

         return texto;
      }

      private string RemoveCaracteresNaoAceitos(string texto)
      {
         //Informa quais tipos de caracteres são aceitos. 
         switch (TiposEntradaPermitidos)
         {
            //case 0:
            //0: deve exibir o dado contido em szValorInicial, sem permitir a edição do mesmo; 
            //texto = _PW_GetData.szValorInicial;
            //break;
            case 1:
               //1: somente numéricos; 
               texto = new string(texto.Where(c => SomenteNumerico(c)).ToArray());
               break;
            case 2:
               //2: somente alfabéticos; 
               texto = new string(texto.Where(c => SomenteAlfabetico(c)).ToArray());
               break;
            case 3:
               //3: numéricos e alfabéticos; 
               texto = new string(texto.Where(c => SomenteNumericoAlfabetico(c)).ToArray());
               break;
            case 7:
               //7: numéricos, alfabéticos e especiais.
               texto = new string(texto.Where(c => SomenteNumericoAlfabeticoEspecial(c)).ToArray());
               break;
            default:
               break;
         }
         return texto;
      }

      private bool PermitidoInput(string texto)
      {
         if (!(new int[] { 0, 1, 2, 3, 7 }.Contains(PW_GetData.bTiposEntradaPermitidos)))
            return true;

         texto = RemoveMascaraDeCaptura(texto);

         switch (PW_GetData.bTiposEntradaPermitidos)
         {
            //case 0:
            //0: deve exibir o dado contido em szValorInicial, sem permitir a edição do mesmo; 
            //return false;
            case 1:
               //1: somente numéricos; 
               return Array.TrueForAll<char>(texto.ToCharArray(), SomenteNumerico);
            case 2:
               //2: somente alfabéticos; 
               return Array.TrueForAll<char>(texto.ToCharArray(), SomenteAlfabetico);
            case 3:
               //3: numéricos e alfabéticos; 
               return Array.TrueForAll<char>(texto.ToCharArray(), SomenteNumericoAlfabetico);
            case 7:
               //7: numéricos, alfabéticos e especiais.
               return Array.TrueForAll<char>(texto.ToCharArray(), SomenteNumericoAlfabeticoEspecial);
            default:
               return true;
         }
      }

      private string FormataInput(string texto)
      {
         texto = RemoveMascaraDeCaptura(texto);
         texto = RemoveCaracteresNaoAceitos(texto);

         //Mascara para apresentação do dado enquanto é digitado
         if (string.IsNullOrEmpty(MascaraDeCaptura))
         {
            //Seta a mascara com o tamanho maximo que é aceito
            //MascaraDeCaptura = new string('@', TamanhoMaximo);
            return texto;
         }

         int indice = 0;
         int length = 0;
         string textoFormatado = "";

         if (IniciaPelaEsquerda)
         {
            for (int i = 0; i < MascaraDeCaptura.Length; i++)
            {
               char c = MascaraDeCaptura[i];
               if (c == '@')
               {
                  if (texto.Length > indice + length)
                  {
                     length++;
                     if (texto.Length != indice + length)
                        continue;
                  }
                  textoFormatado += texto.Substring(indice, length);
                  break;
               }

               textoFormatado += string.Format("{0}{1}", texto.Substring(indice, length), c);
               indice += length;
               length = 0;
            }
            textoFormatado = string.Format("{0}{1}", textoFormatado, MascaraDeCaptura.Substring(textoFormatado.Length, MascaraDeCaptura.Length - textoFormatado.Length));
         }
         else
         {
            for (int i = MascaraDeCaptura.Length - 1; i >= 0; i--)
            {
               char c = MascaraDeCaptura[i];
               if (c == '@')
               {
                  if (texto.Length > indice + length)
                  {
                     length++;
                     if (texto.Length != indice + length && i > 0)
                        continue;
                  }
                  textoFormatado = string.Format("{0}{1}", texto.Substring(texto.Length - indice - length, length), textoFormatado);
                  break;
               }

               textoFormatado = string.Format("{0}{1}{2}", c, texto.Substring(texto.Length - indice - length, length), textoFormatado);
               indice += length;
               length = 0;
            }
            textoFormatado = string.Format("{0}{1}", MascaraDeCaptura.Substring(0, MascaraDeCaptura.Length - textoFormatado.Length), textoFormatado);
         }

         textoFormatado = textoFormatado.Replace("@", ".");

         return textoFormatado;
      }

      private bool ValidaInput(string texto)
      {
         texto = RemoveMascaraDeCaptura(texto);

         if (texto.Length < TamanhoMinimo)
         {
            DisplayErro(
               "",
               " TAMANHO ABAIXO DO  ",
               "       LIMITE       ",
               ""
               );
            return false;
         }

         if (TamanhoMaximo > 0 && texto.Length > TamanhoMaximo)
         {
            DisplayErro(
               "",
            "  TAMANHO ACIMA DO  ",
               "       LIMITE       ",
               ""
               );
            return false;
         }

         if (TiposEntradaPermitidosSomenteNumericos)
         {
            int valor = 0;

            int.TryParse(texto, out valor);

            if (ValorMinimo > 0 && valor < ValorMinimo)
            {
               DisplayErro(MsgDadoMenor);
               return false;
            }

            if (ValorMaximo > 0 && valor > ValorMaximo)
            {
               DisplayErro(MsgDadoMaior);
               return false;
            }
         }

         //Indica se o dado capturado deve passar por algum tipo de validação
         switch (PW_GetData.bValidacaoDado)
         {
            case 0:
               //0: sem validação
               break;
            case 1:
               //1: o dado não pode ser vazio
               if (string.IsNullOrEmpty(texto))
               {
                  DisplayErro(
                     "",
                     "   DADO NAO PODE    ",
                     "     SER VAZIO      ",
                     ""
                     );
                  return false;
               }
               break;
            case 2:
               //2: (último) dígito verificador, algoritmo módulo 10
               if (texto[texto.Length - 1] != Mod10(texto.Substring(0, texto.Length - 1)))
               {
                  DisplayErro(
                     "",
                     " DIGITO VERIFICADOR ",
                     "      INVALIDO      ",
                     ""
                     );
                  return false;
               }
               break;
            case 3:
               //3: CPF ou CNPJ
               if (!ValidaCpfCnpj(texto))
               {
                  if (texto.Length == 11)
                  {
                     DisplayErro(
                        "",
                        "    CPF INVALIDO    ",
                        "");
                     return false;
                  }

                  if (texto.Length == 14)
                  {
                     DisplayErro(
                        "",
                        "   CNPJ INVALIDO    ",
                        ""
                        );
                     return false;
                  }

                  DisplayErro(
                     "",
                     " CPF/CNPJ INVALIDO  ",
                     ""
                     );
                  return false;
               }
               break;
            case 4:
               //4: data no formato “MMAA”
               DateTime dt;
               if (!DateTime.TryParseExact(texto, "MMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
               {
                  DisplayErro(
                     "",
                     "   DATA INVALIDA    ",
                     ""
                     );
                  return false;
               }
               break;
            case 5:
               //5: data no formato “DDMMAA”
               if (!DateTime.TryParseExact(texto, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
               {
                  DisplayErro(
                     "",
                     "   DATA INVALIDA    ",
                     ""
                     );
                  return false;
               }
               break;
            case 6:
               //6: solicitar a digitação duas vezes iguais (confirmação) 
               break;
            default:
               break;
         }

         if (!string.IsNullOrEmpty(_senha))
         {
            if (texto != _senha)
            {
               DisplayErro(
                  "",
                  "   SENHA INVALIDA   ",
                  ""
                  );
               return false;
            }
         }

         /*
         (Byte) bAceitaNulo 
         bTipoDeDado = PWDAT_TYPED 
         Indica se o dado aceita nulo durante sua digitação, mesmo com bTamanhoMinimo>0. 0: exige a digitação; 1: aceita. No caso deste valor ser 1 e o usuário entrar com um dado nulo, o valor “” deve ser utilizado como parâmetro wParam da função PW_iAddParam
         */

         return true;
      }

      private void DisplayErro(params string[] mensagens)
      {
         DisplayErro(3000, mensagens);
      }

      private void DisplayErro(int timeout, params string[] mensagens)
      {
         string mensagem = string.Join("\r", mensagens);

         Keyboard.ClearFocus();
         HideCaptura();
         Display(mensagem);
         stkPrincipal.UpdateLayout();

         _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(timeout);
         _dispatcherTimer.Tick += (sender, e) =>
         {
            _dispatcherTimer.Stop();

            ShowCaptura();
            DisplayPrompt();
            stkPrincipal.UpdateLayout();
            FocusInput();
         };
         _dispatcherTimer.Start();
      }

      #region Validações

      private bool SomenteNumerico(char c)
      {
         return !(c < 48 || c > 57);
      }

      private bool SomenteAlfabetico(char c)
      {
         return !(c < 65 || c > 90 && c < 97 || c > 122);
      }

      private bool SomenteNumericoAlfabetico(char c)
      {
         return !(c < 48 || c > 57 && c < 65 || c > 90 && c < 97 || c > 122);
      }

      private bool SomenteNumericoAlfabeticoEspecial(char c)
      {
         return !(c < 32);
      }

      private bool ValidaCpfCnpj(string cpfcnpj)
      {
         int j, i, soma;
         string digito = string.Empty;
         string sequencia;

         cpfcnpj = Regex.Replace(cpfcnpj, "[^0-9]", string.Empty);

         //verificando se todos os numeros são iguais
         if (new string(cpfcnpj[0], cpfcnpj.Length) == cpfcnpj)
            return false;

         if (cpfcnpj.Length == 11)
         {
            //CPF
            for (i = 0; i <= 1; i++)
            {
               soma = 0;
               for (j = 0; j <= 8 + i; j++)
                  soma += int.Parse(cpfcnpj[j].ToString()) * (10 + i - j);

               digito += (soma % 11 == 0 || soma % 11 == 1) ? 0 : (11 - (soma % 11));
            }
            return (digito[0] == cpfcnpj[9] & digito[1] == cpfcnpj[10]);
         }
         else if (cpfcnpj.Length == 14)
         {
            //CNPJ
            sequencia = "6543298765432";
            for (i = 0; i <= 1; i++)
            {
               soma = 0;
               for (j = 0; j <= 11 + i; j++)
                  soma += int.Parse(cpfcnpj[j].ToString()) * int.Parse(sequencia.Substring(j + 1 - i, 1));

               digito += (soma % 11 == 0 || soma % 11 == 1) ? 0 : (11 - (soma % 11));
            }
            return (digito[0] == cpfcnpj[12] & digito[1] == cpfcnpj[13]);
         }
         else
            return false;
      }

      private int Mod10(string texto)
      {
         int i = 2;
         int sum = 0;
         int res = 0;
         foreach (char c in texto.ToCharArray())
         {
            res = Convert.ToInt32(c.ToString()) * i;
            sum += res > 9 ? (res - 9) : res;
            i = i == 2 ? 1 : 2;
         }
         return 10 - (sum % 10);
      }

      #endregion

   }
}