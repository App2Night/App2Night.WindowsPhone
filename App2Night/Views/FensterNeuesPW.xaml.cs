﻿using App2Night.ModelsEnums.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App2Night.Logik;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterNeuesPW : Page
    {
        public FensterNeuesPW()
        {
            this.InitializeComponent();
        }

        private async void btnBestaetigen_wechselZuAnmelden(object sender, RoutedEventArgs e)
        {
            Login pwLogin = new Login();
            pwLogin.Password =  pwBoxNpwALTESPW.Password;  
            await BackEndComUserLogik.ResetPasswort(pwLogin);

            // TODO: Was nun machen?

            this.Frame.Navigate(typeof(FensterAnmelden));
        }

        private void btnAbbrechen_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }
    }
}
