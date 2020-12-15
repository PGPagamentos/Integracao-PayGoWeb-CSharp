using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   /// <summary>
   /// Tipos de operação possíveis
   /// </summary>
   public enum PWOPER
   {
      /// <summary>
      /// Testa comunicação com a infraestrutura do PayGo.
      /// </summary>
      PWOPER_NULL = 0x00,
      /// <summary>
      /// Registra o Ponto de Captura perante a infraestrutura do
      /// PayGo, para que seja autorizado a realizar transações.
      /// </summary>
      PWOPER_INSTALL = 0x01,
      /// <summary>
      /// Obtém da infraestrutura do PayGo os parâmetros de
      /// operação atualizados do Ponto de Captura.
      /// </summary>
      PWOPER_PARAMUPD = 0x02,
      /// <summary>
      /// Obtém o último comprovante gerado por uma transação.
      /// </summary>
      PWOPER_REPRINT = 0x10,
      /// <summary>
      /// Obtém um relatório sintético das transações realizadas
      /// desde a última obtenção deste relatório.
      /// </summary>
      PWOPER_RPTTRUNC = 0x11,
      /// <summary>
      /// Relatório detalhado das transações realizadas na data
      /// informada, ou data atual.
      /// </summary>
      PWOPER_RPTDETAIL = 0x12,
      /// <summary>
      /// Realiza uma reimpressão de qualquer transação.
      /// </summary>
      PWOPER_REPRINTNTRANSACTION = 0x13,
      /// <summary>
      /// Realiza um teste de comunicação com o Provedor
      /// </summary>
      PWOPER_COMMTEST = 0x14,
      /// <summary>
      /// Realiza um Relatório resumido das transações realizadas na
      /// data informada, ou data atual.
      /// </summary>
      PWOPER_RPTSUMMARY = 0x15,
      /// <summary>
      /// Realiza uma consulta de uma transação.
      /// </summary>
      PWOPER_TRANSACINQ = 0x16,
      /// <summary>
      /// Realiza uma consulta de roteamento.
      /// </summary>
      PWOPER_ROUTINGINQ = 0x17,
      /// <summary>
      /// Acessa qualquer transação que não seja disponibilizada pelo
      /// comando PWOPER_SALE (relatório, reimpressão,
      /// cancelamento, pagamento de contas, entre outras). Um
      /// menu é apresentado para o operador selecionar a transação
      /// desejada.
      /// </summary>
      PWOPER_ADMIN = 0x20,
      /// <summary>
      /// (Venda) Realiza o pagamento de mercadorias e/ou serviços
      /// vendidos pelo Estabelecimento ao Cliente, a quitação do
      /// valor devido pode ser efetuada com diversas modalidades de
      /// pagamento: cartão de crédito/débito, carteira virtual, pontos
      /// de programa de fidelidade, entre outros, transferindo fundos
      /// entre as respectivas contas.
      /// </summary>
      PWOPER_SALE = 0x21,
      /// <summary>
      /// (Cancelamento de venda) Cancela uma transação
      /// PWOPER_SALE, realizando a transferência de fundos inversa.
      /// </summary>
      PWOPER_SALEVOID = 0x22,
      /// <summary>
      /// Realiza a aquisição de créditos pré-pagos (por exemplo,
      /// recarga de celular).
      /// </summary>
      PWOPER_PREPAID = 0x23,
      /// <summary>
      /// Consulta a validade de um cheque papel.
      /// </summary>
      PWOPER_CHECKINQ = 0x24,
      /// <summary>
      /// Consulta o saldo/limite do Estabelecimento (tipicamente,
      ///  limite de crédito para venda de créditos pré-pagos).
      /// </summary>
      PWOPER_RETBALINQ = 0x25,
      /// <summary>
      /// Consulta o saldo do cartão do Cliente.
      /// </summary>
      PWOPER_CRDBALINQ = 0x26,
      /// <summary>
      /// (Inicialização/abertura) Inicializa a operação junto ao
      /// Provedor e/ou obtém/atualiza os parâmetros de operação
      /// mantidos por este
      /// </summary>
      PWOPER_INITIALIZ = 0x27,
      /// <summary>
      /// (Fechamento/finalização) Finaliza a operação junto ao
      /// Provedor
      /// </summary>
      PWOPER_SETTLEMNT = 0x28,
      /// <summary>
      /// (Pré-autorização) Reserva o valor correspondente a uma
      /// venda no limite do cartão de crédito de um Cliente, porém
      /// sem efetivar a transferência de fundos.
      /// </summary>
      PWOPER_PREAUTH = 0x29,
      /// <summary>
      /// (Cancelamento de pré-autorização) Cancela uma transação
      /// PWOPER_PREAUTH, liberando o valor reservado no limite do
      /// cartão de crédito.
      /// </summary>
      PWOPER_PREAUTVOID = 0x2A,
      /// <summary>
      /// (Saque) Registra a retirada de um valor em espécie pelo
      /// Cliente no Estabelecimento, para transferência de fundos nas
      /// respectivas contas.
      /// </summary>
      PWOPER_CASHWDRWL = 0x2B,
      /// <summary>
      /// (Baixa técnica) Registra uma intervenção técnica no
      /// estabelecimento perante o Provedor.
      /// </summary>
      PWOPER_LOCALMAINT = 0x2C,
      /// <summary>
      /// Consulta as taxas de financiamento referentes a uma
      /// possível venda parcelada, sem efetivar a transferência de
      /// fundos ou impactar o limite de crédito do Cliente.
      /// </summary>
      PWOPER_FINANCINQ = 0x2D,
      /// <summary>
      /// Verifica junto ao Provedor o endereço do Cliente
      /// </summary>
      PWOPER_ADDRVERIF = 0x2E,
      /// <summary>
      /// Efetiva uma pré-autorização (PWOPER_PREAUTH),
      /// previamente realizada, realizando a transferência de fundos
      /// entre as contas do Estabelecimento e do Cliente.
      /// </summary>
      PWOPER_SALEPRE = 0x2F,
      /// <summary>
      /// Registra o acúmulo de pontos pelo Cliente, a partir de um
      /// programa de fidelidade.
      /// </summary>
      PWOPER_LOYCREDIT = 0x30,
      /// <summary>
      /// Cancela uma transação PWOPER_LOYCREDIT.
      /// </summary>
      PWOPER_LOYCREDVOID = 0x31,
      /// <summary>
      /// Registra o resgate de pontos/prêmio pelo Cliente, a partir de
      /// um programa de fidelidade.
      /// </summary>
      PWOPER_LOYDEBIT = 0x32,
      /// <summary>
      /// Cancela uma transação PWOPER_LOYDEBIT
      /// </summary>
      PWOPER_LOYDEBVOID = 0x33,
      /// <summary>
      /// Realiza um pagamento de conta/boleto/fatura.
      /// </summary>
      PWOPER_BILLPAYMENT = 0x34,
      /// <summary>
      /// Realiza uma consulta de documento de cobrança.
      /// </summary>
      PWOPER_DOCPAYMENTQ = 0x35,
      /// <summary>
      /// Realiza uma operação de logon no servidor.
      /// </summary>
      PWOPER_LOGON = 0x36,
      /// <summary>
      /// Realiza uma busca de pré-autorização
      /// </summary>
      PWOPER_SRCHPREAUTH = 0x37,
      /// <summary>
      /// Realiza uma alteração no valor de uma pré-autorização.
      /// </summary>
      PWOPER_ADDPREAUTH = 0x38,
      /// <summary>
      /// Exibe um menu com os cancelamentos disponíveis, caso só
      /// exista um tipo, este é selecionado automaticamente.
      /// </summary>
      PWOPER_VOID = 0x39,
      /// <summary>
      /// Realiza uma transação de estatísticas.
      /// </summary>
      PWOPER_STATISTICS = 0x40,
      /// <summary>
      /// Realiza um pagamento de cartão de crédito.
      /// </summary>
      PWOPER_CARDPAYMENT = 0x41,
      /// <summary>
      /// Cancela uma transação PWOPER_CARDPAYMENT.
      /// </summary>
      PWOPER_CARDPAYMENTVOID = 0x44,
      /// <summary>
      /// Cancela uma transação PWOPER_CASHWDRWL.
      /// </summary>
      PWOPER_CASHWDRWLVOID = 0x45,
      /// <summary>
      /// Realiza um desbloqueio de cartão.
      /// </summary>
      PWOPER_CARDUNLOCK = 0x46,
      //PWOPER_TRANSACINQ = 0x47,
      /// <summary>
      /// Realiza uma atualização no chip do cartão.
      /// </summary>
      PWOPER_UPDATEDCHIP = 0x48,
      /// <summary>
      /// Realiza uma transação de relatório promocional.
      /// </summary>
      PWOPER_RPTPROMOTIONAL = 0x49,
      /// <summary>
      /// Imprime um resumo das transações de vendas.
      /// </summary>
      PWOPER_SALESUMMARY = 0x4A,
      /// <summary>
      /// Realiza uma estatística específica do autorizador.
      /// </summary>
      PWOPER_STATISTICSAUTHORIZER = 0x4B,
      /// <summary>
      /// Realiza uma transação administrativa especificada pelo
      /// autorizador.
      /// </summary>
      PWOPER_OTHERADMIN = 0x4C,
      /// <summary>
      /// Cancela uma transação PWOPER_BILLPAYMENT.
      /// </summary>
      PWOPER_BILLPAYMENTVOID = 0x4E,
      PWOPER_RPTCERT = 0x50,
      PWOPER_RPTCERTDETAIL = 0x51,
      PWOPER_CONFIGAUTH = 0x52,
      /// <summary>
      /// Teste de chaves de criptografia do pinpad
      /// </summary>
      PWOPER_TSTKEY = 0xF0,
      /// <summary>
      /// Realiza uma operação para obter os dados básicos do PdC.
      /// O resultado dessa operação deve ser consultado por meio da
      /// chamada de PW_iGetResult para as informações:
      /// PWINFO_AUTADDRESS, PWINFO_APN,
      /// PWINFO_LIBVERSION, PWINFO_POSID,
      /// PWINFO_DESTTCPIP, PWINFO_LOCALIP,
      /// PWINFO_GATEWAY, PWINFO_SUBNETMASK,
      /// PWINFO_PPPPWD, PWINFO_SSID.
      /// </summary>
      PWOPER_COMMONDATA = 0xFA,
      /// <summary>
      /// Exibe o ponto de captura configurado.
      /// </summary>
      PWOPER_SHOWPDC = 0xFB,
      /// <summary>
      /// (Versão) Permite consultar a versão da biblioteca atualmente
      /// em uso.
      /// </summary>
      PWOPER_VERSION = 0xFC,
      /// <summary>
      /// (Configuração) Visualiza e altera os parâmetros de operação
      /// locais da biblioteca.
      /// </summary>
      PWOPER_CONFIG = 0xFD,
      /// <summary>
      /// (Manutenção) Apaga todas as configurações do Ponto de
      /// Captura, devendo ser novamente realizada uma transação de
      /// Instalação.
      /// </summary>
      PWOPER_MAINTENANCE = 0xFE,
   }
}