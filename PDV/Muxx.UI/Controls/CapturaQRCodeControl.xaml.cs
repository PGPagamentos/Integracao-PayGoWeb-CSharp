using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Muxx.Lib.ValueObjects.Structs;
using Muxx.UI.Controls.Extend;
using QRCoder;

namespace Muxx.UI.Controls
{
   /// <summary>
   /// Interaction logic for CapturaQRCodeControl.xaml
   /// </summary>
   public partial class CapturaQRCodeControl : CapturaControl
   {
      #region Member Variables

      private string _qrCode;
      private string _qrCodeRenderizado;

      #endregion

      #region Public Properties

      public string QRCode
      {
         set { _qrCode = value; }
      }

      #endregion

      #region Constructors

      public CapturaQRCodeControl()
      {
         InitializeComponent();
      }

      #endregion

      #region Private Methods

      [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool DeleteObject([In] IntPtr hObject);

      public static ImageSource ToImageSource(Bitmap bmp)
      {
         var handle = bmp.GetHbitmap();
         try
         {
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
         }
         finally
         {
            DeleteObject(handle);
         }
      }

      #endregion

      #region Public Methods

      public override void Bind(PW_GetData pw_GetData)
      {
         base.Bind(pw_GetData);

         if (string.IsNullOrEmpty(Prompt))
            dspDisplay.Visibility = Visibility.Collapsed;
         else
            dspDisplay.Bind(Prompt, false, true);

         if (_qrCodeRenderizado == null ||
             _qrCode != _qrCodeRenderizado)
         {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(_qrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            imgDisplay.Source = ToImageSource(qrCodeImage);

            _qrCodeRenderizado = _qrCode;
         }
      }

      #endregion

      #region Events

      [ObfuscationAttribute(Exclude = true)]
      private void imgDisplay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
#if DEBUG
         Clipboard.SetText(_qrCodeRenderizado);
#endif
      }

      #endregion
   }
}