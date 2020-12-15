using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Muxx.Lib.Helpers;
using Muxx.Lib.ValueObjects.Enums;
using Muxx.Lib.ValueObjects.Structs;
using Newtonsoft.Json;

namespace Muxx.Lib.Services
{

   public static class PGWebLib
   {
      #region Delegates

      private static Action<string> _beginCallPGWebLibCallback;
      private static Action<string, long> _endCallPGWebLibCallback;
      private static Action<string> _debugCallback;

      /// <summary>
      /// Callback feito antes de chamar PW_ixxx.
      /// </summary>
      public static Action<string> BeginCallPGWebLibCallback
      {
         get { return _beginCallPGWebLibCallback; }
         set { _beginCallPGWebLibCallback = value; }
      }

      /// <summary>
      /// Callback feito depois de PW_ixxx retornar.
      /// </summary>
      public static Action<string, long> EndCallPGWebLibCallback
      {
         get { return _endCallPGWebLibCallback; }
         set { _endCallPGWebLibCallback = value; }
      }

      /// <summary>
      /// Callback com as mensagens de debug.
      /// </summary>
      /// <param name="mensagens"></param>
      public static Action<string> DebugCallback
      {
         get { return _debugCallback; }
         set { _debugCallback = value; }
      }

      private static Stopwatch CallBeginCallPGWebLibCallback(params string[] mensagens)
      {
         if (_beginCallPGWebLibCallback != null)
         {
            _beginCallPGWebLibCallback(string.Join(Environment.NewLine, mensagens));
         }
         return Stopwatch.StartNew();
      }

      private static long CallEndCallPGWebLibCallback(Stopwatch stopwatch, params string[] mensagens)
      {
         long elapsedMilliseconds = 0;

         if (stopwatch != null)
         {
            stopwatch.Stop();
            elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
         }

         if (_endCallPGWebLibCallback != null)
         {
            _endCallPGWebLibCallback(string.Join(Environment.NewLine, mensagens), elapsedMilliseconds);
         }

         return elapsedMilliseconds;
      }

      private static void CallDebugCallback(params string[] mensagens)
      {
         Log.PrintThread(mensagens);

         //Evento
         if (_debugCallback != null)
         {
            _debugCallback(string.Join(Environment.NewLine, mensagens));
         }
      }

      private static void CallDebugCallback(string title, object json)
      {
         string log = string.Format("{0} {1}", title, JsonConvert.SerializeObject(json));

         Log.PrintThread(log);

         //Evento
         if (_debugCallback != null)
         {
            _debugCallback(log);
         }
      }
      #endregion

      #region Private Attributes

      #endregion

      //Como caractere separador de diretório (DirectorySeparatorChar), 
      //foi usado o caractere de barra "/". 
      //É o único caractere separador de diretório reconhecido em sistemas UNIX, 
      //e é o caractere alternativo (AltDirectorySeparatorChar) no Windows.
      private const string PGWEBCERTIFICATEADDRESS = @"./Certificado.crt";
      private const string PGWEBLIBADDRESS = @"./PGWebLib/PGWebLib";
      private const short NUMPARAM = 9;

      public const string SENHALOGISTA = "1111";
      public const string SENHATECNICA = "314159";

      #region Public Properties

      /// <summary>
      /// Tipo das mensagens de debug.
      /// </summary>
      public static DebugType DebugType
      {
         get;
         set;
      }

      #endregion

      #region Constructors

      static PGWebLib()
      {
         DebugType = DebugType.Default;
      }

      #endregion

      #region Interop

      /*=========================================================================================================*\
       Funcao     :  PW_iInit

       Descricao  :  Esta função é utilizada para inicializar a biblioteca, e retorna imediatamente. Deve ser 
                     garantido que uma chamada dela retorne PWRET_OK antes de chamar qualquer outra função.
 
       Entradas   :  pszWorkingDir:    Diretório de trabalho (caminho completo, com final nulo) para uso exclusivo 
                                       do Pay&Go Web.

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Operação bem sucedida.
                     PWRET_WRITERR	   Falha de gravação no diretório informado.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iInit", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iInit_(string pszWorkingDir);

      /*=========================================================================================================*\
       Funcao     :  PW_iNewTransac

       Descricao  :  Esta função deve ser chamada para iniciar uma nova transação através do Pay&Go Web, 
                     e retorna imediatamente.

                     Importante: independentemente das funcionalidades suportadas pela Automação e pelo Ponto de 
                     Captura, é requerido que a Automação disponibilize ao operador uma função para realizar uma 
                     transação administrativa (PWOPER_ADMIN), para permitir o acesso às funções de manutenção do 
                     Pay&Go Web. Caso desejado, o acesso a este recurso pode ser restrito a operadores específicos.
 
       Entradas   :  bOper:	Tipo de operação sendo efetuada (constantes PWOPER_xxx):
                              1:  Pagamento 
                              2:  Administrativa 

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Transação inicializada.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iNewTransac", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iNewTransac_(byte bOper);

      /*=========================================================================================================*\
       Funcao     :  PW_iAddParam

       Descricao  :  Esta função é utilizada para alimentar a biblioteca com as informações da transação a ser 
                     realizada, e retorna imediatamente. Estas informações podem ser:
                        •	Pré-fixadas na Automação;
                        •	Capturadas do operador pela Automação antes do acionamento do Pay&Go Web;
                        •	Capturadas do operador após solicitação pelo Pay&Go Web (retorno PW_MOREDATA por PW_iExecTransac).

       Entradas   :  wParam:	      Identificador do parâmetro.
                     pszValue:	   Valor do parâmetro informado.

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Parâmetro acrescentado com sucesso.
                     PWRET_INVPARAM	   O valor do parâmetro é inválido.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_TRNNOTINIT	Não foi executado PW_iNewTransac (ver página 10).
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iAddParam", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iAddParam_(ushort wParam, string pszValue);

      /*=========================================================================================================*\
       Funcao     :  PW_iExecTransac

       Descricao  :  Esta função tenta realizar uma transação através do Pay&Go Web, utilizando os parâmetros 
                     previamente definidos através de PW_iAddParam. Caso algum dado adicional precise ser informado, 
                     o retorno será PWRET_MOREDATA e o parâmetro pvstParam retornará informações dos dados que ainda 
                     devem ser capturados.
                     Esta função, por se comunicar com a infraestrutura Pay&Go Web, pode demorar alguns segundos 
                     para retornar.
 
       Entradas   :  piNumParam: 	Quantidade máxima de dados que podem ser capturados de uma vez, caso o retorno 
                                    seja PW_MOREDATA. (Deve refletir o tamanho da área de memória apontada por 
                                    pvstParam.) Valor sugerido: 9.
 
       Saidas     :  pvstParam: 	   Lista e características dos dados que precisam ser informados para executar a 
                                    transação. Consultar “8.Captura de dados” (página 29) para a descrição da estrutura 
                                    e instruções para a captura de dados adicionais.
                     piNumParam:	   Quantidade de dados adicionais que precisam ser capturados (quantidade de ocorrências 
                                    preenchidas em pvstParam).

       Retorno    :  PWRET_OK	         Transação realizada com sucesso. Os resultados da transação devem ser obtidos através da função PW_iGetResult.
                     PWRET_MOREDATA	   Mais dados são requeridos para executar a transação.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_TRNNOTINIT	Não foi executado PW_iNewTransac (ver página 10).
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iExecTransac", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iExecTransac_([Out] PW_GetData[] vstParam, ref short piNumParam);

      /*=========================================================================================================*\
       Funcao     :  PW_iGetResult

       Descricao  :  Esta função pode ser chamada para obter informações que resultaram da transação efetuada, 
                     independentemente de ter sido bem ou mal sucedida, e retorna imediatamente.
 
       Entradas   :  iInfo:	   Código da informação solicitada sendo requisitada (PWINFO_xxx, ver lista completa 
                                 em “9. Dicionário de dados”, página 36).
                     ulDataSize:	Tamanho (em bytes) da área de memória apontada por pszData. Prever um tamanho maior 
                                 que o máximo previsto para o dado solicitado.

 
       Saidas     :  pszData:	   Valor da informação solicitada (string ASCII com terminador nulo).
 
       Retorno    :  PWRET_OK	         Sucesso. pszData contém o valor solicitado.
                     PWRET_NODATA	   A informação solicitada não está disponível.
                     PWRET_BUFOVFLW 	O valor da informação solicitada não cabe em pszData.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_TRNNOTINIT	Não foi executado PW_iNewTransac (ver página 10).
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iGetResult", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iGetResult_(short iInfo, [Out] StringBuilder pszData, uint ulDataSize);

      /*=========================================================================================================*\
       Funcao     :  PW_iConfirmation

       Descricao  :  Esta função informa ao Pay&Go Web o status final da transação em curso (confirmada ou desfeita). 
                     Consultar “7. Confirmação de transação” (página 28) para informações adicionais.
 
       Entradas   :  ulStatus:   	Resultado da transação (PWCNF_xxx, ver lista abaixo).
                     pszReqNum:  	Referência local da transação, obtida através de PW_iGetResult (PWINFO_REQNUM).
                     pszLocRef:  	Referência da transação para a infraestrutura Pay&Go Web, obtida através de PW_iGetResult (PWINFO_AUTLOCREF). 
                     pszExtRef:  	Referência da transação para o Provedor, obtida através de PW_iGetResult (PWINFO_AUTEXTREF).
                     pszVirtMerch:	Identificador do Estabelecimento, obtido através de PW_iGetResult (PWINFO_VIRTMERCH). 
                     pszAuthSyst:   Nome do Provedor, obtido através de PW_iGetResult (PWINFO_AUTHSYST).
 
       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         O status da transação foi atualizado com sucesso.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iConfirmation", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iConfirmation_(uint ulResult, string pszReqNum, string pszLocRef, string pszExtRef, string pszVirtMerch, string pszAuthSyst);

      /*=========================================================================================================*\
       Funcao     :  PW_iIdleProc

       Descricao  :  Para o correto funcionamento do sistema, a biblioteca do Pay&Go Web precisa de tempos em tempos 
                     executar tarefas automáticas enquanto não está realizando nenhuma transação a pedido da Automação. 

       Entradas   :  não há.

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iIdleProc", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iIdleProc_();

      /*=========================================================================================================*\
       Funcao     :  PW_iGetOperations

       Descricao  :  Esta função pode ser chamada para obter quais operações o Pay&Go WEB disponibiliza no momento, 
                     sejam elas administrativas, de venda ou ambas. 

       Entradas   :              bOperType	      Soma dos tipos de operação a serem incluídos na estrutura de 
                                                   retorno (PWOPTYPE_xxx).	
                                 piNumOperations	Número máximo de operações que pode ser retornado. (Deve refletir 
                                                   o tamanho da área de memória apontada por pvstOperations).
 
       Saídas     :              piNumOperations	Número de operações disponíveis no Pay&Go WEB.
                                 vstOperations	   Lista das operações disponíveis e suas características.

 
       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iGetOperations", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iGetOperations_(byte bOperType, ref PW_Operations[] vstOperations, ref short piNumOperations);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPEventLoop

       Descricao  :  Esta função deverá ser chamada em “loop” até que seja retornado PWRET_OK (ou um erro fatal). Nesse 
                     “loop”, caso o retorno seja PWRET_DISPLAY o ponto de captura deverá atualizar o “display” com as 
                     mensagens recebidas da biblioteca.
 
       Entradas   :  ulDisplaySize	Tamanho (em bytes) da área de memória apontada por pszDisplay. 
                                    Tamanho mínimo recomendado: 100 bytes.

       Saidas     :  pszDisplay	   Caso o retorno da função seja PWRET_DISPLAY, contém uma mensagem de texto 
                                    (string ASCII com terminal nulo) a ser apresentada pela Automação na interface com 
                                    o usuário principal. Para o formato desta mensagem, consultar “4.3.Interface com o 
                                    usuário”, página 8.
 
       Retorno    :  PWRET_NOTHING	   Nada a fazer, continuar aguardando o processamento do PIN-pad.
                     PWRET_DISPLAY	   Apresentar a mensagem recebida em pszDisplay e continuar aguardando o processamento do PIN-pad.
                     PWRET_OK	         Captura de dados realizada com êxito, prosseguir com a transação.
                     PWRET_CANCEL	   A operação foi cancelada pelo Cliente no PIN-pad (tecla [CANCEL]).
                     PWRET_TIMEOUT	   O Cliente não realizou a captura no tempo limite.
                     PWRET_FALLBACK	   Ocorreu um erro na leitura do cartão, passar a aceitar a digitação do número do cartão, caso já não esteja aceitando.
                     PWRET_PPCOMERR	   Falha na comunicação com o PIN-pad.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_INVCALL	   Não há captura de dados no PIN-pad em curso.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPEventLoop", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPEventLoop_([Out] StringBuilder pszDisplay, uint ulDisplaySize);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPAbort

       Descricao  :  Esta função pode ser utilizada pela Automação para interromper uma captura de dados no PIN-pad 
                     em curso, e retorna imediatamente.
 
       Entradas   :  não há.

       Saidas     :  não há. 
 
       Retorno    :  PWRET_OK	         Operação interrompida com sucesso.
                     PWRET_PPCOMERR	   Falha na comunicação com o PIN-pad.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPAbort", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPAbort_();

      /*=========================================================================================================*\
       Funcao     :  PW_iPPGetCard

       Descricao  :  Esta função é utilizada para realizar a leitura de um cartão (magnético, com chip com contato, 
                     ou sem contato) no PIN-pad.
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGetCard", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGetCard_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPGetPIN

       Descricao  :  Esta função é utilizada para realizar a captura no PIN-pad da senha (ou outro dado criptografado) 
                     do Cliente.
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).
   
       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGetPIN", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGetPIN_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPGetData

       Descricao  :  Esta função é utilizada para fazer a captura no PIN-pad de um dado não sensível do Cliente..
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  nao ha.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGetData", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGetData_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPGoOnChip

       Descricao  :  Esta função é utilizada para realizar o processamento off-line (antes da comunicação com o Provedor) 
                     de um cartão com chip no PIN-pad. 
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGoOnChip", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGoOnChip_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPFinishChip

       Descricao  :  Esta função é utilizada para finalizar o processamento on-line (após comunicação com o Provedor) 
                     de um cartão com chip no PIN-pad.
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPFinishChip", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPFinishChip_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPConfirmData

       Descricao  :  Esta função é utilizada para obter do Cliente a confirmação de uma informação no PIN-pad.
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPConfirmData", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPConfirmData_(ushort uiIndex);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPRemoveCard

       Descricao  :  Esta função é utilizada para fazer uma remoção de cartão do PIN-pad.
 
       Entradas   :  não há.

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_INVPARAM	   O valor de uiIndex informado não corresponde a uma captura de dados deste tipo.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPRemoveCard", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPRemoveCard_();

      /*=========================================================================================================*\
       Funcao     :  PW_iPPDisplay

       Descricao  :  Esta função é utilizada para apresentar uma mensagem no PIN-pad
 
       Entradas   :  pszMsg   Mensagem a ser apresentada no PIN-pad. O caractere ‘\r’ (0Dh) indica uma quebra de linha.

       Saidas     :  não há.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPDisplay", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPDisplay_(string pszMsg);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPWaitEvent

       Descricao  :  Esta função é utilizada para aguardar a ocorrência de um evento no PIN-pad.
 
       Entradas   :  não há.

       Saidas     :  pulEvent	         Evento ocorrido.
 
       Retorno    :  PWRET_OK	         Captura iniciada com sucesso, chamar PW_iPPEventLoop para obter o resultado.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPWaitEvent", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPWaitEvent_(ref uint pulEvent);

      /*===========================================================================*\
       Funcao   : PW_iPPGenericCMD

       Descricao  :  Realiza comando genérico de PIN-pad.
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  Não há.
 
       Retorno    :  PWRET_xxx.
      \*===========================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGenericCMD", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGenericCMD_(ushort uiIndex);

      /*===========================================================================*\
       Funcao     : PW_iPPPositiveConfirmation

       Descricao  :  Realiza a confirmação positiva de um dado, ou um bloco de dados,
                      no PIN-pad
 
       Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                              (índice do dado no vetor pvstParam).

       Saidas     :  Não há.
 
       Retorno    :  PWRET_xxx.
      \*===========================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPPositiveConfirmation", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPPositiveConfirmation_(ushort uiIndex);

      /*===========================================================================*\
       Funcao     : PW_iTransactionInquiry

       Descricao  :  Esta função é utilizada para realizar uma consulta de transações 
                     efetuadas por um ponto de captura junto ao Pay&Go WEB.

       Entradas   :  pszXmlRequest	Arquivo de entrada no formato XML, contendo as informações 
                                    necessárias para fazer a consulta pretendida.
                     ulXmlResponseLen Tamanho da string pszXmlResponse.

       Saidas     :  pszXmlResponse	Arquivo de saída no formato XML, contendo o resultado da consulta 
                                    efetuada, o arquivo de saída tem todos os elementos do arquivo de entrada.

       Retorno    :  PWRET_xxx.
      \*===========================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iTransactionInquiry", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iTransactionInquiry_(byte pszXmlRequest, byte pszXmlResponse, uint ulXmlResponseLen);

      /*=========================================================================================================*\
       Funcao     :  PW_iGetUserData

       Descricao  :  Esta função é utilizada para obter um dado digitado pelo portador do cartão no PIN-pad.

       Entradas   :  uiMessageId : Identificador da mensagem a ser exibida como prompt para a captura.
                     bMinLen     : Tamanho mínimo do dado a ser digitado.
                     bMaxLen     : Tamanho máximo do dado a ser digitado.
                     iToutSec    : Tempo limite para a digitação do dado em segundos.
 
       Saídas     :  pszData     : Dado digitado pelo portador do cartão no PIN-pad.
 
       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     PWRET_CANCEL	   A operação foi cancelada pelo Cliente no PIN-pad (tecla [CANCEL]).
                     PWRET_TIMEOUT	   O Cliente não realizou a captura no tempo limite.
                     PWRET_PPCOMERR	   Falha na comunicação com o PIN-pad.
                     PWRET_INVCALL	   Não é possível capturar dados em um PIN-pad não ABECS.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                       de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGetUserData", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGetUserData_(short uiMessageId, short bMinLen, short bMaxLen, short iToutSec, StringBuilder pszData);

      /*=========================================================================================================*\
       Funcao     :  PW_iPPGetPINBlock

       Descricao  :  Esta função é utilizada para obter o PIN block gerado a partir de um dado digitado pelo usuário no PIN-pad.

       Entradas   :  bKeyID	      : Índice da Master Key (para chave PayGo, utilizar o índice “12”).
                     pszWorkingKey	: Sequência 32 caracteres utilizados para a geração do PIN block (dois valores iguais digitados pelo usuário com duas pszWorkingKey diferentes irão gerar dois PIN block diferentes.
                     bMinLen	      : Tamanho mínimo do dado a ser digitado (a partir de 4).
                     bMaxLen     	: Tamanho máximo do dado a ser digitado.
                     iToutSec    	: Tempo limite para a digitação do dado em segundos.
                     pszPrompt	   : Mensagem de 32 caracteres (2 linhas com 16 colunas) para apresentação no momento do pedido do dado do usuário.


       Saídas     :  pszData        : PIN block gerado com base nos dados fornecidos na função combinados com o dado digitado pelo usuário no PIN-pad.

       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     PWRET_CANCEL	   A operação foi cancelada pelo Cliente no PIN-pad (tecla [CANCEL]).
                     PWRET_TIMEOUT	   O Cliente não realizou a captura no tempo limite.
                     PWRET_PPCOMERR	   Falha na comunicação com o PIN-pad.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPGetPINBlock", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPGetPINBlock_(short bKeyID, string pszWorkingKey, short bMinLen,
         short bMaxLen, short iToutSec, string pszPrompt, StringBuilder pszData);

      /*=========================================================================================================*\
       Funcao     :  PW_iWaitConfirmation

       Descricao  :  Esta função é utilizada sincronizar a aplicação com a thread da confirmação.
                     Esta função apenas retorna quando o processo de confirmação é finalizado.

       Entradas   :  Não há.

       Saídas     :  Não há.

       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iWaitConfirmation", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iWaitConfirmation_();

      /*=========================================================================================================*\
       Funcao     :  PW_iPPTestKey

       Descricao  :  Esta função é utilizada para iniciar a captura de uma chave de PIN do teste de chaves.
                     Deve ser chamada em resposta a uma captura de dados do tipo PWDAT_TSTKEY.


       Entradas   :  uiIndex.

       Saídas     :  Não há.

       Retorno    :  PWRET_OK	         Operação realizada com êxito.
                     PWRET_DLLNOTINIT	Não foi executado PW_iInit.
                     PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
                     Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40).
      \*=========================================================================================================*/
      [DllImport(PGWEBLIBADDRESS, EntryPoint = "PW_iPPTestKey", CallingConvention = CallingConvention.StdCall)]
      private static extern short PW_iPPTestKey_(ushort uiIndex);

      #endregion Interop

      #region Public Static Methods

      public static string PwOperToString(byte oper)
      {
         PWOPER pwoper;
         if (Enum.TryParse<PWOPER>(oper.ToString(), out pwoper))
            return pwoper.ToString();
         return oper.ToString();
      }

      public static string PwInfoToString(ushort info)
      {
         return PwInfoToString((int)info);
      }

      public static string PwInfoToString(int info)
      {
         PWINFO pwinfo;
         if (Enum.TryParse<PWINFO>(info.ToString(), out pwinfo))
            return pwinfo.ToString();
         return info.ToString();
      }

      public static string PwRetToString(short ret)
      {
         PWRET pwret;
         if (Enum.TryParse<PWRET>(ret.ToString(), out pwret))
            return pwret.ToString();
         return ret.ToString();
      }

      public static string PwDatToString(short dat)
      {
         PWDAT pwdat;
         if (Enum.TryParse<PWDAT>(dat.ToString(), out pwdat))
            return pwdat.ToString();
         return dat.ToString();
      }

      ///<inheritdoc cref="PW_iInit(string)"/>
      public static short PW_iInit()
      {
         string pszWorkingDir = Path.GetDirectoryName(PGWEBLIBADDRESS);
         return PW_iInit(pszWorkingDir);
      }

      /// <summary>
      /// Wrapper para PW_iInit.
      /// </summary>
      /// <param name="pszWorkingDir"></param>
      /// <returns></returns>
      public static short PW_iInit(string pszWorkingDir)
      {
         string resourceName;
         string pgwebLibAddress;
#if NETCOREAPP
         if (Helpers.OperatingSystem.IsWindows())
         {
#endif
         resourceName = Environment.Is64BitProcess ? "PGWebLib64.dll" : "PGWebLib32.dll";
         pgwebLibAddress = string.Format("{0}.dll", PGWEBLIBADDRESS);
#if NETCOREAPP
         }
         else if (Helpers.OperatingSystem.IsLinux() && Environment.Is64BitProcess)
         {
            resourceName = "PGWebLib64.so";
            pgwebLibAddress = string.Format("{0}.so", PGWEBLIBADDRESS);
         }
         else
         {
            throw new PlatformNotSupportedException();
         }
#endif
         SaveResourceToFile(resourceName, pgwebLibAddress);

         try
         {
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(pgwebLibAddress);

            if (DebugType == DebugType.Json)
            {
               CallDebugCallback(
                     "PGWebLib()",
                     new
                     {
                        File = pgwebLibAddress,
                        FileVersion = fileVersionInfo.FileVersion
                     }
                  );
            }
            else
            {
               CallDebugCallback(
                  "PGWebLib",
                  string.Format("\t{0} = \"{1}\"", "File", pgwebLibAddress),
                  string.Format("\t{0} = \"{1}\"", "File Version", fileVersionInfo.FileVersion)
               );
            }
         }
         catch (Exception)
         {
         }

         try
         {
            if (DebugType == DebugType.Default)
            {
               CallDebugCallback(
                  "PW_iInit",
                  string.Format("\t{0} = {1}", "pszWorkingDir", pszWorkingDir)
                  );
            }

            Stopwatch stopwatch =
               CallBeginCallPGWebLibCallback(
                  "PW_iInit"
               );

            short ret = PW_iInit_(pszWorkingDir);

            long elapsedMilliseconds =
               CallEndCallPGWebLibCallback(
                  stopwatch,
                  "PW_iInit"
                  );

            if (DebugType == DebugType.Json)
            {
               CallDebugCallback(
                  "PW_iInit()",
                  new
                  {
                     pszWorkingDir = pszWorkingDir,
                     RET = PwRetToString(ret),
                     ElapsedMilliseconds = elapsedMilliseconds
                  }
               );
            }
            else
            {
               CallDebugCallback(
                  string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
                  string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
                  );
            }

            return ret;
         }
         catch (BadImageFormatException ex)
         {
            File.Delete(pgwebLibAddress);
            throw ex;
         }
      }

      /// <summary>
      /// Wrapper para PW_iNewTransac.
      /// </summary>
      /// <param name="bOper"></param>
      /// <returns></returns>
      public static short PW_iNewTransac(PWOPER bOper)
      {
         return PW_iNewTransac((byte)bOper);
      }

      /// <summary>
      /// <inheritdoc cref="PW_iNewTransac(PWOPER)"/>
      /// </summary>
      /// <param name="bOper"></param>
      /// <returns></returns>
      public static short PW_iNewTransac(byte bOper)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iNewTransac",
               string.Format("\t{0} = {1}", "bOper", PwOperToString(bOper))
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iNewTransac"
               );

         short ret = PW_iNewTransac_(bOper);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iNewTransac"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iNewTransac()",
               new
               {
                  bOper = PwOperToString(bOper),
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// <inheritdoc cref="PW_iAddParam(ushort, string)"/>
      /// </summary>
      /// <param name="wParam"></param>
      /// <param name="pszValue"></param>
      /// <returns></returns>
      public static short PW_iAddParam(PWINFO wParam, string pszValue)
      {
         return PW_iAddParam((ushort)wParam, pszValue);
      }

      /// <summary>
      /// Wrapper para PW_iAddParam.
      /// </summary>
      /// <param name="wParam"></param>
      /// <param name="pszValue"></param>
      /// <returns></returns>
      public static short PW_iAddParam(ushort wParam, string pszValue)
      {
         if (pszValue == null)
            pszValue = "";

         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iAddParam",
               string.Format("\t{0} = {1}", PwInfoToString(wParam), pszValue)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iAddParam"
               );

         short ret = PW_iAddParam_(wParam, pszValue);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iAddParam"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iAddParam()",
               new
               {
                  Param = PwInfoToString(wParam),
                  Value = pszValue,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iExecTransac.
      /// </summary>
      /// <param name="vstParam"></param>
      /// <param name="piNumParam"></param>
      /// <returns></returns>
      public static short PW_iExecTransac(PW_GetData[] vstParam, ref short piNumParam)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iExecTransac"
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iExecTransac"
               );

         short ret = PW_iExecTransac_(vstParam, ref piNumParam);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iExecTransac"
               );

         if (ret == (short)PWRET.PWRET_MOREDATA)
         {
            if (DebugType == DebugType.Json)
            {
               CallDebugCallback(
                  "PW_iExecTransac()",
                  new
                  {
                     vstParam = StructToString<PW_GetData>(vstParam, piNumParam),
                     piNumParam = piNumParam,
                     RET = PwRetToString(ret),
                     ElapsedMilliseconds = elapsedMilliseconds
                  }
               );
            }
            else
            {
               CallDebugCallback(
                  StructToString<PW_GetData>(vstParam, piNumParam),
                  string.Format("\t{0} = {1}", "piNumParam", piNumParam)
                  );
            }
         }
         else if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iExecTransac()",
               new
               {
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }

         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// <inheritdoc cref="PW_iGetResult(short, StringBuilder, uint)"/>
      /// </summary>
      /// <param name="iInfo"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      public static short PW_iGetResult(short iInfo, out string data)
      {
         uint ulDataSize = 10000;
         StringBuilder pszData = new StringBuilder((int)ulDataSize);

         short ret = PW_iGetResult(iInfo, pszData, ulDataSize);

         data = pszData.ToString();

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iGetResult.
      /// </summary>
      /// <param name="iInfo"></param>
      /// <param name="pszData"></param>
      /// <param name="ulDataSize"></param>
      /// <returns></returns>
      public static short PW_iGetResult(short iInfo, StringBuilder pszData, uint ulDataSize)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iGetResult",
               string.Format("\t{0} = {1}", "iInfo", PwInfoToString((ushort)iInfo))
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iGetResult"
               );

         short ret = PW_iGetResult_(iInfo, pszData, ulDataSize);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iGetResult"
               );

         if (ret == (short)PWRET.PWRET_OK)
         {
            if (DebugType == DebugType.Json)
            {
               CallDebugCallback(
                  "PW_iGetResult()",
                  new
                  {
                     iInfo = PwInfoToString((ushort)iInfo),
                     pszData = TrataQuebraLinha(pszData.ToString()),
                     RET = PwRetToString(ret),
                     ElapsedMilliseconds = elapsedMilliseconds
                  }
               );
            }
            else
            {
               CallDebugCallback(
                  string.Format("\t{0} = {1}", "pszData", TrataQuebraLinha(pszData.ToString())),
                  string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
                  string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
                  );
            }
         }
         else
         {
            if (DebugType == DebugType.Json)
            {
               CallDebugCallback(
                  "PW_iGetResult()",
                  new
                  {
                     iInfo = PwInfoToString((ushort)iInfo),
                     RET = PwRetToString(ret),
                     ElapsedMilliseconds = elapsedMilliseconds
                  }
               );
            }
            else
            {
               CallDebugCallback(
                  string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
                  string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
                  );
            }
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iConfirmation.
      /// </summary>
      /// <param name="ulResult"></param>
      /// <param name="pszReqNum"></param>
      /// <param name="pszLocRef"></param>
      /// <param name="pszExtRef"></param>
      /// <param name="pszVirtMerch"></param>
      /// <param name="pszAuthSyst"></param>
      /// <returns></returns>
      public static short PW_iConfirmation(uint ulResult, string pszReqNum, string pszLocRef, string pszExtRef, string pszVirtMerch, string pszAuthSyst)
      {
         if (pszReqNum == null)
            pszReqNum = "";

         if (pszLocRef == null)
            pszLocRef = "";

         if (pszExtRef == null)
            pszExtRef = "";

         if (pszVirtMerch == null)
            pszVirtMerch = "";

         if (pszAuthSyst == null)
            pszAuthSyst = "";

         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iConfirmation",
               string.Format("\t{0} = {1}", "ulResult", ulResult),
               string.Format("\t{0} = {1}", "pszReqNum", pszReqNum),
               string.Format("\t{0} = {1}", "pszLocRef", pszLocRef),
               string.Format("\t{0} = {1}", "pszExtRef", pszExtRef),
               string.Format("\t{0} = {1}", "pszVirtMerch", pszVirtMerch),
               string.Format("\t{0} = {1}", "pszAuthSyst", pszAuthSyst)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iConfirmation"
               );

         short ret = PW_iConfirmation_(ulResult, pszReqNum, pszLocRef, pszExtRef, pszVirtMerch, pszAuthSyst);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iConfirmation"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iConfirmation()",
               new
               {
                  ulResult = ulResult,
                  pszReqNum = pszReqNum,
                  pszLocRef = pszLocRef,
                  pszExtRef = pszExtRef,
                  pszVirtMerch = pszVirtMerch,
                  pszAuthSyst = pszAuthSyst,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iIdleProc.
      /// </summary>
      /// <returns></returns>
      public static short PW_iIdleProc()
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iIdleProc"
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iIdleProc"
               );

         short ret = PW_iIdleProc_();

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iIdleProc"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iIdleProc()",
               new
               {
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iGetOperations.
      /// </summary>
      /// <param name="bOperType"></param>
      /// <param name="vstOperations"></param>
      /// <param name="piNumOperations"></param>
      /// <returns></returns>
      public static short PW_iGetOperations(byte bOperType, ref PW_Operations[] vstOperations, ref short piNumOperations)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iGetOperations",
               string.Format("\t{0} = {1}", "bOperType", bOperType)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iGetOperations"
               );

         short ret = PW_iGetOperations_(bOperType, ref vstOperations, ref piNumOperations);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iGetOperations"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iGetOperations()",
               new
               {
                  bOperType = bOperType,
                  vstOperations = StructToString<PW_Operations>(vstOperations, piNumOperations),
                  piNumOperations = piNumOperations,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               StructToString<PW_Operations>(vstOperations, piNumOperations),
               string.Format("\t{0} = {1}", "piNumOperations", piNumOperations),
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPEventLoop.
      /// </summary>
      /// <param name="pszDisplay"></param>
      /// <param name="ulDisplaySize"></param>
      /// <returns></returns>
      public static short PW_iPPEventLoop(StringBuilder pszDisplay, uint ulDisplaySize)
      {
         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPEventLoop"
               );

         short ret = PW_iPPEventLoop_(pszDisplay, ulDisplaySize);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPEventLoop"
               );

         if (ret != (short)PWRET.PWRET_NOTHING)
         {
            if (DebugType == DebugType.Default)
            {
               CallDebugCallback(
                  "PW_iPPEventLoop"
                  );
            }

            if (ret == (short)PWRET.PWRET_DISPLAY)
            {
               if (DebugType == DebugType.Json)
               {
                  CallDebugCallback(
                     "PW_iPPEventLoop()",
                     new
                     {
                        pszDisplay = TrataQuebraLinha(pszDisplay.ToString()),
                        RET = PwRetToString(ret),
                        ElapsedMilliseconds = elapsedMilliseconds
                     }
                  );
               }
               else
               {
                  CallDebugCallback(
                     string.Format("\t{0} = {1}", "pszDisplay", TrataQuebraLinha(pszDisplay.ToString())),
                     string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
                     string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
                     );
               }
            }
            else
            {
               if (DebugType == DebugType.Json)
               {
                  CallDebugCallback(
                     "PW_iPPEventLoop()",
                     new
                     {
                        RET = PwRetToString(ret),
                        ElapsedMilliseconds = elapsedMilliseconds
                     }
                  );
               }
               else
               {
                  CallDebugCallback(
                     string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
                     string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
                     );
               }
            }
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPAbort.
      /// </summary>
      /// <returns></returns>
      public static short PW_iPPAbort()
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPAbort"
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPAbort"
               );

         short ret = PW_iPPAbort_();

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPAbort"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPAbort()",
               new
               {
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPGetCard.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPGetCard(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPGetCard",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPGetCard"
               );

         short ret = PW_iPPGetCard_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPGetCard"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPGetCard()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPGetPIN.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPGetPIN(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPGetPIN",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPGetPIN"
               );

         short ret = PW_iPPGetPIN_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPGetPIN"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPGetPIN()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPGetData.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPGetData(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPGetData",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPGetData"
               );

         short ret = PW_iPPGetData_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPGetData"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPGetData()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPGoOnChip.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPGoOnChip(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPGoOnChip",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPGoOnChip"
               );

         short ret = PW_iPPGoOnChip_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPGoOnChip"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPGoOnChip()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPFinishChip.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPFinishChip(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPFinishChip",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPFinishChip"
               );

         short ret = PW_iPPFinishChip_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPFinishChip"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPFinishChip()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPConfirmData.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPConfirmData(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPConfirmData",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPConfirmData"
               );

         short ret = PW_iPPConfirmData_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPConfirmData"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPConfirmData()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPRemoveCard.
      /// </summary>
      /// <returns></returns>
      public static short PW_iPPRemoveCard()
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPRemoveCard"
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPRemoveCard"
               );

         short ret = PW_iPPRemoveCard_();

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPRemoveCard"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPRemoveCard()",
               new
               {
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPDisplay.
      /// </summary>
      /// <param name="pszMsg"></param>
      /// <returns></returns>
      public static short PW_iPPDisplay(string pszMsg)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPDisplay",
               string.Format("\t{0} = {1}", "pszMsg", TrataQuebraLinha(pszMsg))
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPDisplay"
               );

         short ret = PW_iPPDisplay_(pszMsg);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPDisplay"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPDisplay()",
               new
               {
                  pszMsg = pszMsg,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPWaitEvent.
      /// </summary>
      /// <param name="pulEvent"></param>
      /// <returns></returns>
      public static short PW_iPPWaitEvent(ref uint pulEvent)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPWaitEvent",
               string.Format("\t{0} = {1}", "pulEvent", pulEvent)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPWaitEvent"
               );

         short ret = PW_iPPWaitEvent_(ref pulEvent);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPWaitEvent"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPWaitEvent()",
               new
               {
                  pulEvent = pulEvent,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "pulEvent", pulEvent),
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPGenericCMD.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPGenericCMD(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPGenericCMD",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPGenericCMD"
               );

         short ret = PW_iPPGenericCMD_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPGenericCMD"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPGenericCMD()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPPositiveConfirmation.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPPositiveConfirmation(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPPositiveConfirmation",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPPositiveConfirmation"
               );

         short ret = PW_iPPPositiveConfirmation_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPPositiveConfirmation"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPPositiveConfirmation()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iWaitConfirmation.
      /// </summary>
      /// <returns></returns>
      public static short PW_iWaitConfirmation()
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iWaitConfirmation"
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iWaitConfirmation"
               );

         short ret = PW_iWaitConfirmation_();

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iWaitConfirmation"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iWaitConfirmation()",
               new
               {
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      /// <summary>
      /// Wrapper para PW_iPPTestKey.
      /// </summary>
      /// <param name="uiIndex"></param>
      /// <returns></returns>
      public static short PW_iPPTestKey(ushort uiIndex)
      {
         if (DebugType == DebugType.Default)
         {
            CallDebugCallback(
               "PW_iPPTestKey",
               string.Format("\t{0} = {1}", "uiIndex", uiIndex)
               );
         }

         Stopwatch stopwatch =
            CallBeginCallPGWebLibCallback(
               "PW_iPPTestKey"
               );

         short ret = PW_iPPTestKey_(uiIndex);

         long elapsedMilliseconds =
            CallEndCallPGWebLibCallback(
               stopwatch,
               "PW_iPPTestKey"
               );

         if (DebugType == DebugType.Json)
         {
            CallDebugCallback(
               "PW_iPPTestKey()",
               new
               {
                  uiIndex = uiIndex,
                  RET = PwRetToString(ret),
                  ElapsedMilliseconds = elapsedMilliseconds
               }
            );
         }
         else
         {
            CallDebugCallback(
               string.Format("\t{0} = {1}", "RET", PwRetToString(ret)),
               string.Format("\t{0} = {1}", "Elapsed Milliseconds", elapsedMilliseconds)
               );
         }

         return ret;
      }

      #endregion

      #region Funções Auxiliares

      private static string Identa(int depth)
      {
         return new string('\t', depth);
      }

      private static string TrataQuebraLinha(string mensagem)
      {
         return mensagem.Replace("\r", "\\r");
      }

      private static void SaveResourceToFile(string resourceName, string fileName)
      {
         if (File.Exists(fileName))
            return;

         string directoryName = Path.GetDirectoryName(fileName);
         if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

         Assembly assembly = Assembly.GetExecutingAssembly();
         resourceName =
            assembly
            .GetManifestResourceNames()
            .FirstOrDefault(str => str.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase));

         using (Stream stream = assembly.GetManifestResourceStream(resourceName))
         using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            stream.CopyTo(fileStream);
      }

      public static string StructToString<T>(T[] structs, int length, int depth = 1)
      {
         StringBuilder sb = new StringBuilder();

         foreach (var structValue in structs.Take(length))
         {
            string structName = structValue.GetType().Name;
            sb.AppendLine(string.Format("{0}{1}", Identa(depth), structName));
            foreach (var field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
               switch (structName)
               {
                  case "PW_GetData":
                     switch (field.Name)
                     {
                        case "bTipoDeDado":
                           sb.AppendLine(string.Format("{0}{1} = {2}", Identa(depth + 1), field.Name, PwDatToString(short.Parse(field.GetValue(structValue).ToString()))));
                           break;
                        case "wIdentificador":
                           sb.AppendLine(string.Format("{0}{1} = {2}", Identa(depth + 1), field.Name, PwInfoToString(ushort.Parse(field.GetValue(structValue).ToString()))));
                           break;
                        case "vszTextoMenu":
                           sb.AppendLine(string.Format("{0}{1} = ", Identa(depth + 1), field.Name));
                           byte? bNumOpcoesMenu = typeof(T).GetField("bNumOpcoesMenu").GetValue(structValue) as byte?;
                           if (bNumOpcoesMenu.HasValue && bNumOpcoesMenu.Value > 0)
                           {
                              TextoMenu[] vszTextoMenu = field.GetValue(structValue) as TextoMenu[];
                              sb.AppendLine(StructToString<TextoMenu>(vszTextoMenu, bNumOpcoesMenu.Value, (depth + 2)));
                           }
                           break;
                        case "vszValorMenu":
                           sb.AppendLine(string.Format("{0}{1} = ", Identa(depth + 1), field.Name));
                           bNumOpcoesMenu = typeof(T).GetField("bNumOpcoesMenu").GetValue(structValue) as byte?;
                           if (bNumOpcoesMenu.HasValue && bNumOpcoesMenu.Value > 0)
                           {
                              ValorMenu[] vszTextoMenu = field.GetValue(structValue) as ValorMenu[];
                              sb.AppendLine(StructToString<ValorMenu>(vszTextoMenu, bNumOpcoesMenu.Value, (depth + 2)));
                           }
                           break;
                        default:
                           sb.AppendLine(string.Format("{0}{1} = {2}", Identa(depth + 1), field.Name, TrataQuebraLinha(field.GetValue(structValue).ToString())));
                           break;
                     }
                     break;
                  default:
                     sb.AppendLine(string.Format("{0}{1} = {2}", Identa(depth + 1), field.Name, TrataQuebraLinha(field.GetValue(structValue).ToString())));
                     break;
               }
            }
         }
         return sb.ToString().TrimEnd('\r', '\n');
      }

      #endregion

   }
}