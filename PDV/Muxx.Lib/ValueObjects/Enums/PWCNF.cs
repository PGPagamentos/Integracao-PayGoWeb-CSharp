using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   /// <summary>
   /// Códigos de confirmação de transação
   /// </summary>
   public enum PWCNF
   {
      /// <summary>
      /// A transação foi confirmada pelo Ponto de Captura, 
      /// sem intervenção do usuário.
      /// </summary>
      PWCNF_CNF_AUTO = 0x00000121,
      /// <summary>
      /// A transação foi confirmada manualmente na Automação.
      /// </summary>
      PWCNF_CNF_MANU_AUT = 0x00003221,
      /// <summary>
      /// A transação foi desfeita manualmente na Automação.
      /// </summary>
      PWCNF_REV_MANU_AUT = 0x00003231,
      /// <summary>
      /// A transação foi desfeita pela Automação, 
      /// devido a uma falha na impressão do comprovante (não fiscal). 
      /// A priori, não usar. 
      /// Falhas na impressão não devem gerar desfazimento, 
      /// deve ser solicitada a reimpressão da transação.
      /// </summary>
      PWCNF_REV_PRN_AUT = 0x00013131,
      /// <summary>
      /// A transação foi desfeita pela Automação, 
      /// devido a uma falha no mecanismo de liberação da mercadoria.
      /// </summary>
      PWCNF_REV_DISP_AUT = 0x00023131,
      /// <summary>
      /// A transação foi desfeita pela Automação, 
      /// devido a uma falha de comunicação/integração 
      /// com o ponto de captura (Cliente Muxx).
      /// </summary>
      PWCNF_REV_COMM_AUT = 0x00033131,
      /// <summary>
      /// A transação não foi finalizada, 
      /// foi interrompida durante a captura de dados.
      /// </summary>
      PWCNF_REV_ABORT = 0x00043131,
      /// <summary>
      /// A transação foi desfeita a pedido da Automação, 
      /// por um outro motivo não previsto.
      /// </summary>
      PWCNF_REV_OTHER_AUT = 0x00073131,
      /// <summary>
      /// A transação foi desfeita automaticamente pela Automação, 
      /// devido a uma queda de energia (reinício abrupto do sistema).
      /// </summary>
      PWCNF_REV_PWR_AUT = 0x00083131,
      /// <summary>
      /// A transação foi desfeita automaticamente pela Automação, 
      /// devido a uma falha de registro no sistema fiscal 
      /// (impressora S@T, on-line, etc.).
      /// </summary>
      PWCNF_REV_FISC_AUT = 0x00093131,
   }
}