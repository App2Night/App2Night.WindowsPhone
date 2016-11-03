using App2Night.APIObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace App2Night.Controller
{
    public class ZwischenController
    {
        public static Party[] VonJsonArrayInArrayMitPartys(JsonArray jsonArray)
        {
            int durchgehen = 20;
            Party[] partys = new Party[20];

            for (int i = 0; i < jsonArray.Count; i++)
            {
                partys[i] = (Party) jsonArray[i];
            }

            return partys;
        }


        
    }
}
