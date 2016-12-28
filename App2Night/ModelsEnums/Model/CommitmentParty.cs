using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2Night.ModelsEnums.Model;

namespace App2Night.ModelsEnums.Model
{
    public class CommitmentParty
    {
        public CommitmentParty CommitmentState { get; set; }
        public Party Party { get; set; }
    }
}