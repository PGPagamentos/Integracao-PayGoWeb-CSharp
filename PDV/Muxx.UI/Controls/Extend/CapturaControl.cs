using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Muxx.Lib.Helpers;
using Muxx.Lib.ValueObjects.Capturas;
using Muxx.Lib.ValueObjects.Structs;

namespace Muxx.UI.Controls.Extend
{
   public abstract class CapturaControl : UserControl
   {
      #region Delegates

      private Action<Captura> _paramCapturadoCallback;

      public Action<Captura> ParamCapturadoCallback
      {
         get { return _paramCapturadoCallback; }
         set { _paramCapturadoCallback = value; }
      }

      #endregion Delegates

      #region Member Variables

      private PW_GetData _pw_GetData;
      private bool _paramFoiCapturado = false;
      private Captura _captura = new Captura();

      #endregion

      #region Protected Properties

      protected byte TipoDeDado
      {
         get { return _pw_GetData.bTipoDeDado; }
      }

      protected string Prompt
      {
         get { return _pw_GetData.szPrompt; }
      }

      protected string MascaraDeCaptura
      {
         get { return _pw_GetData.szMascaraDeCaptura; }
         set { _pw_GetData.szMascaraDeCaptura = value; }
      }

      //Indica se o dado a ser capturado, digitado pelo operador, será preenchido na tela a partir da esquerda ou da direita: 
      //0: a partir da direita (default); 
      //1: a partir da esquerda
      protected bool IniciaPelaEsquerda
      {
         get { return _pw_GetData.bStartFromLeft == 1; }
      }

      //Indica se os caracteres digitados devem ser apresentados (ecoados) na interface com o usuário, ou se devem ser mascarados 
      //0: mostrar o dado enquanto é digitado
      //1: mascarar o dado
      protected bool OcultarDadosDigitados
      {
         get { return _pw_GetData.bOcultarDadosDigitados == 1; }
      }

      protected byte TamanhoMinimo
      {
         get { return _pw_GetData.bTamanhoMinimo; }
      }

      protected byte TamanhoMaximo
      {
         get { return _pw_GetData.bTamanhoMaximo; }
      }

      protected int ValorMinimo
      {
         get { return _pw_GetData.ulValorMinimo; }
      }

      protected int ValorMaximo
      {
         get { return _pw_GetData.ulValorMaximo; }
      }

      protected string MsgDadoMenor
      {
         get { return _pw_GetData.szMsgDadoMenor; }
      }

      protected string MsgDadoMaior
      {
         get { return _pw_GetData.szMsgDadoMaior; }
      }

      protected string MsgValidacao
      {
         get { return _pw_GetData.szMsgValidacao; }
      }

      protected byte TiposEntradaPermitidos
      {
         get { return _pw_GetData.bTiposEntradaPermitidos; }
      }

      protected bool TiposEntradaPermitidosSomenteNumericos
      {
         get { return _pw_GetData.bTiposEntradaPermitidos == 1; }
      }

      protected byte TeclasDeAtalho
      {
         get { return _pw_GetData.bTeclasDeAtalho; }
      }

      protected byte ItemInicial
      {
         get { return _pw_GetData.bItemInicial; }
      }

      #endregion

      #region Public Properties

      public PW_GetData PW_GetData
      {
         get { return _pw_GetData; }
         set { _pw_GetData = value; }
      }

      public bool ParamFoiCapturado
      {
         get { return _paramFoiCapturado; }
      }

      public Captura Captura
      {
         get { return _captura; }
      }

      #endregion

      #region Public Methods

      public virtual void Bind(PW_GetData pw_GetData)
      {
         _pw_GetData = pw_GetData;
         _paramFoiCapturado = false;
         _captura = new Captura();
      }

      public void CapturaCancelada(string param)
      {
         _captura.CapturaCancelada = true;
      }

      public void ParamCapturado(string param)
      {
         _paramFoiCapturado = true;
         _captura.Valor = param;

         //Callback
         if (_paramCapturadoCallback != null)
         {
            _paramCapturadoCallback(_captura);
         }
      }

      #endregion

      #region Protected Methods

      protected static void Focus(UIElement element)
      {
         if (!element.Focus())
         {
            element.Dispatcher.BeginInvoke(
               DispatcherPriority.Input,
               new ThreadStart(delegate () { element.Focus(); })
               );
         }
      }

      #endregion
   }
}