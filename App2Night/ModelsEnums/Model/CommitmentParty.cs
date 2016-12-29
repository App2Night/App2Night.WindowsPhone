using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App2Night.ModelsEnums.Model;
using App2Night.ModelsEnums.Enums;

namespace App2Night.ModelsEnums.Model
{
    public class CommitmentParty
    {
        public EventCommitmentState Teilnahme { get; set; }
        public Party Party { get; set; }
    }
}