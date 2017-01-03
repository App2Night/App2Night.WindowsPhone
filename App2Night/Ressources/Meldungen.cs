using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.Ressources
{
    public static class Meldungen
    {
        public static class Anmeldung
        {
            public static readonly string Erfolg = "Erfolgreich angemeldet. Viel Spaß!";
            public static readonly string Misserfolg = "Keine korrekten Nutzerdaten oder der Aktivierungslink wurde noch nicht bestätigt!";
            public static readonly string UnbekannterFehler = "Es ist ein unerwarteter Fehler aufgetreten. Bitte versuche es später erneut.";
        }

        public static class Registrierung
        {
            public static readonly string SpeicherFehler = "Leider ist ein Fehler beim Speichern der Daten aufgetreten. Bitte versuche, die Anzumelden.";
            public static readonly string ErstellenFehler = "Fehler bei Erstellen des Nutzers!";
            public static readonly string PasswortFehler = "Fehler! Die Passwörter stimmen nicht überein!";
            public static readonly string ErfolgEins = "Eine E-Mail mit Aktivierungslink wurde an die angegebene E-Mailadresse (";
            public static readonly string ErfolgZwei =  ") geschickt.";
        }

        public static class Erstellen
        {
            public static readonly string Erfolg = "Party erfolgreich gespeichert!";
            public static readonly string SpeicherFehler = "Es ist ein Fehler beim Speichern aufgetreten. Bitte versuche es später erneut.";
            public static readonly string UngueltigeEingabe = "Ein oder mehrere Eingaben sind ungültig!\nBeispielsweise wird eine Party in der Vergangenheit angelegt oder die Adresse existiert nicht!";
        }

        public static class Hauptansicht
        {
            public static readonly string Nutzungsbedingungen = "Um Partys anzuzeigen benötigt die App GPS und eine Internetverbindung. Zum Suchen oder Aktualisieren klicke bitte auf die Lupe unten. Viel Spaß!";
            public static readonly string KeinePartysInDerNaehe = "Leider sind keine Partys in deiner Nähe. Erstelle eine eigene Party, vergrößere in den Einstellungen den Suchradius oder versuche es später erneut.";
            public static readonly string AnzeigePartyFehler = "Leider ist ein Fehler aufgetreten. Bitte versuche es erneut";
        }

        public static class UserEinstellungen
        {
            public static readonly string EmailKontakt = "Schreib uns doch unter win.app2night@outlook.de!";
            public static readonly string About = "Diese App wurde entwickelt von Yvette Labastille und Manuela Leopold im Zuge einer Vorlesung an der DHBW Stuttgart Campus Horb.\n" +
                                                    "Verwendet wurde:\n" +
                                                    "- JSON-Framwork von Newtonsoft\n" +
                                                    "- Xam.Plugin.Geolocator von James Montemango\n" +
                                                    "- Maps von Bing\n" +
                                                    // "- Verschlüsselung von Dritten\n" + 
                                                    "- Bibliotheken vom Microsoft";
            public static readonly string SpeicherFehler = "Leider ist ein Fehler beim Speichern der Daten aufgetreten.";
        }

        public static class Anzeige
        {
            public static readonly string AbsicherungLoeschen = "Willst du diese Party wirklich löschen?";
            public static readonly string ErfolgLoeschen = "Party erfolgreich gelöscht.";
            public static readonly string MisserfolgLoeschen = "Party nicht gelöscht.";
            public static readonly string AbbrechenLoeschen = "Aktion abgebrochen";

            public static readonly string ErfolgTeilnahme = "Deine Teilnahme wurde berücksichtigt! Es kann etwas dauern, bis du die Änderungen siehst.";
            public static readonly string ErfolgAbsage = "Deine Absage wurde berücksichtigt! Es kann etwas dauern, bis du die Änderungen siehst.";
            public static readonly string FehlerTeilnahmeAbsage = "Es ist ein Fehler aufgetreten. Bitte versuche es später erneut.";

            public static readonly string ErfolgVormerken = "Diese Party wurde für dich vorgemerkt! Es kann etwas dauern, bis du die Änderungen siehst.";
            public static readonly string ErfolgVergessen = "Diese Party ist nicht mehr vorgemerkt! Es kann etwas dauern, bis du die Änderungen siehst.";
            public static readonly string FehlerVormerkenVergessen = "Es ist ein Fehler aufgetreten. Bitte versuche es später erneut!";

        }
    }
}
