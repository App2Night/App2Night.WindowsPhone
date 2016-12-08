using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2Night.ModelsEnums.Enums;

namespace App2Night.ModelsEnums.Model
{
    public class User
     { 
         public string Name { get; set; } 
         public int Age { get; set; } 
         public Gender Gender { get; set; } 
         public string Email { get; set; } 
         //public ObservableCollection<Party> Events { get; set; } 
         public Location Addresse { get; set; } 
         public Location LastGpsLocation { get; set; } 
    }

}
