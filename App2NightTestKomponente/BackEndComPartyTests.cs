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
            // TODO: gültige Daten!
            //temp.Email = "testY@test.de";
            //temp.Password = "hallo1234";
            //temp.Username = "YvetteLa";

            return await BackEndComUserLogik.GetToken(temp);
        }

        [TestMethod]
        public async void PostCreatePartyTest()
        {
            var testParty = new CreatePartyModel()
            {
                PartyName = "Hallo",
                Description = "Test",
                CityName = "Horb am Neckar",
                CountryName = "Deutschland",
                HouseNumber = "15",
                MusicGenre = MusicGenre.Electro,
                PartyDate = DateTime.Today.AddDays(30),
                PartyType = PartyType.Bar,
                StreetName = "Florianstraße",
                ZipCode = "72160"
            };

            var token = await GetToken();

            token = await BackEndComUserLogik.RefreshToken(token);

            var erg = await BackEndComPartyLogik.CreateParty(testParty, token);

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
            Location position = new Location();
            position.CityName = "Horb am Neckar";
            position.CountryName = "Deutschland";
            position.HouseNumber = "15";
            position.StreetName = "Florianstraße";
            position.Zipcode = "72160";
            position.Latitude = 48.445031;
            position.Longitude = 8.696494;

            string erg = await BackEndComPartyLogik.ValidateLocation(position);

            Assert.Equals("200", erg);
        }



    }
}
