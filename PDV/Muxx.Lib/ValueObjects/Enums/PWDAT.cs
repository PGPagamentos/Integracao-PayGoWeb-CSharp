using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   /// <summary>
   /// Tipos utilizados na captura de dados dinamica
   /// </summary>
   public enum PWDAT : byte
   {
      /// <summary>
      /// Menu de opções
      /// </summary>
      PWDAT_MENU = 1,
      /// <summary>
      /// Entrada digitada
      /// </summary>
      PWDAT_TYPED = 2,
      /// <summary>
      /// Dados de cartão
      /// </summary>
      PWDAT_CARDINF = 3,
      /// <summary>
      /// Entrada digitada no PIN-pad
      /// </summary>
      PWDAT_PPENTRY = 5,
      /// <summary>
      /// Senha criptografada
      /// </summary>
      PWDAT_PPENCPIN = 6,
      /// <summary>
      /// Processamento off-line de cartão com chip
      /// </summary>
      PWDAT_CARDOFF = 9,
      /// <summary>
      /// Processamento on-line de cartão com chip
      /// </summary>
      PWDAT_CARDONL = 10,
      /// <summary>
      /// Confirmação de informação no PIN-pad
      /// </summary>
      PWDAT_PPCONF = 11,
      /// <summary>
      /// Código de barras, lido ou digitado.
      /// </summary>
      PWDAT_BARCODE = 12,
      /// <summary>
      /// Remoção do cartão do PIN-pad.
      /// </summary>
      PWDAT_PPREMCRD = 13,
      /// <summary>
      /// Comando proprietário da rede no PIN-pad.
      /// </summary>
      PWDAT_PPGENCMD = 14,
      /// <summary>
      /// Confirmação positiva de dados no PIN-pad.
      /// </summary>
      PWDAT_PPDATAPOSCNF = 16,
      /// <summary>
      /// Validação da senha.
      /// </summary>
      PWDAT_USERAUTH = 17,
      /// <summary>
      /// Exibição de determinada mensagem no checkout durante o processamento
      /// </summary>
      PWDAT_DSPCHECKOUT = 18,
      /// <summary>
      /// Processamento do teste de chaves
      /// </summary>
      PWDAT_TSTKEY = 19,
      /// <summary>
      /// Exibição de QR code no checkout
      /// </summary>
      PWDAT_DSPQRCODE = 20,
   }
}