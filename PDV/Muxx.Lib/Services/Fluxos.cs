using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Muxx.Lib.Entities;
using Muxx.Lib.Exceptions;
using Muxx.Lib.Helpers;
using Muxx.Lib.ValueObjects;
using Muxx.Lib.ValueObjects.Capturas;
using Muxx.Lib.ValueObjects.Enums;
using Muxx.Lib.ValueObjects.Structs;

namespace Muxx.Lib.Services
{
   public static class Fluxos
   {

      #region Const
      private const string DISPLAY_PADRAO = "\r   PROCESSANDO...   \r";
      #endregion

      #region Delegates

      private static Action<PW_GetData> _loopCallback;

      private static Action<string, bool> _displayCallback;
      private static Func<PW_GetData, Captura> _capturaMenuFunc;
      private static Func<PW_GetData, Captura> _capturaDigitadaFunc;
      private static Func<PW_GetData, CapturaCodigoBarras> _capturaCodigoBarrasFunc;
      private static Func<PW_GetData, Captura> _capturaSenhaFunc;
      private static Func<PW_GetData, string, Captura> _capturaQRCodeFunc;

      private static Func<bool> _cancelarOperacaoFunc = null;

      /// <summary>
      /// Callback chamado nos loops:
      /// - PW_iExecTransac
      /// - PW_iPPEventLoop
      /// </summary>
      public static Action<PW_GetData> LoopCallback
      {
         get { return _loopCallback; }
         set { _loopCallback = value; }
      }

      /// <summary>
      /// Callback para exibição do display.
      /// </summary>
      public static Action<string, bool> DisplayCallback
      {
         get { return _displayCallback; }
         set { _displayCallback = value; }
      }

      /// <summary>
      /// Menu (PWDAT_MENU)
      /// Delegate para exibição e captura do menu.
      /// </summary>
      public static Func<PW_GetData, Captura> CapturaMenuFunc
      {
         get { return _capturaMenuFunc; }
         set { _capturaMenuFunc = value; }
      }

      /// <summary>
      /// Entrada digitada (PWDAT_TYPED)
      /// Delegate para exibição e captura da entrada digitada.
      /// </summary>
      public static Func<PW_GetData, Captura> CapturaDigitadaFunc
      {
         get { return _capturaDigitadaFunc; }
         set { _capturaDigitadaFunc = value; }
      }

      /// <summary>
      /// Código de barras (PWDAT_BARCODE)
      /// Delegate para exibição e captura do código de barras.
      /// </summary>
      public static Func<PW_GetData, CapturaCodigoBarras> CapturaCodigoBarrasFunc
      {
         get { return _capturaCodigoBarrasFunc; }
         set { _capturaCodigoBarrasFunc = value; }
      }

      /// <summary>
      /// Senha (PWDAT_USERAUTH)
      /// Delegate para exibição e captura da senha.
      /// </summary>
      public static Func<PW_GetData, Captura> CapturaSenhaFunc
      {
         get { return _capturaSenhaFunc; }
         set { _capturaSenhaFunc = value; }
      }

      /// <summary>
      /// Exibição de QR Code (PWDAT_DSPQRCODE)
      /// Delegate para exibição e captura do QR Code.
      /// </summary>
      public static Func<PW_GetData, string, Captura> CapturaQRCodeFunc
      {
         get { return _capturaQRCodeFunc; }
         set { _capturaQRCodeFunc = value; }
      }

      /// <summary>
      /// Delegate que verifica se a automação quer 
      /// interromper uma operação já iniciada.<br/>
      /// Por exemplo: Operação cancelada pelo operador.
      /// </summary>
      public static Func<bool> CancelarOperacaoFunc
      {
         set
         {
            _cancelarOperacaoFunc = value;
            CallCancelarOperacaoFunc();
         }
      }

      private static void CallLoopCallback()
      {
         Log.PrintThread(string.Format("CallLoopCallback"));

         //Loop Callback
         if (_loopCallback != null)
         {
            _loopCallback(_pw_GetData);
         }
      }

      private static void CallDisplayPadraoCallback()
      {
         CallDisplayCallback(DISPLAY_PADRAO, true);
      }

      private static void CallDisplayCallback(string display)
      {
         CallDisplayCallback(display, false);
      }

      private static void CallDisplayCallback(string display, bool padrao)
      {
         Log.PrintThread(string.Format("CallDisplayCallback [{0}]", display));

         //Display Callback
         if (_displayCallback != null)
         {
            _displayCallback(display, padrao);
         }
      }

      private static Captura CallCapturaMenuFunc(PW_GetData pw_GetData)
      {
         Log.PrintThread("CallCapturaMenuFunc");

         if (_capturaMenuFunc == null)
            throw new FunctionNotDefinedExceptions("CapturaMenuFunc");

         return _capturaMenuFunc(pw_GetData);
      }

      private static Captura CallCapturaDigitadaFunc(PW_GetData pw_GetData)
      {
         Log.PrintThread("CallCapturaDigitadaFunc");

         if (_capturaDigitadaFunc == null)
            throw new FunctionNotDefinedExceptions("CapturaDigitadaFunc");

         return _capturaDigitadaFunc(pw_GetData);
      }

      private static CapturaCodigoBarras CallCapturaCodigoBarrasFunc(PW_GetData pw_GetData)
      {
         Log.PrintThread("CallCapturaCodigoBarrasFunc");

         if (_capturaCodigoBarrasFunc == null)
            throw new FunctionNotDefinedExceptions("CapturaCodigoBarrasFunc");

         return _capturaCodigoBarrasFunc(pw_GetData);
      }

      private static Captura CallCapturaSenhaFunc(PW_GetData pw_GetData)
      {
         Log.PrintThread("CallCapturaSenhaFunc");

         if (_capturaSenhaFunc == null)
            throw new FunctionNotDefinedExceptions("CapturaSenhaFunc");

         return _capturaSenhaFunc(pw_GetData);
      }

      private static Captura CallCapturaQRCodeFunc(PW_GetData pw_GetData, string qrCode)
      {
         Log.PrintThread("CallCapturaQRCodeFunc");

         if (_capturaQRCodeFunc == null)
            throw new FunctionNotDefinedExceptions("CapturaQRCodeFunc");

         return _capturaQRCodeFunc(pw_GetData, qrCode);
      }

      private static bool CallCancelarOperacaoFunc()
      {
         //Log.PrintThread("CallCancelarOperacaoFunc");

         if (_cancelarOperacaoFunc == null)
            return false;

         _operacaoCancelada = _cancelarOperacaoFunc();
         return _operacaoCancelada;
      }

      #endregion

      #region Member Variables

      private static short _ret;
      private static short? _retExecTransac;
      private static bool _pw_iInitJaChamada = false;
      private static PW_GetData _pw_GetData;
      /// <summary>
      /// Adiciona parametros usados ANTES do Loop principal, 
      /// ou seja, logo após PW_iNewTransac.
      /// </summary>
      private static List<Param> _params = new List<Param>();
      /// <summary>
      /// Adiciona parametros usados DURANTE o Loop principal, 
      /// ou seja, logo após PW_iExecTransac. 
      /// Funciona como um repositorio.
      /// </summary>
      private static List<Param> _paramsFonteDados = new List<Param>();
      /// <summary>
      /// Armazena TODOS os parametros solicitados em PW_iExecTransac, 
      /// independente da captura.
      /// </summary>
      private static List<Param> _paramsSolicitados = new List<Param>();
      /// <summary>
      /// Armazena SOMENTE os parametros capturados, ou seja, 
      /// que serão informados no PW_iAddParam,
      /// depois de solicitados em PW_iExecTransac.
      /// </summary>
      private static List<Param> _paramsCapturados = new List<Param>();
      /// <summary>
      /// Adiciona resultados para ser capturado 
      /// posteriormente no <see cref="FluxoGetResult()"/>
      /// </summary>
      private static List<Info> _results = new List<Info>();
      /// <summary>
      /// Armazena os resultados que retornaram do PW_iGetResult,
      /// funciona como uma espécie de cache.
      /// </summary>
      private static List<Info> _resultsEnviados = new List<Info>();
      private static string _comprovanteCompleto;
      private static string _comprovanteDiferenciadoEstabelecimento;
      private static string _comprovanteDiferenciadoCliente;
      private static string _comprovanteReduzidoCliente;

      private static bool _verificaKeyDown = false;
      private static bool _pinpadCancelarOperacao = false;
      private static Func<PW_GetData, bool> _capturaCardInfFunc = null;

      private static bool _operacaoCancelada = false;

      #endregion

      #region Public Properties

      /// <summary>
      /// Último retorno da chamada:<br/>
      /// - PW_ixxx<br/>
      /// </summary>
      public static short Ret
      {
         get { return _ret; }
      }

      /// <summary>
      /// Último retorno da chamada:<br/>
      /// - PW_iExecTransac<br/>
      /// </summary>
      public static short? RetExecTransac
      {
         get { return _retExecTransac; }
      }

      public static string RetToString
      {
         get
         {
            PWRET pwret;
            if (Enum.TryParse<PWRET>(_ret.ToString(), out pwret))
               return pwret.ToString();
            return string.Format("{0} - DESCONHECIDO???", _ret);
         }
      }

      ///<inheritdoc cref="_params"/>
      public static List<Param> Params
      {
         get { return _params; }
      }

      ///<inheritdoc cref="_paramsFonteDados"/>
      public static List<Param> ParamsFonteDados
      {
         get { return _paramsFonteDados; }
      }

      ///<inheritdoc cref="_paramsSolicitados"/>
      public static List<Param> ParamsSolicitados
      {
         get { return _paramsSolicitados; }
      }

      ///<inheritdoc cref="_paramsCapturados"/>
      public static List<Param> ParamsCapturados
      {
         get { return _paramsCapturados; }
      }

      ///<inheritdoc cref="_results"/>
      public static List<Info> Results
      {
         get { return _results; }
      }

      ///<inheritdoc cref="_resultsEnviados"/>
      public static List<Info> ResultsEnviados
      {
         get { return _resultsEnviados; }
      }

      /// <summary>
      /// <inheritdoc cref="_resultsEnviados"/><br/>
      /// Retorna só os que tiveram sucesso na chamada.<br/>
      /// </summary>
      public static List<Info> ResultsEnviadosComSucesso
      {
         get
         {
            return
               _resultsEnviados
               .Where(info => info.Ret == (short)PWRET.PWRET_OK)
               .ToList();
         }
      }

      public static string ComprovanteCompleto
      {
         get { return _comprovanteCompleto; }
      }

      public static string ComprovanteCompletoFormatado
      {
         get { return FormatarComprovante(_comprovanteCompleto); }
      }

      public static string ComprovanteDiferenciadoEstabelecimento
      {
         get { return _comprovanteDiferenciadoEstabelecimento; }
      }

      public static string ComprovanteDiferenciadoEstabelecimentoFormatado
      {
         get { return FormatarComprovante(_comprovanteDiferenciadoEstabelecimento); }
      }

      public static string ComprovanteDiferenciadoCliente
      {
         get { return _comprovanteDiferenciadoCliente; }
      }

      public static string ComprovanteDiferenciadoClienteFormatado
      {
         get { return FormatarComprovante(_comprovanteDiferenciadoCliente); }
      }

      public static string ComprovanteReduzidoCliente
      {
         get { return _comprovanteReduzidoCliente; }
      }

      public static string ComprovanteReduzidoClienteFormatado
      {
         get { return FormatarComprovante(_comprovanteReduzidoCliente); }
      }

      public static bool OperacaoCancelada
      {
         get { return _operacaoCancelada; }
      }

      #endregion

      #region Public Static Methods

      /// <summary>
      /// Limpa os dados da última transação.
      /// </summary>
      public static void Clear()
      {
         _ret = 0;
         _retExecTransac = null;

         _params.Clear();
         _paramsFonteDados.Clear();
         _paramsSolicitados.Clear();
         _paramsCapturados.Clear();
         _results.Clear();
         _resultsEnviados.Clear();

         _comprovanteCompleto = null;
         _comprovanteDiferenciadoEstabelecimento = null;
         _comprovanteDiferenciadoCliente = null;
         _comprovanteReduzidoCliente = null;

         _pinpadCancelarOperacao = false;
      }

      public static void ParamsClear()
      {
         _params.Clear();
      }

      public static void ParamsFonteDadosClear()
      {
         _paramsFonteDados.Clear();
      }

      public static void ResultsClear()
      {
         _results.Clear();
      }

      public static void ResultsEnviadosClear()
      {
         _resultsEnviados.Clear();
      }

      /// <summary>
      /// <inheritdoc cref="_params"/>
      /// </summary>
      /// <param name="pwInfo"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public static bool ParamsAdd(PWINFO pwInfo, string value)
      {
         Param param = _params.FirstOrDefault(p => p.PwInfo.Equals(pwInfo));
         if (param != null)
            _params.Remove(param);
         _params.Add(Param.New(pwInfo, value));
         return true;
      }

      /// <summary>
      /// <inheritdoc cref="_paramsFonteDados"/>
      /// </summary>
      /// <param name="pwInfo"></param>
      /// <param name="value"></param>
      /// <returns></returns>
      public static bool ParamsFonteDadosAdd(PWINFO pwInfo, string value)
      {
         Param param = _paramsFonteDados.FirstOrDefault(p => p.PwInfo.Equals(pwInfo));
         if (param != null)
            _paramsFonteDados.Remove(param);
         _paramsFonteDados.Add(Param.New(pwInfo, value));
         return true;
      }

      /// <summary>
      /// <inheritdoc cref="_results"/>
      /// </summary>
      /// <param name="pwInfo"></param>
      /// <returns></returns>
      public static bool ResultsAdd(PWINFO pwInfo)
      {
         Info info = new Info(pwInfo);
         _results.Add(info);
         return true;
      }

      ///<inheritdoc cref="FluxoInit(string, bool)"/>
      public static Task<bool> FluxoInitAsync()
      {
         return
            Task.Factory.StartNew<bool>(() =>
            {
               return FluxoInit();
            });
      }

      ///<inheritdoc cref="FluxoInit(string, bool)"/>
      public static bool FluxoInit()
      {
         return FluxoInit(null, true);
      }

      ///<inheritdoc cref="FluxoInit(string, bool)"/>
      public static bool FluxoInit(string workingDir)
      {
         return FluxoInit(workingDir, true);
      }

      /// <summary>
      /// Inicializa a biblioteca<br/>
      /// - PW_iInit.
      /// </summary>
      /// <param name="workingDir">
      /// Diretório de trabalho (caminho completo) para uso exclusivo do PayGo.
      /// </param>
      /// <param name="controlaChamada">
      /// Controla se já foi efetuada uma chamada à função 
      /// PW_iInit após o carregamento da biblioteca.
      /// </param>
      /// <returns></returns>
      public static bool FluxoInit(string workingDir, bool controlaChamada)
      {
         if (controlaChamada)
         {
            if (_pw_iInitJaChamada)
               return true;
         }

         if (workingDir == null)
            _ret = PGWebLib.PW_iInit();
         else
            _ret = PGWebLib.PW_iInit(workingDir);
         if (_ret != (short)PWRET.PWRET_OK)
         {
            //Transacao Finalizada
            _pw_iInitJaChamada = false;
            return false;
         }
         _pw_iInitJaChamada = true;
         return true;
      }

      /// <summary>
      /// Inicia uma nova transação através do PayGo.<br/>
      /// - PW_iNewTransac.<br/>
      /// - PW_iAddParam: adiciona os parâmetros obrigatórios e iniciais da transação.<br/>
      /// </summary>
      /// <param name="pwoper">
      /// Tipo de transação a ser realizada (PWOPER_xxx, conforme enum).
      /// </param>
      /// <returns></returns>
      public static bool FluxoNewTransac(PWOPER pwoper)
      {
         Log.PrintThread("FluxoNewTransac");

         _retExecTransac = null;

         _paramsSolicitados.Clear();
         _paramsCapturados.Clear();
         _resultsEnviados.Clear();

         _comprovanteCompleto = null;
         _comprovanteDiferenciadoEstabelecimento = null;
         _comprovanteDiferenciadoCliente = null;
         _comprovanteReduzidoCliente = null;

         _pinpadCancelarOperacao = false;

         CallDisplayPadraoCallback();

         _ret = PGWebLib.PW_iNewTransac(pwoper);
         if (_ret != (short)PWRET.PWRET_OK)
         {
            //Transacao Finalizada
            return false;
         }

         //Parametros adicionados em todas as transações...
         if (!FluxoAddParam(_params))
         {
            //Transacao Finalizada
            return false;
         }

         return true;
      }

      /// <summary>
      /// Tenta realizar uma transação através do PayGo, utilizando os
      /// parâmetros previamente definidos através de PW_iAddParam.<br/>
      /// - PW_iExecTransac: loop.<br/>
      /// </summary>
      /// <returns>
      /// true : Transação realizada com sucesso<br/>
      /// false: Transação com erro<br/>
      /// </returns>
      public static bool FluxoExecTransac()
      {
         Log.PrintThread("FluxoExecTransac");

         bool callDisplayPadrao = true;
         short piNumParam = 0;
         PW_GetData[] pw_GetDatas = null;

         #region Loop

         for (; ; )
         {
            //A Captura deve ser interrompida?
            if (CallCancelarOperacaoFunc())
            {
               //Transacao Finalizada
               return false;
            }

            //PWRET_MOREDATA
            for (int i = 0; i < piNumParam; i++)
            {
               _verificaKeyDown = false;
               _capturaCardInfFunc = null;

               _pw_GetData = pw_GetDatas[i];
               _paramsSolicitados.Add(Param.New(_pw_GetData.wIdentificador, null));

               CallLoopCallback();

               switch (_pw_GetData.bTipoDeDado)
               {
                  case (byte)PWDAT.PWDAT_MENU:

                     if (!FluxoMenu(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_TYPED:

                     if (!FluxoTyped(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_BARCODE:

                     if (!FluxoBarCode(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_CARDINF:

                     switch (_pw_GetData.ulTipoEntradaCartao)
                     {
                        case 1:
                           //Se igual a 1, solicitar a digitação do número do cartão; 
                           if (!FluxoCardInf(_pw_GetData))
                           {
                              //Transacao Finalizada
                              return false;
                           }
                           break;

                        case 2:
                           //Se igual a 2, chamar a função PW_iPPGetCard, Tratando Fall Back
                           if (!FluxoPinPad(() => PGWebLib.PW_iPPGetCard((ushort)i)))
                           {
                              //Transacao Finalizada
                              return false;
                           }
                           break;

                        case 3:
                           //Se igual a 3, chamar a função PW_iPPGetCard, e também permitir que o operador digite o número do cartão. 
                           //Caso o operador opte pela digitação, a Automação deve interromper o processamento do PIN-pad
                           //antes de prosseguir. 

                           FluxoPinPadVerificaTrocaParaFluxoCardInf();

                           if (!FluxoPinPad(() => PGWebLib.PW_iPPGetCard((ushort)i)))
                           {
                              //Transacao Finalizada
                              return false;
                           }
                           break;

                        default:
                           break;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_USERAUTH:

                     if (!FluxoUserAuth(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_DSPCHECKOUT:

                     callDisplayPadrao = false;

                     if (!FluxoDspCheckout(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_DSPQRCODE:

                     callDisplayPadrao = false;

                     if (!FluxoDspQRCode(_pw_GetData))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  #region Outras capturas no PIN-pad

                  case (byte)PWDAT.PWDAT_PPENTRY:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPGetData((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_PPENCPIN:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPGetPIN((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_CARDOFF:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPGoOnChip((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_CARDONL:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPFinishChip((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_PPCONF:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPConfirmData((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_PPREMCRD:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPRemoveCard()))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_PPGENCMD:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPGenericCMD((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_PPDATAPOSCNF:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPPositiveConfirmation((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  case (byte)PWDAT.PWDAT_TSTKEY:
                     if (!FluxoPinPad(() => PGWebLib.PW_iPPTestKey((ushort)i)))
                     {
                        //Transacao Finalizada
                        return false;
                     }
                     break;

                  #endregion Outras capturas no PIN-pad

                  default:
                     break;
               }
            }

            if (callDisplayPadrao)
               CallDisplayPadraoCallback();

            //PW_iExecTransac
            piNumParam = 9; //Valor sugerido: 9.
            pw_GetDatas = new PW_GetData[piNumParam];

            _ret = PGWebLib.PW_iExecTransac(pw_GetDatas, ref piNumParam);
            _retExecTransac = _ret;

            if (_ret == (short)PWRET.PWRET_MOREDATA ||
                _ret == (short)PWRET.PWRET_NOTHING)
               continue;

            break;
         }

         #endregion Loop

         if (_ret != (short)PWRET.PWRET_OK)
            return false;

         return true;
      }

      ///<inheritdoc cref="FluxoPrincipal(PWOPER)"/>
      public static Task<bool> FluxoPrincipalAsync(PWOPER pwoper)
      {
         return
            Task.Factory.StartNew<bool>(() =>
            {
               return FluxoPrincipal(pwoper);
            });
      }

      /// <summary>
      /// <inheritdoc cref="FluxoNewTransac(PWOPER)"/>
      /// <br/>
      /// <inheritdoc cref="FluxoExecTransac"/>
      /// </summary>
      /// <param name="pwoper"></param>
      /// <returns>
      /// </returns>
      public static bool FluxoPrincipal(PWOPER pwoper)
      {
         Log.PrintThread("FluxoPrincipal");

         if (!FluxoNewTransac(pwoper))
         {
            //Transacao Finalizada
            return false;
         }
         return FluxoExecTransac();
      }

      /// <summary>
      /// Durante a captura do cartão no pinpad, verifica se alguma tecla 
      /// numérica é pressionada, caso afirmativo aborta a captura no pinpad 
      /// e troca para a captura digitada do cartão.
      /// </summary>
      /// <param name="somenteTeclaNumerica">
      /// Delegate que retorna true se alguma tecla numérica for pressionada
      /// </param>
      public static void FluxoPinPadKeyDown(Func<bool> somenteTeclaNumerica)
      {
         if (!_verificaKeyDown)
            return;

         switch (_pw_GetData.bTipoDeDado)
         {
            case (byte)PWDAT.PWDAT_CARDINF:

               switch (_pw_GetData.ulTipoEntradaCartao)
               {
                  case 2:
                  case 3:
                     //Se igual a 2, chamar a função PW_iPPGetCard, Tratando Fall Back
                     //Se igual a 3, chamar a função PW_iPPGetCard, e também permitir que o operador digite o número do cartão. 
                     //Caso o operador opte pela digitação, a Automação deve interromper o processamento do PIN-pad
                     //antes de prosseguir. 
                     if (somenteTeclaNumerica())
                     {
                        _verificaKeyDown = false;
                        _pinpadCancelarOperacao = true;
                        _capturaCardInfFunc = FluxoCardInf;
                     }
                     break;
               }

               break;
         }
      }

      /// <summary>
      /// Interface com o pinpad<br/>
      /// - PW_iPPxxx()<br/>
      /// </summary>
      /// <param name="pw_iPPxxx"></param>
      /// <returns></returns>
      public static bool FluxoPinPad(Func<short> pw_iPPxxx)
      {
         CallDisplayPadraoCallback();

         _ret = pw_iPPxxx();
         return FluxoPinPad(_ret);
      }

      /// <summary>
      /// Interface com o pinpad<br/>
      /// - PW_iPPEventLoop.<br/>
      /// - PW_iPPAbort.<br/>
      /// </summary>
      /// <param name="ret"></param>
      /// <returns></returns>
      public static bool FluxoPinPad(short ret)
      {
         if (ret != (byte)PWRET.PWRET_OK)
            return false;

         //Tamanho mínimo recomendado: 100 bytes.
         uint ulDisplaySize = 100;
         StringBuilder pszDisplay = new StringBuilder((int)ulDisplaySize);

         for (; ; )
         {
            CallLoopCallback();

            _ret = PGWebLib.PW_iPPEventLoop(pszDisplay, ulDisplaySize);

            switch (_ret)
            {
               case (short)PWRET.PWRET_OK:
                  //Captura Realizada
                  //Evento de Display
                  CallDisplayPadraoCallback();
                  return true;

               case (short)PWRET.PWRET_DISPLAY:
                  //Evento de Display
                  CallDisplayCallback(pszDisplay.ToString());
                  break;

               case (short)PWRET.PWRET_NOTHING:
                  break;

               case (short)PWRET.PWRET_FALLBACK:
                  FluxoPinPadVerificaTrocaParaFluxoCardInf();
                  break;

               case (short)PWRET.PWRET_CANCEL:
                  _operacaoCancelada = true;
                  return false;

               default:
                  return false;
            }

            //A Captura deve ser interrompida?
            if (CallCancelarOperacaoFunc())
            {
               _pinpadCancelarOperacao = true;
               //Evento de Display
               CallDisplayCallback("\r      OPERACAO\r      CANCELADA");
            }

            //A Captura deve ser interrompida?
            if (_pinpadCancelarOperacao)
            {
               _pinpadCancelarOperacao = false;

               PGWebLib.PW_iPPAbort();
               //FluxoPinPad(() => PGWebLib.PW_iPPAbort());

               if (_capturaCardInfFunc != null)
               {
                  return _capturaCardInfFunc(_pw_GetData);
               }

               return false;
            }

            //Atenção: durante o ciclo de chamadas repetitivas a PW_iPPEventLoop, 
            //a Automação deve tomar o cuidado de liberar o tempo de processador 
            //para o sistema operacional. 
            //Recomendamos aguardar no mínimo 100 milissegundos entre duas chamadas. 
            Thread.Sleep(500);
         }
      }

      /// <summary>
      /// <inheritdoc cref="FluxoAddParam(List{Param})"/>
      /// </summary>
      /// <param name="parametro">
      /// Parâmetros com identificador e valor
      /// </param>
      /// <returns></returns>
      public static bool FluxoAddParam(Param parametro)
      {
         List<Param> parametros = new List<Param>();
         parametros.Add(parametro);
         return FluxoAddParam(parametros);
      }

      /// <summary>
      /// Alimentar a biblioteca com as informações da transação a ser realizada<br/>
      /// - PW_iAddParam.<br/>
      /// </summary>
      /// <param name="parametros">
      /// Lista de parâmetros com identificador e valor
      /// </param>
      /// <returns></returns>
      public static bool FluxoAddParam(List<Param> parametros)
      {
         foreach (var parametro in parametros)
         {
            _ret = PGWebLib.PW_iAddParam(parametro.PwInfo, parametro.Value);
            parametro.Ret = _ret;
            if (_ret != (short)PWRET.PWRET_OK)
            {
               //Transacao Finalizada
               return false;
            }
         }
         return true;
      }

      public static bool FluxoAddParamCapturado(Param parametro)
      {
         _paramsCapturados.Add(parametro);

         return FluxoAddParam(parametro);
      }

      /// <summary>
      /// <inheritdoc cref="FluxoGetResult(PWINFO, out Info)"/>
      /// Recupera tudo que foi adicionado usando <see cref="ResultsAdd"/><br/>
      /// </summary>
      public static void FluxoGetResult()
      {
         _results
            .All((info) =>
            {
               FluxoGetResult(info.PwInfo);
               return true;
            });
      }

      ///<inheritdoc cref="FluxoGetResultPwInfos"/>
      public static Task FluxoGetResultPwInfosAsync()
      {
         return
            Task.Factory.StartNew(() =>
            {
               FluxoGetResultPwInfos();
            });
      }

      /// <summary>
      /// <inheritdoc cref="FluxoGetResult(PWINFO, out Info)"/>
      /// Recupera todo o enum PWINFO<br/>
      /// </summary>
      public static void FluxoGetResultPwInfos()
      {
         Enum.GetValues(typeof(PWINFO))
            .Cast<PWINFO>()
            .All((pwInfo) =>
            {
               FluxoGetResult(pwInfo);
               return true;
            });
      }

      ///<inheritdoc cref="FluxoGetResult(PWINFO, out Info)"/>
      public static bool FluxoGetResult(PWINFO pwInfo)
      {
         Info result;
         return FluxoGetResult(pwInfo, out result);
      }

      /// <summary>
      /// Obtem informações que resultaram da transação efetuada, 
      /// independentemente de ter sido bem ou malsucedida.<br/>
      /// - PW_iGetResult.<br/>
      /// </summary>
      /// <param name="pwInfo">
      /// PWINFO_xxx da informação solicitada sendo requisitada
      ///</param>
      /// <param name="result">
      /// Valor da informação solicitada
      /// </param>
      /// <returns></returns>
      public static bool FluxoGetResult(PWINFO pwInfo, out Info result)
      {
         string value;
         _ret = PGWebLib.PW_iGetResult((short)pwInfo, out value);
         result = Info.New(pwInfo, value, _ret);

         //Adiciona na lista para consulta no final do fluxo se necessario...
         Info info = _resultsEnviados.FirstOrDefault(r => r.PwInfo.Equals(pwInfo));
         if (info != null)
            _resultsEnviados.Remove(info);
         _resultsEnviados.Add(result);

         if (_ret != (short)PWRET.PWRET_OK)
            return false;

         return true;
      }

      /// <summary>
      /// Verifica se existe alguma transação pendente de confirmação no PayGoWeb<br/>
      /// - PW_iExecTransac retornou PWRET_FROMHOSTPENDTRN<br/>
      /// </summary>
      /// <returns></returns>
      public static bool PossuiPendencia()
      {
         if (!_retExecTransac.HasValue)
            return false;

         if (_retExecTransac.Value != (short)PWRET.PWRET_FROMHOSTPENDTRN)
            return false;

         return true;
      }

      ///<inheritdoc cref="FluxoConfirmacaoPendencia(uint)"/>
      public static bool FluxoConfirmacaoPendencia(PWCNF pwcnf)
      {
         return FluxoConfirmacaoPendencia((uint)pwcnf);
      }

      /// <summary>
      /// Realiza a confirmação da transação pendente de confirmação no PayGoWeb<br/>
      /// - PW_iExecTransac retornou PWRET_FROMHOSTPENDTRN<br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_PNDREQNUM,<br/>
      /// --- PWINFO_PNDAUTLOCREF,<br/>
      /// --- PWINFO_PNDAUTEXTREF,<br/>
      /// --- PWINFO_PNDVIRTMERCH,<br/>
      /// --- PWINFO_PNDAUTHSYST<br/>
      /// - PW_iConfirmation.<br/>
      /// </summary>
      /// <param name="pwcnf"></param>
      /// <returns></returns>
      public static bool FluxoConfirmacaoPendencia(uint pwcnf)
      {
         if (!PossuiPendencia())
            return false;

         string pszReqNum;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_PNDREQNUM, out pszReqNum);
         if (_ret != (short)PWRET.PWRET_OK)
            pszReqNum = "";

         string pszLocRef;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_PNDAUTLOCREF, out pszLocRef);
         if (_ret != (short)PWRET.PWRET_OK)
            pszLocRef = "";

         string pszExtRef;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_PNDAUTEXTREF, out pszExtRef);
         if (_ret != (short)PWRET.PWRET_OK)
            pszExtRef = "";

         string pszVirtMerch;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_PNDVIRTMERCH, out pszVirtMerch);
         if (_ret != (short)PWRET.PWRET_OK)
            pszVirtMerch = "";

         string pszAuthSyst;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_PNDAUTHSYST, out pszAuthSyst);
         if (_ret != (short)PWRET.PWRET_OK)
            pszAuthSyst = "";

         _ret = PGWebLib.PW_iConfirmation(pwcnf, pszReqNum, pszLocRef, pszExtRef, pszVirtMerch, pszAuthSyst);
         if (_ret != (short)PWRET.PWRET_OK)
            return false;

         return true;
      }

      /// <summary>
      /// Verifica se a transação requer confirmação<br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_CNFREQ<br/>
      /// </summary>
      /// <returns></returns>
      public static bool RequerConfirmacao()
      {
         string pszData;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_CNFREQ, out pszData);
         if (_ret != (short)PWRET.PWRET_OK)
            return false;

         if (pszData != "1")
            return false;

         //Se este for igual a 1, indica que o status final da transação deve obrigatoriamente 
         //ser informado pela Automação ao Pay&Go Web, através da função PW_iConfirmation.

         return true;
      }

      /// <summary>
      /// <inheritdoc cref="FluxoConfirmacao(uint)"/>
      /// --- PWCNF.PWCNF_CNF_AUTO
      /// </summary>
      /// <returns></returns>
      public static bool FluxoConfirmacao()
      {
         return FluxoConfirmacao(PWCNF.PWCNF_CNF_AUTO);
      }

      ///<inheritdoc cref="FluxoConfirmacao(uint)"/>
      public static bool FluxoConfirmacao(PWCNF pwcnf)
      {
         return FluxoConfirmacao((uint)pwcnf);
      }

      /// <summary>
      /// Realiza a confirmação da transação atual<br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_CNFREQ,<br/>
      /// --- PWINFO_REQNUM,<br/>
      /// --- PWINFO_AUTLOCREF,<br/>
      /// --- PWINFO_AUTEXTREF,<br/>
      /// --- PWINFO_VIRTMERCH,<br/>
      /// --- PWINFO_AUTHSYST<br/>
      /// - PW_iConfirmation.<br/>
      /// </summary>
      /// <param name="pwcnf"></param>
      /// <returns></returns>
      public static bool FluxoConfirmacao(uint pwcnf)
      {
         if (!RequerConfirmacao())
            return false;

         string pszReqNum;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_REQNUM, out pszReqNum);
         if (_ret != (short)PWRET.PWRET_OK)
            pszReqNum = "";

         string pszLocRef;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_AUTLOCREF, out pszLocRef);
         if (_ret != (short)PWRET.PWRET_OK)
            pszLocRef = "";

         string pszExtRef;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_AUTEXTREF, out pszExtRef);
         if (_ret != (short)PWRET.PWRET_OK)
            pszExtRef = "";

         string pszVirtMerch;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_VIRTMERCH, out pszVirtMerch);
         if (_ret != (short)PWRET.PWRET_OK)
            pszVirtMerch = "";

         string pszAuthSyst;
         _ret = PGWebLib.PW_iGetResult((short)PWINFO.PWINFO_AUTHSYST, out pszAuthSyst);
         if (_ret != (short)PWRET.PWRET_OK)
            pszAuthSyst = "";

         _ret = PGWebLib.PW_iConfirmation(pwcnf, pszReqNum, pszLocRef, pszExtRef, pszVirtMerch, pszAuthSyst);
         if (_ret != (short)PWRET.PWRET_OK)
            return false;

         return true;
      }

      /// <summary>
      /// Recupera o comprovante completo que fica 
      /// disponível em <see cref="ComprovanteCompleto"/><br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_RCPTFULL<br/>
      /// </summary>
      /// <returns>
      /// true : Sucesso, o comprovante está disponível<br/>
      /// false: O comprovante não está disponível<br/>
      /// </returns>
      public static bool FluxoComprovanteCompleto()
      {
         Info info;
         if (FluxoGetResult(PWINFO.PWINFO_RCPTFULL, out info))
         {
            _comprovanteCompleto = info.Value;
            return true;
         }
         _comprovanteCompleto = null;
         return false;
      }

      /// <summary>
      /// Recupera o comprovante diferenciado do estabelecimento que fica 
      /// disponível em <see cref="ComprovanteDiferenciadoEstabelecimento"/><br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_RCPTMERCH<br/>
      /// </summary>
      /// <returns>
      /// true : Sucesso, o comprovante está disponível<br/>
      /// false: O comprovante não está disponível<br/>
      /// </returns>
      public static bool FluxoComprovanteDiferenciadoEstabelecimento()
      {
         Info info;
         if (FluxoGetResult(PWINFO.PWINFO_RCPTMERCH, out info))
         {
            _comprovanteDiferenciadoEstabelecimento = info.Value;
            return true;
         }
         _comprovanteDiferenciadoEstabelecimento = null;
         return false;
      }

      /// <summary>
      /// Recupera o comprovante diferenciado do cliente que fica 
      /// disponível em <see cref="ComprovanteDiferenciadoCliente"/><br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_RCPTCHOLDER<br/>
      /// </summary>
      /// <returns>
      /// true : Sucesso, o comprovante está disponível<br/>
      /// false: O comprovante não está disponível<br/>
      /// </returns>
      public static bool FluxoComprovanteDiferenciadoCliente()
      {
         Info info;
         if (FluxoGetResult(PWINFO.PWINFO_RCPTCHOLDER, out info))
         {
            _comprovanteDiferenciadoCliente = info.Value;
            return true;
         }
         _comprovanteDiferenciadoCliente = null;
         return false;
      }

      /// <summary>
      /// Recupera o comprovante reduzido do cliente que fica 
      /// disponível em <see cref="ComprovanteReduzidoCliente"/><br/>
      /// - PW_iGetResult:<br/>
      /// --- PWINFO_RCPTCHSHORT<br/>
      /// </summary>
      /// <returns>
      /// true : Sucesso, o comprovante está disponível<br/>
      /// false: O comprovante não está disponível<br/>
      /// </returns>
      public static bool FluxoComprovanteReduzidoCliente()
      {
         Info info;
         if (FluxoGetResult(PWINFO.PWINFO_RCPTCHSHORT, out info))
         {
            _comprovanteReduzidoCliente = info.Value;
            return true;
         }
         _comprovanteReduzidoCliente = null;
         return false;
      }

      #endregion

      #region Private Static Methods

      /// <summary>
      /// Menu (PWDAT_MENU)
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoMenu(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoMenu");

         Captura captura = null;
         Param param = ParamsFonteDadosGet(pw_GetData.wIdentificador);
         if (param != null)
         {
            Log.PrintThread("FluxoMenu - Parametro encontrado na Fonte de Dados, pulando evento...");
            captura = new Captura()
            {
               CapturaCancelada = false,
               Valor = param.Value
            };
         }

         if (captura == null)
            captura = CallCapturaMenuFunc(pw_GetData);

         if (captura.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         return FluxoAddParamCapturado(
            new Param(pw_GetData.wIdentificador, captura.Valor)
            );
      }

      /// <summary>
      /// Entrada digitada (PWDAT_TYPED)
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoTyped(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoTyped");

         Captura captura = null;
         Param param = ParamsFonteDadosGet(pw_GetData.wIdentificador);
         if (param != null)
         {
            Log.PrintThread("FluxoTyped - Parametro encontrado na Fonte de Dados, pulando evento...");
            captura = new Captura()
            {
               CapturaCancelada = false,
               Valor = param.Value
            };
         }

         if (captura == null)
            captura = CallCapturaDigitadaFunc(pw_GetData);

         if (captura.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         return FluxoAddParamCapturado(
            new Param(pw_GetData.wIdentificador, captura.Valor)
            );
      }

      /// <summary>
      /// Código de barras (PWDAT_BARCODE)
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoBarCode(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoBarCode");

         CapturaCodigoBarras capturaCodigoBarras = null;
         Param barCode = ParamsFonteDadosGet(PWINFO.PWINFO_BARCODE);
         if (barCode != null)
         {
            Param barCodEntMode = ParamsFonteDadosGet(PWINFO.PWINFO_BARCODENTMODE);
            if (barCodEntMode != null)
            {
               Log.PrintThread("FluxoBarCode - Parametro encontrado na Fonte de Dados, pulando evento...");
               capturaCodigoBarras = new CapturaCodigoBarras()
               {
                  CapturaCancelada = false,
                  BarCode = barCode.Value,
                  BarCodEntMode = barCodEntMode.Value
               };
            }
         }

         if (capturaCodigoBarras == null)
            capturaCodigoBarras = CallCapturaCodigoBarrasFunc(pw_GetData);

         if (capturaCodigoBarras.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         if (!FluxoAddParamCapturado(new Param(PWINFO.PWINFO_BARCODE, capturaCodigoBarras.BarCode)))
         {
            //Transacao Finalizada
            return false;
         }

         return FluxoAddParamCapturado(
            new Param(PWINFO.PWINFO_BARCODENTMODE, capturaCodigoBarras.BarCodEntMode)
            );
      }

      /// <summary>
      /// Dados do cartão (PWDAT_CARDINF)
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoCardInf(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoCardInf");

         Captura captura = null;
         Param cardFullPan = ParamsFonteDadosGet(PWINFO.PWINFO_CARDFULLPAN);
         if (cardFullPan != null)
         {
            Log.PrintThread("FluxoCardInf - Parametro encontrado na Fonte de Dados, pulando evento...");
            captura = new Captura()
            {
               CapturaCancelada = false,
               Valor = cardFullPan.Value
            };
         }

         if (captura == null)
         {
            //captura = RaiseCapturaEntradaDigitada(pw_GetData);
            captura = CallCapturaDigitadaFunc(new PW_GetData()
            {
               wIdentificador = (ushort)PWINFO.PWINFO_CARDFULLPAN,
               szPrompt = "\r   NUMERO CARTAO:\r",
               bTiposEntradaPermitidos = 1, //Somente numéricos
               bTamanhoMinimo = 13,
               bTamanhoMaximo = 19,
               bAceitaNulo = 0,
            });
         }

         if (captura.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         if (!FluxoAddParamCapturado(new Param(PWINFO.PWINFO_CARDFULLPAN, captura.Valor)))
         {
            //Transacao Finalizada
            return false;
         }

         //Se o campo bCapturarDataVencCartao for igual a 0, 
         //também solicitar a digitação da data de vencimento do cartão:
         //   - Com máscara “@@/@@”; 
         //   - Com validação “MMAA” (validação de data válida, não de vencimento); 
         if (pw_GetData.bCapturarDataVencCartao == 0)
         {
            captura = null;
            Param cardExpDate = ParamsFonteDadosGet(PWINFO.PWINFO_CARDEXPDATE);
            if (cardExpDate != null)
            {
               Log.PrintThread("FluxoCardInf - Parametro encontrado na Fonte de Dados, pulando evento...");
               captura = new Captura()
               {
                  CapturaCancelada = false,
                  Valor = cardExpDate.Value
               };
            }

            if (captura == null)
            {
               captura = CallCapturaDigitadaFunc(new PW_GetData()
               {
                  wIdentificador = (ushort)PWINFO.PWINFO_CARDEXPDATE,
                  szPrompt = "\rDATA DE VENCIMENTO:\r",
                  bTiposEntradaPermitidos = 1, //Somente numéricos
                  szMascaraDeCaptura = "@@/@@",
                  bValidacaoDado = 4, //Data no formato “MMAA”
                  szMsgValidacao = "\r    DATA INVALIDA"
               });
            }

            if (captura.CapturaCancelada)
            {
               //Transacao Finalizada
               return false;
            }

            if (!FluxoAddParamCapturado(new Param(PWINFO.PWINFO_CARDEXPDATE, captura.Valor)))
            {
               //Transacao Finalizada
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Senha (PWDAT_USERAUTH)
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoUserAuth(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoUserAuth");

         Captura captura = null;
         Param param = ParamsFonteDadosGet(pw_GetData.wIdentificador);
         if (param != null)
         {
            Log.PrintThread("FluxoUserAuth - Parametro encontrado na Fonte de Dados, pulando evento...");
            captura = new Captura()
            {
               CapturaCancelada = false,
               Valor = param.Value
            };
         }

         if (captura == null)
            captura = CallCapturaSenhaFunc(pw_GetData);

         if (captura.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         return FluxoAddParamCapturado(
            new Param(pw_GetData.wIdentificador, captura.Valor)
            );
      }

      /// <summary>
      /// Exibição de uma mensagem no checkout (PWDAT_DSPCHECKOUT)
      /// PWINFO_AUTCAP = 128: exibição de mensagem no checkout
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoDspCheckout(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoDspCheckout");

         CallDisplayCallback(pw_GetData.szPrompt);

         return FluxoAddParamCapturado(
            new Param(pw_GetData.wIdentificador, "")
            );
      }

      /// <summary>
      /// Exibição de QR Code (PWDAT_DSPQRCODE)
      /// PWINFO_AUTCAP = 256: exibição de QR Code no checkout
      /// </summary>
      /// <param name="pw_GetData"></param>
      /// <returns></returns>
      private static bool FluxoDspQRCode(PW_GetData pw_GetData)
      {
         Log.PrintThread("FluxoDspQRCode");

         //QR Code: recupera o conteúdo
         Info info;
         if (!FluxoGetResult(PWINFO.PWINFO_AUTHPOSQRCODE, out info))
         {
            return false;
         }

         Captura captura =
            CallCapturaQRCodeFunc(pw_GetData, info.Value);

         if (captura.CapturaCancelada)
         {
            //Transacao Finalizada
            return false;
         }

         return FluxoAddParamCapturado(
            new Param(pw_GetData.wIdentificador, "")
            );
      }

      /// <summary>
      /// Verifica se tem algum cartão setado, caso positivo:
      /// aborta o fluxo do pinpad e segue o fluxo de transação digitada
      /// </summary>
      private static void FluxoPinPadVerificaTrocaParaFluxoCardInf()
      {
         Param cardFullPan = ParamsFonteDadosGet(PWINFO.PWINFO_CARDFULLPAN);
         if (cardFullPan == null)
         {
            _verificaKeyDown = true;
         }
         else
         {
            _pinpadCancelarOperacao = true;
            _capturaCardInfFunc = FluxoCardInf;
         }
      }

      private static Param ParamsFonteDadosGet(ushort pwInfo)
      {
         PWINFO pwInfoEnum;
         Enum.TryParse(pwInfo.ToString(), out pwInfoEnum);
         return ParamsFonteDadosGet(pwInfoEnum);
      }

      private static Param ParamsFonteDadosGet(PWINFO pwInfo)
      {
         return _paramsFonteDados.FirstOrDefault(p => p.PwInfo.Equals(pwInfo));
      }

      public static Info ResultsEnviadosGet(PWINFO pwInfo)
      {
         Info info = _resultsEnviados.FirstOrDefault(r => r.PwInfo.Equals(pwInfo));
         if (info == null)
            FluxoGetResult(pwInfo, out info);
         return info;
      }

      public static string ResultsEnviadosToString(PWINFO pwInfo)
      {
         Info info = ResultsEnviadosGet(pwInfo);
         if (info == null)
            return null;

         if (info.Ret != (short)PWRET.PWRET_OK)
            return null;

         return info.Value;
      }

      public static string FormatarComprovante(string comprovante)
      {
         if (string.IsNullOrEmpty(comprovante))
            return null;

         StringBuilder comprovanteFormatado = new StringBuilder();

         string[] linhas = comprovante.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

         var linhaMaxLength = linhas.Aggregate((max, cur) => max.Length > cur.Length ? max : cur).Length;

         string separador =
            string.Format("+{0}+", new string('-', linhaMaxLength + 2));

         string linha =
            string.Format("|{0}|", new string(' ', linhaMaxLength + 2));

         comprovanteFormatado.AppendLine(separador);
         comprovanteFormatado.AppendLine(linha);
         for (int i = 0; i < linhas.Length; i++)
            comprovanteFormatado.AppendLine(string.Format("| {0} |", linhas[i].PadRight(linhaMaxLength, ' ')));
         comprovanteFormatado.AppendLine(linha);
         comprovanteFormatado.AppendLine(separador);

         return comprovanteFormatado.ToString();
      }

      #endregion

   }

}