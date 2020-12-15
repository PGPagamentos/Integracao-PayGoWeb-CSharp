using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Muxx.Lib.ValueObjects.Structs;
using Muxx.UI.Controls.Extend;

namespace Muxx.UI.Controls
{
   public class MenuItem
   {
      public string Texto { get; set; }
      public string Value { get; set; }

      public override string ToString()
      {
         return string.Format("{0}", Texto);
      }
   }

   /// <summary>
   /// Interaction logic for MenuControl.xaml
   /// </summary>
   public partial class CapturaMenuControl : CapturaControl
   {

      #region Constructors

      public CapturaMenuControl()
      {
         InitializeComponent();
      }

      #endregion

      #region Public Methods

      public override void Bind(PW_GetData pw_GetData)
      {
         base.Bind(pw_GetData);

         List<MenuItem> itensMenu = new List<MenuItem>();
         MenuItem intemMenu;

         if (string.IsNullOrEmpty(Prompt))
            dspDisplay.Visibility = Visibility.Collapsed;
         else
            dspDisplay.Bind(Prompt, false, true);

         if (pw_GetData.bNumOpcoesMenu > 0)
         {
            for (int i = 0; i < pw_GetData.bNumOpcoesMenu; i++)
            {
               intemMenu = new MenuItem();

               //bTeclasDeAtalho seja igual a 1, 
               //a Automação deve acrescentar na frente do descritivo de cada opção um dígito numérico 
               //correspondendo à posição (iniciada em 1) da opção na lista. 
               if (TeclasDeAtalho == 1)
                  intemMenu.Texto = string.Format("{0}.{1}", i + 1, pw_GetData.vszTextoMenu[i].szTextoMenu);
               else
                  intemMenu.Texto = pw_GetData.vszTextoMenu[i].szTextoMenu.ToString();
               intemMenu.Value = pw_GetData.vszValorMenu[i].szValorMenu.ToString();

               itensMenu.Add(intemMenu);
            }

            lstbMenu.ItemsSource = itensMenu;

            try
            {
               //bItemInicial contém o índice (iniciado em 0) da opção padrão
               lstbMenu.SelectedIndex = ItemInicial;
            }
            catch (Exception)
            {
               lstbMenu.SelectedIndex = 0;
            }

            //Pre-generates item containers
            lstbMenu.UpdateLayout();

            var listBoxItem = (ListBoxItem)lstbMenu
                .ItemContainerGenerator
                .ContainerFromItem(lstbMenu.SelectedItem);

            listBoxItem.Focus();
            if (!listBoxItem.IsKeyboardFocused)
            {
               Keyboard.Focus(listBoxItem);
            }
         }
      }

      #endregion

      #region Events

      [ObfuscationAttribute(Exclude = true)]
      private void lstbMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         //base.ParamCapturado(((MenuItem)(lstbMenu.SelectedItem)).Value);
      }

      [ObfuscationAttribute(Exclude = true)]
      private void lstbMenu_MouseUp(object sender, MouseButtonEventArgs e)
      {
         base.ParamCapturado(((MenuItem)(lstbMenu.SelectedItem)).Value);
      }

      [ObfuscationAttribute(Exclude = true)]
      private void CapturaControl_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         e.Handled = true;

         switch (e.Key)
         {
            case Key.D1:
            case Key.NumPad1:
               lstbMenu.SelectedIndex = 0;
               break;
            case Key.D2:
            case Key.NumPad2:
               lstbMenu.SelectedIndex = 1;
               break;
            case Key.D3:
            case Key.NumPad3:
               lstbMenu.SelectedIndex = 2;
               break;
            case Key.D4:
            case Key.NumPad4:
               lstbMenu.SelectedIndex = 3;
               break;
            case Key.D5:
            case Key.NumPad5:
               lstbMenu.SelectedIndex = 4;
               break;
            case Key.D6:
            case Key.NumPad6:
               lstbMenu.SelectedIndex = 5;
               break;
            case Key.D7:
            case Key.NumPad7:
               lstbMenu.SelectedIndex = 6;
               break;
            case Key.D8:
            case Key.NumPad8:
               lstbMenu.SelectedIndex = 7;
               break;
            case Key.D9:
            case Key.NumPad9:
               lstbMenu.SelectedIndex = 8;
               break;

            case Key.Left:
               lstbMenu.SelectedIndex = 0;
               break;
            case Key.Right:
               lstbMenu.SelectedIndex = lstbMenu.Items.Count - 1;
               break;

            case Key.Down:
               if (lstbMenu.Items.Count == lstbMenu.SelectedIndex + 1)
                  lstbMenu.SelectedIndex = 0;
               else
                  lstbMenu.SelectedIndex += 1;
               break;
            case Key.Up:
               if (lstbMenu.SelectedIndex == 0)
                  lstbMenu.SelectedIndex = lstbMenu.Items.Count - 1;
               else
                  lstbMenu.SelectedIndex -= 1;
               break;

            case Key.Enter:
               base.ParamCapturado(((MenuItem)(lstbMenu.SelectedItem)).Value);
               break;

            default:
               break;
         }
      }

      #endregion

   }
}