using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using App2Night.ModelsEnums.Model;
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using App2Night.ModelsEnums.Enums;
using App2Night.Logik;

namespace App2NightTestKomponente
{
    [TestClass]
    public class BackEndComPartyTests
    {

        public async Task<Token> GetToken()
        {
            Login temp = new Login();
            temp.Username = "mobapp4";
            temp.Email = "mobapp4@you-spam.com";
            temp.Password = "mobapp";

            return await BackEndComUserLogik.GetToken(temp);
        }

        [TestMethod]
        public async void PostCreatePartyTest()
        {
            Party testParty = new Party();

            testParty.PartyName = "PostCreatePartyTest";
            testParty.PartyType = PartyType.Bar;
            testParty.PartyDate = new DateTime(2016, 02, 12, 20, 14, 00);
            testParty.Description = "Diese Party wurde durch einen Test erstellt";
            testParty.MusicGenre = MusicGenre.Pop;
            testParty.Location.CityName = "Horb am Neckar";
            testParty.Location.ZipCode = "72160";
            testParty.Location.StreetName = "Floranstraße";
            testParty.Location.HouseNumber = "15";

            var erg = await BackEndComPartyLogik.CreateParty(testParty);

            Assert.IsTrue(erg);
        }

        [TestMethod]
        public async void GetParties()
        {
            Location position = new Location();
            position.Latitude = 48.445031;
            position.Longitude = 8.696494;
            float radius = 30;

            IEnumerable<Party> partyListe = await BackEndComPartyLogik.GetParties(position, radius);

            Assert.IsNotNull(partyListe);
        }

        [TestMethod]
        public async void ValidateLoc(Location loc)
        {
            Token token = await GetToken();

            Location position = new Location();
            position.CityName = "Horb am Neckar";
            position.CountryName = "Deutschland";
            position.HouseNumber = "15";
            position.StreetName = "Florianstraße";
            position.ZipCode = "72160";
            position.Latitude = 48.445031;
            position.Longitude = 8.696494;

            var erg = await BackEndComPartyLogik.ValidateLocation(position, token);

            Assert.Equals("200", erg);
        }



    }
}
