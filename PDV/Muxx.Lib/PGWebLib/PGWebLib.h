/*********************** SETIS - Automação e Sistemas ************************

 Arquivo          : PGWebLib.h
 Projeto          : Pay&Go WEB
 Plataforma       : POSPlug
 Data de criação  : 25/02/2013
 Autor            : Guilherme Eduardo Leite
 Descrição        : Definições da dll para integração com a solução Pay&Go WEB.

 ================================= HISTÓRICO =================================
 Data      Responsável Modificação
 --------  ----------- -------------------------------------------------------
 07/Ago/17 Guilherme    - Criados retornos PWRET_OFFCFGxxx (CA17-0049).
 06/Out/17 Guilherme    - Criado intervalo exclusivo para os erros no 
                          processamento local.(CA17-0061).
 23/Out/17 Massaia      - Criado PWRET_OFFINTERNAL17 para erros relacionados
                          ao processo de criptografia específica (CA17-0060).
 16/Nov/17 Guilherme    - Criado PWRET_OFFINTERNAL18 (CA17-0079).
 14/Dez/17 Guilherme    - Criado PWRET_PPABORT (CA17-0090).
 01/Fev/18 Guilherme    - Criado o novo erro PWRET_OFFINTERNAL19(CA18-0012).
 07/Fev/18 Guilherme    - Criada nova função PW_iPPGetUserData (CA18-0014).
 22/Fev/18 Guilherme    - Ampliação de PWMENU_MAXINTENS de 20 para 40 (CA18-0021).
 20/Jul/18 Massaia      - Adicionado PWRET_PPERRTREATMENT (CA18-0062).
 01/Ago/18 Erwin			- Adicionado PWINFO_AUTHSYSTEXTENDED (CA18-0067).
 30/Ago/18 Guilherme    - Adicionadas TAGS para recebimento dos dados de confirmação
                          da biblioteca de integração.
 05/Out/18 Massaia      - Adicionado algumas tags para implementação do comprovante
                          gráfico(CA18-0087).
 19/Out/18 Mateus V.    - Adicionado nova operação para exibir no menu o PDC (CA18-0094).
 06/Dez/18 Guilherme    - Criação da função PW_iPPGetPINBlock para obtenção de um 
                          PIN block do PIN-pad (CA18-0117).
 02/Jan/19 Mateus V.    - Adicionado erro PWRET_INVPAYMENTMODE para modo de pagamento não
                          encontrado nas tabelas de inicialização.(CA19-0002).
 04/Jan/19 Guilherme    - Criado PWRET_APNERROR (CA19-0003).
 26/Fev/19 Massaia      - Passa a receber novas tags para o tratamento da carteira 
                          digital PWINFO_AUTHPOSQRCODE e PWINFO_WALLETUSERIDTYPE (CA19-0031).
 11/Mar/19 Erwin        - Adicionado tag PWINFO_UNIQUEID (CA19-0046).
 26/Mar/19 Massaia      - Alteração do valor da tag wallettype (CA19-0053).
 18/Abr/19 Felipe S.    - Criado PWRET_WIFIAUTHERR (CA19-0063).
 23/Abr/19 Massaia      - Criado PWINFO_RESULTID (CA19-0069).
 15/Mai/19 Guilherme    - Criados PWPPEVT_ICCOUT e PWPPEVTIN_ICCOUT(CA19-0093).
 27/Mai/19 Massaia      - Criado os retornos PWRET_PPS_CTLSIFCHG, PWRET_PPS_CTLSEXTCVM (CA19-0100).
 29/Mai/19 Massaia      - Criado PWINFO_SPLITPAYMENT (CA19-0100).
 24/Jun/19 Erwin        - Criado PWINFO_TRNORIGLOCREF (CA19-0123).
 28/Jun/19 Mateus V.    - Criado PWINFO_EMVRESPCODE (CA19-0127).
 23/Jul/19 Massaia      - Criado PWRET_OFFINTERNAL20 (CA19-0142).
 02/Ago/19 Massaia      - Criado PWDAT_DSPCHECKOUT (CA19-0149).
 12/Ago/19 Erwin        - Criado PWOPER_RPTCERT, PWOPER_RPTCERTDETAIL e PWOPER_CONFIGAUTH (CA19-0153)
 28/Ago/19 Massaia      - Criado PWRET_QRCODENOTSUPPORTED e PWRET_QRCODEERR (CA19-0159).
 04/Set/19 Erwin        - Criado PWRET_PRODNAMEDESC (CA19-0167).
 09/Set/19 Mateus V.    - Criação da função PW_iGetOperationsEx para obter a Lista de Operações X autorizadores (CA19-0171).
 17/Set/19 Mateus V.    - Criado PWINFO_CTLSCAPTURE (CA19-0173).
 26/Set/19 Guilherme    - Criados PWINFO_MERCHNAMERCPT, PWINFO_PRODESTABRCPT, PWINFO_PRODCLIRCPT, PWINFO_EMVCRYPTTYPE, 
                          PWINFO_TRNORIGDATETIME, PWINFO_DATETIMERCPT e PWINFO_OPERATIONORIG(CA19-0182).
 30/Out/19 Erwin        - Criado PWRET_OFFINTERNAL21 (CA19-0206).
 31/Out/19 Mateus V.    - Criado PWINFO_CHOLDERGRARCP e PWINFO_MERCHRGRARCP (CA19-0208).
 26/Nov/19 Mateus V.    - Criado PWRET_DEFAULT_COMM_ERROR (CA19-0227).
 06/Fev/20 Mateus V.    - Criado PWINFO_TRNINFO PWINFO_RMCCAUSE (CA20-0015).
 13/Fev/20 Massaia      - Criado PWRET_CTLSMAGSTRIPENOTALLOW (CA20-0018).
 11/Mar/20 Mateus V.    - Criado PWOPER_COMMONDATA, PWINFO_APN, PWINFO_AUTADDRESS, PWINFO_LIBVERSION (CA20-0043).
 11/Mar/20 Massaia      - Criado PWOPER_TSTKEY, PWDAT_TSTKEY e PWINFO_TSTKEYTYPE, PWINFO_TSTKEYATR, PWINFO_TKPINDUKPT3DES
                          PWINFO_TKPINMK3DES, PWINFO_TKPINMKDES, PWINFO_TKDADOSDUKPT3DES, PWINFO_TKDADOSMK3DES 
                          e PWINFO_TKDADOSMKDES (CA20-0043).
 11/Mar/20 Guilherme    - Criado o erro PWRET_PARAMSFILEERRSIZE (CA19-0236).
 17/Mar/20 Mateus V.    - Criada função PW_iWaitConfirmation (CA20-0047).
 20/Mar/20 Mateus V.    - Criado PWINFO_DSPTSTKEY, PWINFO_GETKSNPIN, PWINFO_GETKSNDATA (CA20-0052).
 25/Mar/20 Mateus V.    - Criado PWINFO_PINDUKPT3DESNAME, PWINFO_PINMK3DESNAME, PWINFO_PINMKDESNAME, PWINFO_DATADUKPT3DESNAME,
                          PWINFO_DATAMK3DESNAME, PWINFO_DATAMKDESNAME (CA20-0055).
 03/Abr/20 Guilherme    - Criado PWINFO_LOCALINFO2 (CA20-0062).
 03/Abr/20 Mateus V.    - Criado PWINFO_SERNUM, PWINFO_MACADDR, PWINFO_IMEI, PWINFO_IPADDRESS, PWINFO_SSID_IDX, PWINFO_DNSSERVER_P,
                          PWINFO_DNSSERVER_S, PWINFO_OSVERSION, PWINFO_APPDOWNLOADVER (CA20-0064).
 12/Mai/20 Guilherme    - Criados PWINFO_RCPTADDINFOESTABCLI, PWINFO_RCPTADDINFOCLI e PWINFO_RCPTADDINFOESTAB (CA20-0085).
 19/Mai/20 Massaia      - Criado PWRET_PPS_CTCARDBLOCKED (CA20-0090).
 15/Jun/20 Mateus V.    - Criado PWINFO_RCPTECVID (CA20-0112).
 18/Set/20 Guilherme    - Adicionados PWINFO_DSPQRPREF e PWDAT_DSPQRCODE (CA20-0148).
 \*****************************************************************************/
#ifndef _PGWEBLIB_INCLUDED_
#define _PGWEBLIB_INCLUDED_

// Incluindo o header que define os tipos de variáveis para a plataforma em questão
#ifndef PGLIB_DACASA
#include "PWL_Type.h"
#endif 

#ifndef CALLBACK
#define CALLBACK
#endif /* CALLBACK */

#ifndef PW_EXPORT
#define PW_EXPORT
#endif /* PW_EXPORT */

// Códigos de retorno da biblioteca
enum {
   // Erros gerais
   PWRET_OK = 0,   
   PWRET_FROMHOSTPENDTRN = -2599, 
   PWRET_FROMHOSTPOSAUTHERR,
   PWRET_FROMHOSTUSRAUTHERR,
   PWRET_FROMHOST,
   PWRET_TLVERR,
   PWRET_SRVINVPARAM,
   PWRET_REQPARAM,
   PWRET_HOSTCONNUNK,
   PWRET_INTERNALERR,
   PWRET_BLOCKED,
   PWRET_FROMHOSTTRNNFOUND,
   PWRET_PARAMSFILEERR,
   PWRET_NOCARDENTMODE,
   PWRET_INVALIDVIRTMERCH,
   PWRET_HOSTTIMEOUT,
   PWRET_CONFIGREQUIRED,
   PWRET_HOSTCONNERR,
   PWRET_HOSTCONNLOST,
   PWRET_FILEERR,
   PWRET_PINPADERR,
   PWRET_MAGSTRIPEERR,
   PWRET_PPCRYPTERR,
   PWRET_SSLCERTERR,
   PWRET_SSLNCONN,
   PWRET_GPRSATTACHFAILED,
   PWRET_EMVDENIEDCARD,
   PWRET_EMVDENIEDHOST,
   PWRET_NOLINE,
   PWRET_NOANSWER,
   PWRET_SYNCERROR,
   PWRET_CRCERR,
   PWRET_DECOMPERR,
   PWRET_PROTERR,
   PWRET_NOSIM,
   PWRET_SIMERROR,
   PWRET_SIMBLOCKED,
   PWRET_PPPNEGFAILED,
   PWRET_WIFICONNERR,
   PWRET_WIFINOTFOUND,
   PWRET_COMPERR,
   PWRET_INVALIDCPFCNPJ,
   PWRET_APNERROR,
   PWRET_WIFIAUTHERR,
   PWRET_QRCODEERR,
   PWRET_QRCODENOTSUPPORTED,
   PWRET_QRCODENOTFOUND,
   PWRET_DEFAULT_COMM_ERROR,
   PWRET_CTLSMAGSTRIPENOTALLOW,
   PWRET_PARAMSFILEERRSIZE,
   /* Inserir novos erros gerais somente AQUI */

   // Erros específicos da biblioteca
   PWRET_INVPARAM = -2499,
   PWRET_NOTINST,   
   PWRET_MOREDATA,  
   PWRET_NODATA,    
   PWRET_DISPLAY,    
   PWRET_INVCALL,    
   PWRET_NOTHING,    
   PWRET_BUFOVFLW,   
   PWRET_CANCEL,     
   PWRET_TIMEOUT,    
   PWRET_PPNOTFOUND, 
   PWRET_TRNNOTINIT, 
   PWRET_DLLNOTINIT, 
   PWRET_FALLBACK,   
   PWRET_WRITERR,    
   PWRET_PPCOMERR,	 
   PWRET_NOMANDATORY,
   PWRET_OFFINTERNAL,
   PWRET_OFFINVCAP,
   PWRET_OFFNOCARDENTMODE,
   PWRET_OFFINVCARDENTMODE,
   PWRET_OFFNOTABLECARDRANGE,
   PWRET_OFFNOTABLEPRODUCT,
   PWRET_OFFINVTAG,
   PWRET_OFFNOCARDFULLPAN,
   PWRET_OFFINVCARDEXPDT,
   PWRET_OFFCARDEXP,
   PWRET_OFFNOTRACKS,
   PWRET_OFFTRACKERR,
   PWRET_OFFCHIPMANDATORY,
   PWRET_OFFINVCARD,
   PWRET_OFFINVCURR,
   PWRET_OFFINVAMOUNT,
   PWRET_OFFGREATERAMNT,
   PWRET_OFFLOWERAMNT,
   PWRET_OFFGREATERINST,
   PWRET_OFFLOWERINST,
   PWRET_OFFINVCARDTYPE,
   PWRET_OFFINVFINTYPE,
   PWRET_OFFINVINST,
   PWRET_OFFGREATERINSTNUM,
   PWRET_OFFLOWERINSTNUM,
   PWRET_OFFMANDATORYCVV,
   PWRET_OFFINVLASTFOUR,
   PWRET_OFFNOAID,
   PWRET_OFFNOFALLBACK,
   PWRET_OFFNOPINPAD,
   PWRET_OFFNOAPOFF,
   PWRET_OFFTRNNEEDPP,
   PWRET_OFFCARDNACCEPT,
   PWRET_OFFTABLEERR,
   PWOFF_OFFMAXTABERR,
   PWRET_OFFINTERNAL1,
   PWRET_OFFINTERNAL2,
   PWRET_OFFINTERNAL3,
   PWRET_OFFINTERNAL4,
   PWRET_OFFINTERNAL5,
   PWRET_OFFINTERNAL6,
   PWRET_OFFINTERNAL7,
   PWRET_OFFINTERNAL8,
   PWRET_OFFINTERNAL9,
   PWRET_OFFINTERNAL10,
   PWRET_OFFINTERNAL11,
   PWRET_OFFNOPRODUCT,
   PWRET_OFFINTERNAL12,
   PWRET_OFFINTERNAL13,
   PWRET_OFFINTERNAL14,
   PWRET_NOPINPAD,
   PWRET_OFFINTERNAL15,
   PWRET_OFFINTERNAL16,
   PWRET_ABECSERRCOM,
   PWRET_OFFCFGNOCARDRANGE,
   PWRET_OFFCFGNOPRODUCT,
   PWRET_OFFCFGNOTRANSACTION,
   PWRET_OFFINTERNAL17,
   PWRET_OFFINTERNAL18,
   PWRET_PPABORT,
   PWRET_OFFINTERNAL19,
   PWRET_PPERRTREATMENT,
   PWRET_INVPAYMENTMODE,
   PWRET_OFFINVALIDOPER,
   PWRET_OFFINTERNAL20,
   PWRET_OFFINTERNAL21,
   /* Inserir novos erros de processamento local somente AQUI */
   PWRET_OFFEND   
   /* Inserir novos erros da biblioteca somente AQUI */

};
// Erros específicos da biblioteca compartilhada de PIN-pad
#define PWRET_PPS_MAX      -2100
#define PWRET_PPS_MIN      PWRET_PPS_MAX - 100

enum{
   /* Status de -2199 a -2180 : Erros de processamento de cartão com chip sem contato */
   PWRET_PPS_CTLSIFCHG = PWRET_PPS_MAX - 87,
   PWRET_PPS_CTLSEXTCVM,
   PWRET_PPS_CTLSSAPPNAUT,
   PWRET_PPS_CTLSSAPPNAV,
   PWRET_PPS_CTLSSPROBLEMS,
   PWRET_PPS_CTLSSINVALIDAT,
   PWRET_PPS_CTLSSCOMMERR,
   PWRET_PPS_CTLSSMULTIPLE,
   PWRET_PPS_CTCARDBLOCKED,

   /* Status de -2179 a -2160 : Erros de processamento de cartão com chip com contato */
   PWRET_PPS_ERRFALLBACK = PWRET_PPS_MAX - 76,
   PWRET_PPS_VCINVCURR,
   PWRET_PPS_CARDNOTEFFECT,
   PWRET_PPS_LIMITEXC,
   PWRET_PPS_NOBALANCE,
   PWRET_PPS_CARDAPPNAUT,
   PWRET_PPS_CARDAPPNAV,
   PWRET_PPS_CARDINVDATA,
   PWRET_PPS_CARDPROBLEMS,
   PWRET_PPS_CARDINVALIDAT,
   PWRET_PPS_CARDERRSTRUCT,
   PWRET_PPS_CARDEXPIRED,
   PWRET_PPS_CARDNAUTH,
   PWRET_PPS_CARDBLOCKED,
   PWRET_PPS_CARDINV,
   PWRET_PPS_ERRCARD,
   PWRET_PPS_DUMBCARD,

   /* Status de -2159 a -2150 : Erros de processamento de cartão com chip (SAM) */
   PWRET_PPS_SAMINV = PWRET_PPS_MAX - 52,
   PWRET_PPS_NOSAM,
   PWRET_PPS_SAMERR,

   /* Status de -2149 a -2140 : Erros básicos reportados pelo pinpad */
   PWRET_PPS_PINBUSY = PWRET_PPS_MAX - 44,
   PWRET_PPS_NOCARD,
   PWRET_PPS_ERRPIN,
   PWRET_PPS_MCDATAERR,
   PWRET_PPS_INTERR,

   /* Status de -2139 a -2130 : Erros de comunicação/protocolo com o pinpad */
   PWRET_PPS_COMMTOUT = PWRET_PPS_MAX - 34,
   PWRET_PPS_RSPERR,
   PWRET_PPS_UNKNOWNSTAT,
   PWRET_PPS_COMMERR,
   PWRET_PPS_PORTERR,

   /* Status de -2129 a -2110 : Erros básicos da biblioteca */
   PWRET_PPS_NOAPPLIC = PWRET_PPS_MAX - 22,
   PWRET_PPS_TABERR,
   PWRET_PPS_TABEXP,
   RESERVED,
   PWRET_PPS_NOFUNC,
   PWRET_PPS_INVMODEL,
   PWRET_PPS_EXECERR,
   PWRET_PPS_NOTOPEN,
   PWRET_PPS_ALREADYOPEN,
   PWRET_PPS_CANCEL,
   PWRET_PPS_TIMEOUT,
   PWRET_PPS_INVPARM,
   PWRET_PPS_INVCALL,

   /* Status de -2109 a -2100  : Não representam erros */
   PWRET_PPS_BACKSP = PWRET_PPS_MAX - 8,
   PWRET_PPS_F4,
   PWRET_PPS_F3,
   PWRET_PPS_F2,
   PWRET_PPS_F1,
   PWRET_PPS_NOTIFY = PWRET_PPS_MAX - 2,
   PWRET_PPS_PROCESSING,
   PWRET_PPS_OK
};

// Códigos de confirmação de transação
#define PWCNF_CNF_AUTO	      0x00000121	/*A transação foi confirmada pelo Ponto de Captura, sem intervenção do usuário.*/
#define PWCNF_CNF_MANU_AUT	   0x00003221	/*A transação foi confirmada manualmente na Automação.*/
#define PWCNF_REV_MANU_AUT	   0x00003231	/*A transação foi desfeita manualmente na Automação.*/
#define PWCNF_REV_PRN_AUT	   0x00013131	/*A transação foi desfeita pela Automação, devido a uma falha na impressão do comprovante (não fiscal). A priori, não usar. Falhas na impressão não devem gerar desfazimento, deve ser solicitada a reimpressão da transação.*/
#define PWCNF_REV_DISP_AUT	   0x00023131	/*A transação foi desfeita pela Automação, devido a uma falha no mecanismo de liberação da mercadoria.*/
#define PWCNF_REV_COMM_AUT	   0x00033131	/*A transação foi desfeita pela Automação, devido a uma falha de comunicação/integração com o ponto de captura (Cliente Muxx).*/
#define PWCNF_REV_ABORT	      0x00043131	/*A transação não foi finalizada, foi interrompida durante a captura de dados.*/
#define PWCNF_REV_OTHER_AUT   0x00073131	/*A transação foi desfeita a pedido da Automação, por um outro motivo não previsto.*/
#define PWCNF_REV_PWR_AUT	   0x00083131	/*A transação foi desfeita automaticamente pela Automação, devido a uma queda de energia (reinício abrupto do sistema).*/
#define PWCNF_REV_FISC_AUT	   0x00093131	/*A transação foi desfeita automaticamente pela Automação, devido a uma falha de registro no sistema fiscal (impressora S@T, on-line, etc.).*/

// Tipos utilizados na captura de dados dinamica
#define PWDAT_MENU         1     /*menu de opções*/
#define PWDAT_TYPED        2     /*entrada digitada*/
#define PWDAT_CARDINF      3     /*dados de cartão*/
#define PWDAT_PPENTRY      5     /*entrada digitada no PIN-pad*/
#define PWDAT_PPENCPIN     6     /*senha criptografada   */             
#define PWDAT_CARDOFF      9     /*processamento off-line de cartão com chip*/
#define PWDAT_CARDONL      10    /*processamento on-line de cartão com chip*/
#define PWDAT_PPCONF       11    /*confirmação de informação no PIN-pad*/
#define PWDAT_BARCODE	   12	   /*Código de barras, lido ou digitado.*/
#define PWDAT_PPREMCRD     13    /*Remoção do cartão do PIN-pad.*/
#define PWDAT_PPGENCMD     14    /*comando proprietário da rede no PIN-pad.*/
#define PWDAT_PPDATAPOSCNF 16    /*confirmação positiva de dados no PIN-pad.*/
#define PWDAT_USERAUTH     17    /*validação da senha.*/
#define PWDAT_DSPCHECKOUT  18    /*Exibição de determinada mensagem no checkout durante o processamento */
#define PWDAT_TSTKEY       19    /*Processamento do teste de chaves */
#define PWDAT_DSPQRCODE    20    /*Exibição de QR code no checkout */

// Tipos de evento a serem ativados para monitoração no PIN-pad
#define PWPPEVTIN_KEYS     1
#define PWPPEVTIN_MAG      2  
#define PWPPEVTIN_ICC      4
#define PWPPEVTIN_CTLS     8
#define PWPPEVTIN_ICCOUT   16

// Tipos de evento retornados pelo PIN-pad
#define PWPPEVT_MAGSTRIPE  0x01  /* Foi passado um cartão magnético. */
#define PWPPEVT_ICC        0x02  /* Foi detectada a presença de um cartão com chip. */
#define PWPPEVT_CTLS       0x03  /* Foi detectada a presença de um cartão sem contato. */
#define PWPPEVT_ICCOUT     0x04  /* Foi detectada a ausencia de um cartão com chip. */
#define PWPPEVT_KEYCONF    0x11  /* Foi pressionada a tecla [OK]. */
#define PWPPEVT_KEYBACKSP  0x12  /* Foi pressionada a tecla [CORRIGE]. */
#define PWPPEVT_KEYCANC    0x13  /* Foi pressionada a tecla [CANCELA]. */
#define PWPPEVT_KEYF1      0x21  /* Foi pressionada a tecla [F1]. */
#define PWPPEVT_KEYF2      0x22  /* Foi pressionada a tecla [F2]. */
#define PWPPEVT_KEYF3      0x23  /* Foi pressionada a tecla [F3]. */
#define PWPPEVT_KEYF4      0x24  /* Foi pressionada a tecla [F4]. */

// Número maximo de itens em um menu de seleção
#define PWMENU_MAXINTENS        40

// Tipos de operação possíveis
#define PWOPER_NULL              0x00
#define PWOPER_INSTALL           0x01
#define PWOPER_PARAMUPD          0x02
#define PWOPER_REPRINT           0x10
#define PWOPER_RPTTRUNC          0x11
#define PWOPER_RPTDETAIL         0x12
#define PWOPER_REPRINTNTRANSACTION  0x13
#define PWOPER_COMMTEST          0x14
#define PWOPER_RPTSUMMARY        0x15
#define PWOPER_TRANSACINQ        0x16
#define PWOPER_ROUTINGINQ        0x17
#define PWOPER_ADMIN             0x20
#define PWOPER_SALE              0x21
#define PWOPER_SALEVOID          0x22
#define PWOPER_PREPAID           0x23
#define PWOPER_CHECKINQ          0x24
#define PWOPER_RETBALINQ         0x25
#define PWOPER_CRDBALINQ         0x26
#define PWOPER_INITIALIZ         0x27
#define PWOPER_SETTLEMNT         0x28
#define PWOPER_PREAUTH           0x29
#define PWOPER_PREAUTVOID        0x2A
#define PWOPER_CASHWDRWL         0x2B
#define PWOPER_LOCALMAINT        0x2C
#define PWOPER_FINANCINQ         0x2D
#define PWOPER_ADDRVERIF         0x2E
#define PWOPER_SALEPRE           0x2F
#define PWOPER_LOYCREDIT         0x30
#define PWOPER_LOYCREDVOID       0x31
#define PWOPER_LOYDEBIT          0x32
#define PWOPER_LOYDEBVOID        0x33
#define PWOPER_BILLPAYMENT	      0x34    
#define PWOPER_DOCPAYMENTQ	      0x35    
#define PWOPER_LOGON	            0x36    
#define PWOPER_SRCHPREAUTH	      0x37    
#define PWOPER_ADDPREAUTH	      0x38    
#define PWOPER_VOID	            0x39    
#define PWOPER_STATISTICS	      0x40    
#define PWOPER_CARDPAYMENT	      0x41    
#define PWOPER_CARDPAYMENTVOID	0x44    
#define PWOPER_CASHWDRWLVOID	   0x45    
#define PWOPER_CARDUNLOCK	      0x46    
//#define PWOPER_TRANSACINQ	      0x47 
#define PWOPER_UPDATEDCHIP       0x48
#define PWOPER_RPTPROMOTIONAL    0x49
#define PWOPER_SALESUMMARY       0x4A
#define PWOPER_STATISTICSAUTHORIZER 0x4B
#define PWOPER_OTHERADMIN	      0x4C
#define PWOPER_BILLPAYMENTVOID   0x4E
#define PWOPER_RPTCERT           0x50
#define PWOPER_RPTCERTDETAIL     0x51
#define PWOPER_CONFIGAUTH        0x52
#define PWOPER_TSTKEY            0xF0
#define PWOPER_COMMONDATA        0xFA
#define PWOPER_SHOWPDC           0xFB
#define PWOPER_VERSION	         0xFC
#define PWOPER_CONFIG            0xFD
#define PWOPER_MAINTENANCE       0xFE	

// Tipos de dados
#define PWINFO_OPERATION         0x02
#define PWINFO_PPPPWD            0x03
#define PWINFO_SENHASIM          0x04
#define PWINFO_AUTIP             0x05
#define PWINFO_USINGAUT          0x06
#define PWINFO_AUTPORT           0x07
#define PWINFO_ADDRMODE          0x08
#define PWINFO_LOCALIP           0x09
#define PWINFO_GATEWAY           0x0A
#define PWINFO_SUBNETMASK        0x0B
#define PWINFO_SSID              0x0C
#define PWINFO_WIFITYPE          0x0D
#define PWINFO_WIFIKEY           0x0E
#define PWINFO_COMMTYPE          0x0F
//Livre!!
#define PWINFO_POSID				   0x11
#define PWINFO_AUTNAME			   0x15
#define PWINFO_AUTVER			   0x16
#define PWINFO_AUTDEV			   0x17
#define PWINFO_DESTTCPIP			0x1B
#define PWINFO_MERCHCNPJCPF      0x1C
#define PWINFO_AUTCAP			   0x24
#define PWINFO_TOTAMNT		      0x25
#define PWINFO_CURRENCY		      0x26
#define PWINFO_CURREXP			   0x27
#define PWINFO_FISCALREF			0x28
#define PWINFO_CARDTYPE		      0x29
#define PWINFO_PRODUCTNAME			0x2A	
#define PWINFO_DATETIME		      0x31
#define PWINFO_REQNUM				0x32
#define PWINFO_AUTHSYST		      0x35
#define PWINFO_VIRTMERCH	      0x36
#define PWINFO_AUTMERCHID	      0x38
#define PWINFO_PHONEFULLNO		   0x3A
#define PWINFO_FINTYPE		      0x3B
#define PWINFO_INSTALLMENTS	   0x3C
#define PWINFO_INSTALLMDATE	   0x3D
#define PWINFO_PRODUCTID         0x3E
#define PWINFO_RESULTMSG	      0x42
#define PWINFO_CNFREQ   	      0x43
#define PWINFO_AUTLOCREF	      0x44
#define PWINFO_AUTEXTREF	      0x45
#define PWINFO_AUTHCODE		      0x46
#define PWINFO_AUTRESPCODE	      0x47
#define PWINFO_DISCOUNTAMT	      0x49
#define PWINFO_CASHBACKAMT	      0x4A
#define PWINFO_CARDNAME		      0x4B
#define PWINFO_ONOFF				   0x4C	
#define PWINFO_BOARDINGTAX		   0x4D
#define PWINFO_TIPAMOUNT			0x4E
#define PWINFO_INSTALLM1AMT		0x4F
#define PWINFO_INSTALLMAMNT		0x50
#define PWINFO_RCPTFULL 			0x52
#define PWINFO_RCPTMERCH			0x53
#define PWINFO_RCPTCHOLDER			0x54		
#define PWINFO_RCPTCHSHORT			0x55
#define PWINFO_TRNORIGDATE	      0x57
#define PWINFO_TRNORIGNSU	      0x58
#define PWINFO_SALDOVOUCHER      0x59
#define PWINFO_TRNORIGAMNT		   0x60
#define PWINFO_LANGUAGE          0x6C
#define PWINFO_PROCESSMSG        0x6F
#define PWINFO_TRNORIGAUTH		   0x62
#define PWINFO_TRNORIGREQNUM		0x72
#define PWINFO_TRNORIGTIME		   0x73
#define PWINFO_CNCDSPMSG	      0x74	
#define PWINFO_CNCPPMSG	         0x75	
#define PWINFO_OPERABORTED	      0x76
#define PWINFO_TRNORIGLOCREF	   0x78
#define PWINFO_AUTHSYSTEXTENDED  0x87
#define PWINFO_CARDENTMODE	      0xC0
#define PWINFO_CARDFULLPAN		   0xC1
#define PWINFO_CARDEXPDATE		   0xC2
#define PWINFO_CARDNAMESTD       0xC4
#define PWINFO_PRODNAMEDESC      0xC5
#define PWINFO_CARDPARCPAN			0xC8
#define PWINFO_CHOLDVERIF	      0xCF
#define PWINFO_EMVRESPCODE       0xD6
#define PWINFO_AID               0xD8
#define PWINFO_SMSGCHOLDER	      0xE2 
#define PWINFO_SMSGMERCH	      0xE3 
#define PWINFO_SMSGTOUTSEC	      0xE4 
#define PWINFO_BARCODENTMODE	   0xE9
#define PWINFO_BARCODE	         0xEA
#define PWINFO_SMSGLOCAL         0xEB 
#define PWINFO_MERCHADDDATA1	   0xF0
#define PWINFO_MERCHADDDATA2	   0xF1	   
#define PWINFO_MERCHADDDATA3	   0xF2	
#define PWINFO_MERCHADDDATA4	   0xF3	
#define PWINFO_RCPTPRN           0xF4
#define PWINFO_AUTHMNGTUSER      0xF5
#define PWINFO_AUTHTECHUSER      0xF6
#define PWINFO_MERCHNAMERCPT     0xFA
#define PWINFO_PRODESTABRCPT     0xFB
#define PWINFO_PRODCLIRCPT       0xFC
#define PWINFO_EMVCRYPTTYPE      0xFD
#define PWINFO_PAYMNTTYPE        0x1F21
#define PWINFO_GRAPHICRCPHEADER  0x1F36
#define PWINFO_GRAPHICRCPFOOTER  0x1F37
#define PWINFO_CHOLDERNAME       0x1F38
#define PWINFO_MERCHNAMEPDC      0x1F39
#define PWINFO_TRANSACDESCRIPT   0x1F40
#define PWINFO_ARQC              0x1F41
#define PWINFO_DEFAULTCARDPARCPAN 0x1F42
#define PWINFO_RCPTADDINFOESTABCLI	0x1F44
#define PWINFO_RCPTADDINFOCLI	      0x1F45
#define PWINFO_RCPTADDINFOESTAB	   0x1F46
#define PWINFO_SOFTDESCRIPTOR    0x1F43
#define PWINFO_SPLITPAYMENT      0x1F59
#define PWINFO_AUTHPOSQRCODE     0x1F77
#define PWINFO_WALLETUSERIDTYPE  0x1F81
#define PWINFO_RCPTECVID         0x1F91
#define PWINFO_USINGPINPAD       0x7F01
#define PWINFO_PPCOMMPORT        0x7F02
#define PWINFO_LOCALINFO2        0x7F03
#define PWINFO_IDLEPROCTIME      0x7F04
#define PWINFO_PNDAUTHSYST	      0x7F05
#define PWINFO_PNDVIRTMERCH	   0x7F06
#define PWINFO_PNDREQNUM	      0x7F07
#define PWINFO_PNDAUTLOCREF	   0x7F08
#define PWINFO_PNDAUTEXTREF	   0x7F09
#define PWINFO_LOCALINFO1        0x7F0A
#define PWINFO_SERVERPND	      0x7F0B
#define PWINFO_COMMODE           0x7F0C
#define PWINFO_COMMPROT          0x7F0D
#define PWINFO_DIALMODE          0x7F0E
#define PWINFO_PRINUMBER         0x7F0F
#define PWINFO_SECNUMBER         0x7F10
#define PWINFO_DIALPREFIX        0x7F11
#define PWINFO_DIALWAITTIME      0x7F12
#define PWINFO_MODSPEED          0x7F13
#define PWINFO_TPDU              0x7F14
#define PWINFO_PPINFO            0x7F15
#define PWINFO_RESULTID          0x7F16
#define PWINFO_DPSCHECKOUT1      0x7F17
#define PWINFO_DPSCHECKOUT2      0x7F18
#define PWINFO_DPSCHECKOUT3      0x7F19
#define PWINFO_DPSCHECKOUT4      0x7F1A
#define PWINFO_DPSCHECKOUT5      0x7F1B
#define PWINFO_CTLSCAPTURE       0x7F1C
#define PWINFO_CHOLDERGRARCP     0x7F1D
#define PWINFO_MERCHGRARCP       0x7F1E
#define PWINFO_AUTADDRESS        0x7F1F
#define PWINFO_APN               0x7F20
#define PWINFO_LIBVERSION        0x7F21
#define PWINFO_TSTKEYTYPE        0x7F30
#define PWINFO_TSTKEYATR         0x7F31
#define PWINFO_TKPINDUKPT3DES    0x7F32
#define PWINFO_TKPINMK3DES       0x7F33
#define PWINFO_TKPINMKDES        0x7F34
#define PWINFO_TKDADOSDUKPT3DES  0x7F35
#define PWINFO_TKDADOSMK3DES     0x7F36
#define PWINFO_TKDADOSMKDES      0x7F37
#define PWINFO_DSPTSTKEY         0x7F38
#define PWINFO_GETKSNPIN         0x7F39
#define PWINFO_GETKSNDATA        0x7F40
#define PWINFO_PINDUKPT3DESNAME  0x7F41
#define PWINFO_PINMK3DESNAME     0x7F42
#define PWINFO_PINMKDESNAME      0x7F43
#define PWINFO_DATADUKPT3DESNAME 0x7F44
#define PWINFO_DATAMK3DESNAME    0x7F45
#define PWINFO_DATAMKDESNAME     0x7F46
#define PWINFO_SERNUM            0x7F47
#define PWINFO_MACADDR           0x7F48
#define PWINFO_IMEI              0x7F49
#define PWINFO_IPADDRESS         0x7F4A
#define PWINFO_SSID_IDX          0x7F4B
#define PWINFO_DNSSERVER_P       0x7F4C
#define PWINFO_DNSSERVER_S       0x7F4D
#define PWINFO_OSVERSION         0x7F4E
#define PWINFO_APPDOWNLOADVER    0x7F4F 
#define PWINFO_DSPQRPREF         0x7F50

#define PWINFO_GRAPHICRCP        0x9F12
#define PWINFO_OPERATIONORIG     0x9F17
#define PWINFO_DUEAMNT           0xBF06
#define PWINFO_READJUSTEDAMNT    0xBF09
#define PWINFO_DATETIMERCPT      0xBF0E
#define PWINFO_TRNORIGDATETIME   0xBF0F
#define PWINFO_SMSPHONE          0xBF6D
#define PWINFO_UNIQUEID          0xBF90
#define PWINFO_TRNRESULT         0xBF91
#define PWINFO_RMCCAUSE          0xBF92

/* Faixa reservada para tags de transmissão (PTI), evitando conflitos com as tags da lib   */
/* Devido a padronização, a faixa foi criada para evitar eventuais conflitos de tags entre */
/* comunicação do POS e PTI.                                                               */
#define PWPTI_FIRSTTAG           0x80 /* Início do range para Tags na PTI */
#define PWPTI_RESULT             0x81
#define PWPTI_MSGTYPE            0x82
#define PWPTI_TIMEOUT            0x83
#define PWPTI_POSMACADD          0x84
#define PWPTI_VERSION            0x85
#define PWPTI_POSMODEL           0x86
#define PWPTI_POSSERNO           0x87
#define PWPTI_DSPMSG             0x88
#define PWPTI_KEY                0x89
#define PWPTI_PROMPT             0x8A
#define PWPTI_DATA               0x8B
#define PWPTI_FORMAT             0x8C
#define PWPTI_LENMIN             0x8D
#define PWPTI_LENMAX             0x8E
#define PWPTI_FROMLEFT           0x8F
#define PWPTI_MASK               0x90
#define PWPTI_ALPHA              0x91
#define PWPTI_NUMITENS           0x92
#define PWPTI_MENUITENS          0x93
#define PWPTI_SELECTION          0x94
#define PWPTI_BEEPTYPE           0x95
#define PWPTI_PRNTEXT            0x96
#define PWPTI_AUTVERSION         0x97
#define PWPTI_AUTDEVELOP         0x98
#define PWPTI_CAPTURELINE        0x99
#define PWPTI_IDLEMSG            0x9A
#define PWPTI_IDLETIME           0x9B
#define PWPTI_RCPTTOPRN          0x9C
#define PWPTI_MUXTERMINALID      0x9D
#define PWPTI_AUTCAP             0x9E
#define PWPTI_EFTRESMSG          0xA0
#define PWPTI_EFTCONF            0xA1
#define PWPTI_CODESYMBOL         0xA2
#define PWPTI_BARCODEERR         0xA3
#define PWPTI_RESPCODE           0xA4
#define PWPTI_COMMODE            0xA5
#define PWPTI_CLIVERSION         0xA6
#define PWPTI_EFTCONFREQNUM      0xA7
#define PWPTI_EFTCONFLOCREF      0xA8
#define PWPTI_EFTCONFEXTREF      0xA9
#define PWPTI_EFTCONFVIRTMERCH   0xAA
#define PWPTI_EFTCONFAUTSYST     0xAB
#define PWPTI_LASTTAG            0xBF /* Fim do range para Tags na PTI */


// Tipos de operação, utilizados na função PW_iGetOperations
#define PWOPTYPE_ADMIN           1
#define PWOPTYPE_SALE            2

// Tipo de chave de criiptografia wi-fi
#define PW_WIFITYPE_UNKNOWN  0
#define PW_WIFITYPE_NONE     1
#define PW_WIFITYPE_WPA      2
#define PW_WIFITYPE_WEP      3
#define PW_WIFITYPE_WEP64    4
#define PW_WIFITYPE_WEP128   5
#define PW_WIFITYPE_WPA2     6

// Definições para chave de criptografia Wi-Fi
#define PW_WPAKEY_ASC_MIN_SIZE    8
#define PW_WPAKEY_ASC_MAX_SIZE   63
#define PW_WEP64KEY_ASC_SIZE      5
#define PW_WEP128KEY_ASC_SIZE    13
#define PW_WIFIKEY_ASC_MAX_SIZE  PW_WPAKEY_ASC_MAX_SIZE
#define PW_SSID_MAX_SIZE         32

typedef struct{
   Word     wIdentificador; 
   Byte     bTipoDeDado;
   char     szPrompt[84];
   Byte     bNumOpcoesMenu;
   char     vszTextoMenu[PWMENU_MAXINTENS][41];
   char     vszValorMenu[PWMENU_MAXINTENS][256];
   char     szMascaraDeCaptura[41];
   Byte     bTiposEntradaPermitidos;
   Byte     bTamanhoMinimo;
   Byte     bTamanhoMaximo;
   Uint32   ulValorMinimo;
   Uint32   ulValorMaximo;
   Byte     bOcultarDadosDigitados;
   Byte     bValidacaoDado;
   Byte     bAceitaNulo;
   char     szValorInicial[41];
   Byte     bTeclasDeAtalho;
   char     szMsgValidacao[84];
   char     szMsgConfirmacao[84];
   char     szMsgDadoMaior[84];
   char     szMsgDadoMenor[84];
   Byte     bCapturarDataVencCartao;
   Uint32   ulTipoEntradaCartao;
   Byte     bItemInicial;
   Byte     bNumeroCapturas;
   char     szMsgPrevia[84];
   Byte     bTipoEntradaCodigoBarras;
   Byte     bOmiteMsgAlerta;
   Byte     bIniciaPelaEsquerda;
   Byte     bNotificarCancelamento;
   Byte     bAlinhaPelaDireita;
} PW_GetData; 

typedef struct {
   Byte  bOperType;
   char  szText[21];
   char  szValue[21];
} PW_Operations;

typedef struct{
   Byte bOperType;
   char szOperName[21];
   char szAuthSyst[21];
   char szValue[21];
   Bool fAuthPreferential;
} PW_OperationsEx;

/******************************************/
/* Public functions - Exported within DLL */
/******************************************/
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
extern Int16 PW_EXPORT PW_iInit (const char* pszWorkingDir);

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
extern Int16 PW_EXPORT PW_iNewTransac (Byte bOper);

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
extern Int16 PW_EXPORT PW_iAddParam (Word wParam, const char *pszValue);

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
extern Int16 PW_EXPORT PW_iExecTransac (PW_GetData vstParam[], Int16 *piNumParam);

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
extern Int16 PW_EXPORT PW_iGetResult (Int16 iInfo, char *pszData, Uint32 ulDataSize);

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
extern Int16 PW_EXPORT PW_iConfirmation (Uint32 ulResult, const char* pszReqNum, const char* pszLocRef, const char* pszExtRef,
   const char* pszVirtMerch, const char* pszAuthSyst);

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
extern Int16 PW_EXPORT PW_iIdleProc(void);

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
extern Int16 PW_EXPORT PW_iGetOperations(Byte bOperType, PW_Operations vstOperations[], Int16 *piNumOperations);

/*=========================================================================================================*\
 Funcao     :  PW_iGetOperationsEx

 Descricao  :  Esta função pode ser chamada para obter quais operações o Pay&Go Web disponibiliza no momento, 
               sejam elas administrativas, de venda ou ambas. Retorna além da operação, o nome do sistema autorizador que oferece a opção.

 Entradas   :              bOperType	      Soma dos tipos de operação a serem incluídos na estrutura de 
                                             retorno (PWOPTYPE_xxx).	
                           piNumOperations	Número máximo de operações que pode ser retornado. (Deve refletir 
                                             o tamanho da área de memória apontada por pvstOperations).
                           iStructSize	      Tamanho da estrutura PW_OperationsEx.
 
 Saídas     :              piNumOperations	Número de operações disponíveis no Pay&Go WEB.
                           vstOperations	   Lista das operações disponíveis e suas características.

 
 Retorno    :  PWRET_OK	         Operação realizada com êxito.
               PWRET_DLLNOTINIT	Não foi executado PW_iInit.
               PWRET_NOTINST	   É necessário efetuar uma transação de Instalação.
               Outro	            Outro erro de execução (ver “10. Códigos de retorno”, página 40). Uma mensagem 
                                 de erro pode ser obtida através da função PW_iGetResult (PWINFO_RESULTMSG).
\*=========================================================================================================*/ 
extern Int16 PW_EXPORT PW_iGetOperationsEx(Byte bOperType, PW_OperationsEx vstOperations[], Int16 iStructSize, 
   Int16 *piNumOperations);


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
extern Int16 PW_EXPORT PW_iPPEventLoop (char *pszDisplay, Uint32 ulDisplaySize);

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
extern Int16 PW_EXPORT PW_iPPAbort(void);

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
extern Int16 PW_EXPORT PW_iPPGetCard (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPGetPIN (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPGetData (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPGoOnChip (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPFinishChip (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPConfirmData (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iPPRemoveCard(void);

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
extern Int16 PW_EXPORT PW_iPPDisplay(const char *pszMsg);

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
extern Int16 PW_EXPORT PW_iPPWaitEvent(Uint32 *pulEvent);

/*===========================================================================*\
 Funcao   : PW_iPPGenericCMD

 Descricao  :  Realiza comando genérico de PIN-pad.
 
 Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                        (índice do dado no vetor pvstParam).

 Saidas     :  Não há.
 
 Retorno    :  PWRET_xxx.
\*===========================================================================*/
extern Int16 PW_EXPORT PW_iPPGenericCMD (Uint16 uiIndex);

/*===========================================================================*\
 Funcao     : PW_iPPPositiveConfirmation

 Descricao  :  Realiza a confirmação positiva de um dado, ou um bloco de dados,
                no PIN-pad
 
 Entradas   :  uiIndex	Índice (iniciado em 0) do dado solicitado na última execução de PW_iExecTransac 
                        (índice do dado no vetor pvstParam).

 Saidas     :  Não há.
 
 Retorno    :  PWRET_xxx.
\*===========================================================================*/
extern Int16 PW_EXPORT PW_iPPPositiveConfirmation (Uint16 uiIndex);

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
extern Int16 PW_EXPORT PW_iTransactionInquiry (const char *pszXmlRequest, char* pszXmlResponse, Uint32 ulXmlResponseLen);

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
extern Int16 PW_EXPORT PW_iPPGetUserData(Uint16 uiMessageId, Byte bMinLen, Byte bMaxLen, Int16 iToutSec, char *pszData);

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
extern Int16 PW_EXPORT PW_iPPGetPINBlock(Byte bKeyID, const char* pszWorkingKey, Byte bMinLen, 
   Byte bMaxLen, Int16 iToutSec, const char* pszPrompt, char* pszData);

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
extern Int16 PW_EXPORT PW_iWaitConfirmation(void);

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
extern Int16 PW_EXPORT PW_iPPTestKey (Uint16 uiIndex);
#endif












