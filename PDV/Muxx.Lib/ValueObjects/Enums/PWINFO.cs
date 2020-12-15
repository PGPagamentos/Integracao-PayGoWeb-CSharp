using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muxx.Lib.ValueObjects.Enums
{
   /// <summary>
   /// Tipos de dados
   /// </summary>
   public enum PWINFO : ushort
   {
      PWINFO_OPERATION = 0x02,
      PWINFO_PPPPWD = 0x03,
      PWINFO_SENHASIM = 0x04,
      PWINFO_AUTIP = 0x05,
      PWINFO_USINGAUT = 0x06,
      PWINFO_AUTPORT = 0x07,
      PWINFO_ADDRMODE = 0x08,
      PWINFO_LOCALIP = 0x09,
      PWINFO_GATEWAY = 0x0A,
      PWINFO_SUBNETMASK = 0x0B,
      PWINFO_SSID = 0x0C,
      PWINFO_WIFITYPE = 0x0D,
      PWINFO_WIFIKEY = 0x0E,
      PWINFO_COMMTYPE = 0x0F,
      //Livre!!
      PWINFO_POSID = 0x11,
      /// <summary>
      /// Nome do aplicativo de Automação.
      /// </summary>
      PWINFO_AUTNAME = 0x15,
      /// <summary>
      /// Versão do aplicativo de Automação.
      /// </summary>
      PWINFO_AUTVER = 0x16,
      /// <summary>
      /// Empresa desenvolvedora do aplicativo de
      /// Automação.
      /// </summary>
      PWINFO_AUTDEV = 0x17,
      PWINFO_DESTTCPIP = 0x1B,
      PWINFO_MERCHCNPJCPF = 0x1C,
      /// <summary>
      /// Capacidades da Automação (soma dos valores
      /// abaixo):<br/>
      /// 1: funcionalidade de troco/saque;<br/>
      /// 2: funcionalidade de desconto;<br/>
      /// 4: valor fixo, sempre incluir;<br/>
      /// 8: impressão das vias diferenciadas do comprovante
      ///    para Cliente/Estabelecimento;<br/>
      /// 16: impressão do cupom reduzido;<br/>
      /// 32: utilização de saldo total do voucher para
      ///     abatimento do valor da compra;<br/>
      /// 64: remoção do cartão do pinpad;<br/>
      /// 128: exibição de mensagem no checkout;<br/>
      /// 256: exibição de QR Code no checkout.<br/>
      /// </summary>
      PWINFO_AUTCAP = 0x24,
      PWINFO_TOTAMNT = 0x25,
      PWINFO_CURRENCY = 0x26,
      PWINFO_CURREXP = 0x27,
      PWINFO_FISCALREF = 0x28,
      PWINFO_CARDTYPE = 0x29,
      PWINFO_PRODUCTNAME = 0x2A,
      PWINFO_MENU1 = 0x2D, //MUXTAG
      PWINFO_MENU2 = 0x2E, //MUXTAG
      PWINFO_MENU3 = 0x2F, //MUXTAG
      PWINFO_DATETIME = 0x31,
      PWINFO_REQNUM = 0x32,
      PWINFO_AUTHSYST = 0x35,
      PWINFO_VIRTMERCH = 0x36,
      PWINFO_AUTMERCHID = 0x38,
      PWINFO_PHONEFULLNO = 0x3A,
      PWINFO_FINTYPE = 0x3B,
      PWINFO_INSTALLMENTS = 0x3C,
      PWINFO_INSTALLMDATE = 0x3D,
      PWINFO_PRODUCTID = 0x3E,
      PWINFO_RESULTMSG = 0x42,
      PWINFO_CNFREQ = 0x43,
      PWINFO_AUTLOCREF = 0x44,
      PWINFO_AUTEXTREF = 0x45,
      PWINFO_AUTHCODE = 0x46,
      PWINFO_AUTRESPCODE = 0x47,
      /// <summary>
      /// Data/hora da transação para o Provedor, formato “AAAAMMDDhhmmss”.
      /// </summary>
      PWINFO_AUTDATETIME = 0x48,
      PWINFO_DISCOUNTAMT = 0x49,
      PWINFO_CASHBACKAMT = 0x4A,
      PWINFO_CARDNAME = 0x4B,
      PWINFO_ONOFF = 0x4C,
      PWINFO_BOARDINGTAX = 0x4D,
      PWINFO_TIPAMOUNT = 0x4E,
      PWINFO_INSTALLM1AMT = 0x4F,
      PWINFO_INSTALLMAMNT = 0x50,
      PWINFO_RCPTFULL = 0x52,
      PWINFO_RCPTMERCH = 0x53,
      PWINFO_RCPTCHOLDER = 0x54,
      PWINFO_RCPTCHSHORT = 0x55,
      PWINFO_NOSECCODEREASON = 0x56, //MUXTAG
      PWINFO_TRNORIGDATE = 0x57,
      PWINFO_TRNORIGNSU = 0x58,
      PWINFO_SALDOVOUCHER = 0x59,
      PWINFO_TRNORIGAMNT = 0x60,
      PWINFO_TRNORIGAUTH = 0x62,
      PWINFO_LANGUAGE = 0x6C,
      PWINFO_PROCESSMSG = 0x6F,
      PWINFO_TRNORIGREQNUM = 0x72,
      PWINFO_TRNORIGTIME = 0x73,
      PWINFO_CNCDSPMSG = 0x74,
      PWINFO_CNCPPMSG = 0x75,
      PWINFO_OPERABORTED = 0x76,
      /// <summary>
      /// Referência local da transação original, no caso de um cancelamento.
      /// </summary>
      PWINFO_TRNORIGLOCREF = 0x78,
      PWINFO_AUTHSYSTEXTENDED = 0x87,
      PWINFO_CARDENTMODE = 0xC0,
      PWINFO_CARDFULLPAN = 0xC1,
      PWINFO_CARDEXPDATE = 0xC2,
      PWINFO_CARDNAMESTD = 0xC4,
      PWINFO_PRODNAMEDESC = 0xC5,
      PWINFO_CARDSECCODE = 0xC7, //MUXTAG
      PWINFO_CARDPARCPAN = 0xC8,
      PWINFO_CHOLDVERIF = 0xCF,
      PWINFO_EMVRESPCODE = 0xD6,
      PWINFO_AID = 0xD8,
      PWINFO_CRYPTRESULTS = 0XDC, //MUXTAG
      PWINFO_LASTFOURCARDDIG = 0xE0, //MUXTAG
      PWINFO_SMSGCHOLDER = 0xE2,
      PWINFO_SMSGMERCH = 0xE3,
      PWINFO_SMSGTOUTSEC = 0xE4,
      PWINFO_BARCODENTMODE = 0xE9,
      PWINFO_BARCODE = 0xEA,
      PWINFO_SMSGLOCAL = 0xEB,
      PWINFO_PAYMENTTYPE = 0XEC, //MUXTAG
      PWINFO_MERCHADDDATA1 = 0xF0,
      PWINFO_MERCHADDDATA2 = 0xF1,
      PWINFO_MERCHADDDATA3 = 0xF2,
      PWINFO_MERCHADDDATA4 = 0xF3,
      PWINFO_RCPTPRN = 0xF4,
      PWINFO_AUTHMNGTUSER = 0xF5,
      PWINFO_AUTHTECHUSER = 0xF6,
      PWINFO_MERCHNAMERCPT = 0xFA,
      PWINFO_PRODESTABRCPT = 0xFB,
      PWINFO_PRODCLIRCPT = 0xFC,
      PWINFO_EMVCRYPTTYPE = 0xFD,

      PWINFO_ITEMCODE = 0X1F02, //MUXTAG
      PWINFO_ITEMDESC = 0X1F03, //MUXTAG
      PWINFO_ITEMQTY = 0X1F04, //MUXTAG
      PWINFO_DRIVERID = 0X1F1C, //MUXTAG
      PWINFO_DRIVERPWD = 0X1F1D, //MUXTAG
      PWINFO_CARKM = 0X1F1E, //MUXTAG
      PWINFO_PAYMNTTYPE = 0x1F21,
      PWINFO_LICENSEPLATE = 0X1F23, //MUXTAG
      PWINFO_IDORDERSERVICE = 0X1F24, //MUXTAG
      PWINFO_CODVERSION = 0X1F25, //MUXTAG
      PWINFO_IDSAC = 0X1F26, //MUXTAG
      PWINFO_CODCPS = 0X1F27, //MUXTAG
      PWINFO_VOUCHERID = 0X1F29, //MUXTAG
      PWINFO_GRAPHICRCPHEADER = 0x1F36,
      PWINFO_GRAPHICRCPFOOTER = 0x1F37,
      PWINFO_CHOLDERNAME = 0x1F38,
      PWINFO_MERCHNAMEPDC = 0x1F39,
      PWINFO_TRANSACDESCRIPT = 0x1F40,
      PWINFO_ARQC = 0x1F41,
      PWINFO_DEFAULTCARDPARCPAN = 0x1F42,
      PWINFO_SOFTDESCRIPTOR = 0x1F43,
      PWINFO_RCPTADDINFOESTABCLI = 0x1F44,
      PWINFO_RCPTADDINFOCLI = 0x1F45,
      PWINFO_RCPTADDINFOESTAB = 0x1F46,
      PWINFO_SPLITPAYMENT = 0x1F59,
      PWINFO_COBVENCTO = 0X1F60, //MUXTAG
      PWINFO_COBCEDENTE = 0X1F61, //MUXTAG
      PWINFO_COBDTCONTABIL = 0X1F62, //MUXTAG
      PWINFO_COBVLRDOC = 0X1F63, //MUXTAG
      PWINFO_COBVLRDESC = 0X1F64, //MUXTAG
      PWINFO_COBVLRABAT = 0X1F65, //MUXTAG
      PWINFO_COBVLRBONIF = 0X1F66, //MUXTAG
      PWINFO_COBVLRMULTA = 0X1F67, //MUXTAG
      PWINFO_COBVLRJUROS = 0X1F68, //MUXTAG
      PWINFO_COBVLRDEVIDO = 0X1F69, //MUXTAG
      PWINFO_COBCNPJCPFPROP = 0X1F6A, //MUXTAG
      PWINFO_COBCNPJCPFFAV = 0X1F6B, //MUXTAG
      PWINFO_COBCNPJCPFPORT = 0X1F6C, //MUXTAG
      PWINFO_AUTHPOSQRCODE = 0x1F77,
      PWINFO_WALLETUSERIDTYPE = 0x1F81,
      PWINFO_RCPTECVID = 0x1F91,

      PWINFO_USINGPINPAD = 0x7F01,
      PWINFO_PPCOMMPORT = 0x7F02,
      PWINFO_LOCALINFO2 = 0x7F03,
      PWINFO_IDLEPROCTIME = 0x7F04,
      PWINFO_PNDAUTHSYST = 0x7F05,
      PWINFO_PNDVIRTMERCH = 0x7F06,
      PWINFO_PNDREQNUM = 0x7F07,
      PWINFO_PNDAUTLOCREF = 0x7F08,
      PWINFO_PNDAUTEXTREF = 0x7F09,
      PWINFO_LOCALINFO1 = 0x7F0A,
      PWINFO_SERVERPND = 0x7F0B,
      PWINFO_COMMODE = 0x7F0C,
      PWINFO_COMMPROT = 0x7F0D,
      PWINFO_DIALMODE = 0x7F0E,
      PWINFO_PRINUMBER = 0x7F0F,
      PWINFO_SECNUMBER = 0x7F10,
      PWINFO_DIALPREFIX = 0x7F11,
      PWINFO_DIALWAITTIME = 0x7F12,
      PWINFO_MODSPEED = 0x7F13,
      PWINFO_TPDU = 0x7F14,
      PWINFO_PPINFO = 0x7F15,
      PWINFO_RESULTID = 0x7F16,
      PWINFO_DPSCHECKOUT1 = 0x7F17,
      PWINFO_DPSCHECKOUT2 = 0x7F18,
      PWINFO_DPSCHECKOUT3 = 0x7F19,
      PWINFO_DPSCHECKOUT4 = 0x7F1A,
      PWINFO_DPSCHECKOUT5 = 0x7F1B,
      PWINFO_CTLSCAPTURE = 0x7F1C,
      PWINFO_CHOLDERGRARCP = 0x7F1D,
      PWINFO_MERCHGRARCP = 0x7F1E,
      PWINFO_AUTADDRESS = 0x7F1F,
      PWINFO_APN = 0x7F20,
      PWINFO_LIBVERSION = 0x7F21,
      PWINFO_TSTKEYTYPE = 0x7F30,
      PWINFO_TSTKEYATR = 0x7F31,
      PWINFO_TKPINDUKPT3DES = 0x7F32,
      PWINFO_TKPINMK3DES = 0x7F33,
      PWINFO_TKPINMKDES = 0x7F34,
      PWINFO_TKDADOSDUKPT3DES = 0x7F35,
      PWINFO_TKDADOSMK3DES = 0x7F36,
      PWINFO_TKDADOSMKDES = 0x7F37,
      PWINFO_DSPTSTKEY = 0x7F38,
      PWINFO_GETKSNPIN = 0x7F39,
      PWINFO_GETKSNDATA = 0x7F40,
      PWINFO_PINDUKPT3DESNAME = 0x7F41,
      PWINFO_PINMK3DESNAME = 0x7F42,
      PWINFO_PINMKDESNAME = 0x7F43,
      PWINFO_DATADUKPT3DESNAME = 0x7F44,
      PWINFO_DATAMK3DESNAME = 0x7F45,
      PWINFO_DATAMKDESNAME = 0x7F46,
      PWINFO_SERNUM = 0x7F47,
      PWINFO_MACADDR = 0x7F48,
      PWINFO_IMEI = 0x7F49,
      PWINFO_IPADDRESS = 0x7F4A,
      PWINFO_SSID_IDX = 0x7F4B,
      PWINFO_DNSSERVER_P = 0x7F4C,
      PWINFO_DNSSERVER_S = 0x7F4D,
      PWINFO_OSVERSION = 0x7F4E,
      PWINFO_APPDOWNLOADVER = 0x7F4F,
      /// <summary>
      /// Caso a exibição de QR Code seja suportada pela
      /// Automação Comercial e pelo PIN-Pad, indica a
      /// preferência do local de exibição:<br/>
      /// 1: exibe no PIN-Pad;<br/>
      /// 2: exibe no checkout;<br/>
      /// OBS: Caso esse campo não seja informado pela
      /// automação e o ponto de captura esteja configurado
      /// como autoatendimento, o QR Code é exibido no
      /// checkout. Caso contrário, é exibido no pinpad.
      /// </summary>
      PWINFO_DSPQRPREF = 0x7F50,

      PWINFO_GRAPHICRCP = 0x9F12,
      PWINFO_OPERATIONORIG = 0x9F17,

      PWINFO_PAYMENTPLAY = 0XBF01, //MUXTAG
      PWINFO_BILLCYCLES = 0XBF02, //MUXTAG
      PWINFO_DTEMBARQUE = 0XBF03, //MUXTAG
      PWINFO_DTINSTALLMENTS = 0XBF04, //MUXTAG
      PWINFO_MNTINTALLMENTS = 0XBF05, //MUXTAG
      PWINFO_DUEAMNT = 0xBF06,
      PWINFO_ITEMNUMBER = 0XBF07, //MUXTAG
      PWINFO_TERMOFPAYMENT = 0XBF08, //MUXTAG
      PWINFO_READJUSTEDAMNT = 0xBF09,
      PWINFO_CNPJCPFPORTADOR = 0XBF0A, //MUXTAG
      PWINFO_CNPJCPFLOJISTA = 0XBF0B, //MUXTAG
      PWINFO_COBVLRDOCALT = 0XBF0C, //MUXTAG
      PWINFO_DATETIMERCPT = 0xBF0E,
      PWINFO_TRNORIGDATETIME = 0xBF0F,
      PWINFO_SMSPHONE = 0xBF6D,
      /// <summary>
      /// ID único da transação armazenada no banco de dados
      /// </summary>
      PWINFO_UNIQUEID = 0xBF90,
      PWINFO_TRNRESULT = 0xBF91,
      PWINFO_RMCCAUSE = 0xBF92,
   }
}