﻿using System;
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
using App2Night.ModelsEnums.Model;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace App2Night.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FensterReg : Page
    {
        Login neuerNutzer = new Login();

        public FensterReg()
        {
            this.InitializeComponent();
        }

        private void btnZurueck_wechselZuAnmOderReg(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FensterAnmOdReg));
        }

        private async void btnNutzerAnlegen_wechselZuHauptansicht(object sender, RoutedEventArgs e)
        {
            //TODO: CreateUser, wenn Passwoerter gleich
            //"dc2f9fcb-c3df-4b02-6007-08d40f0986a3"
            neuerNutzer.Username = textBoxRegNUTZERNAME.Text;
            neuerNutzer.Email = textBoxRegEMAIL.Text;

            if (pwBoxPASSWORT.Password == pwBoxPASSWORTBEST.Password)
            {
                neuerNutzer.Password = pwBoxPASSWORTBEST.Password;
                string userID = await BackEndCommunication.BackEndComUser.CreateUser(neuerNutzer);

                this.Frame.Navigate(typeof(FensterHauptansicht));
            }
            else
            {
                var message = new MessageDialog("Fehler! Die Passwörter stimmen nicht überein!");
                message.ShowAsync();
            }
        }
    }
}
