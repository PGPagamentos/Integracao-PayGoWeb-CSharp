using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   public enum PWINFO_AUTCAP : ushort
   {
      PWINFO_AUTCAP_TROCO_SAQUE = 1,              /* funcionalidade de troco/saque */
      PWINFO_AUTCAP_DESCONTO = 2,                 /* funcionalidade de desconto */
      PWINFO_AUTCAP_VALOR_FIXO = 4,               /* valor fixo, sempre incluir */
      PWINFO_AUTCAP_CUPOM_VIAS_DIFERENCIADAS = 8, /* impressão das vias diferenciadas do comprovante para Cliente/Estabelecimento */
      PWINFO_AUTCAP_CUPOM_REDUZIDO = 16,          /* impressão do cupom reduzido */
      PWINFO_AUTCAP_SALDO_TOTAL_VOUCHER = 32,     /* utilização de saldo total do voucher para abatimento do valor da compra */
      PWINFO_AUTCAP_REMOCAO_CARTAO = 64,          /* tratar a remoção do cartão do PIN-pad */
      PWINFO_AUTCAP_DSP_CHECKOUT = 128,           /* Capacidade de exibição de mensagens durante o fluxo transacional */
      PWINFO_AUTCAP_DSP_QRCODE = 256              /* Capacidade de exibição de QRcode para pagamento com carteira digital */
   }
}