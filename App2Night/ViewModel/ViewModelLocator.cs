using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2Night.ViewModel
{
    public class ViewModelLocator  
    {
        public ViewModelLocator()

        {

        } 


        public HauptansichtViewModel Hauptansicht

        {

            get 
            { 
                return new HauptansichtViewModel(); 
            }

        }
    }
}
