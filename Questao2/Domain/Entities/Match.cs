﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Domain.Entities
{
    public class Match
    {
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public int Team1Goals { get; set; }
        public int Team2Goals { get; set; }
        public int Year { get; set; }
    }
}
