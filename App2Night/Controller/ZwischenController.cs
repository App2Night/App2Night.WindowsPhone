using App2Night.APIObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace App2Night.Controller
{
    public class ZwischenController
    {
        public static StorageFolder lokalerOrdner = ApplicationData.Current.LocalFolder;
        public static StorageFile dateiMitAktuellenDaten = null;

        public static void TransferDatenSchreibenZumAnzeigen(ref Party.RootObject partyZuTransferieren)
        {
            string[] partyDaten;

            // Von Party zu Array wandeln
            //partyDaten = partyZuTransferieren.ToString();

            partyDaten = Party.VonPartyRootObjectToArray(partyZuTransferieren);

            string datenPfadDatei = lokalerOrdner.Path + "\\dataFile.txt";

            if (!Directory.Exists(lokalerOrdner.Path))
            {
                Directory.CreateDirectory(lokalerOrdner.Path);
            }
            else
            {
                // nichts?
            }
            if (!File.Exists(datenPfadDatei))
            {
                File.Create(datenPfadDatei);
            }
            else
            {
                File.Delete(datenPfadDatei);
                File.Create(datenPfadDatei);
            }

            // TODO: Zeilenweise Inhalt des Arrays schreiben
            byte[] byteArray = Encoding.ASCII.GetBytes(datenPfadDatei);
            MemoryStream datenStreamSchreiben = new MemoryStream(byteArray);
            StreamWriter streamWriterPartyDatenSchreiben = new System.IO.StreamWriter(datenStreamSchreiben);

            for (int i = 0; i < partyDaten.Length; i++)
            {
                streamWriterPartyDatenSchreiben.WriteLine(partyDaten[i]);
            }


        }


        public static async Task<Party.RootObject> TransferDatenLesenZumAnzeigen()
        {
            try
            {
                dateiMitAktuellenDaten = await lokalerOrdner.GetFileAsync("dataFile.txt");
            }
            catch (Exception)
            {

                throw;

            }
            //Read the first line of dataFile.txt in LocalFolder and store it in a String
            string datenInhalt = await FileIO.ReadTextAsync(dateiMitAktuellenDaten);

            //String in Party.RootObject umwandeln
            Party.RootObject partyAusgelesen = new Party.RootObject();

            //TODO: Zeilenweise lesen und Inhalt in Array packen
            //partyAusgelesen = VonStringZuPartyRootObject(datenInhalt);
            //StreamReader reader = new StreamReader(stream);
            //string text = reader.ReadToEnd();


            return partyAusgelesen;            
        }

        
    }
}
