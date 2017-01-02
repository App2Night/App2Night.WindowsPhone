using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace App2Night.Views
{
    /// <summary>
    /// Das ist die Startseite. Hier kann der Nutzer wählen, ob er sich anmelden oder registrieren will.
    /// </summary>
    public sealed partial class FensterAnmOdReg : Page
    {
        public FensterAnmOdReg()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Einfacher Wechsel zu FensterAnmelden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Anmelden_wechselZuAnmelden(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmelden));
        }

        /// <summary>
        /// Einfacher Wechsel zu FensterRegistrieren.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registrieren_wechselZuReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterReg));
        }
    }
}
