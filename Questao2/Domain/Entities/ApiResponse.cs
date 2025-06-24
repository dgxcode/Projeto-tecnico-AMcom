﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2.Domain.Entities
{
    public class ApiResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Total_Pages { get; set; }
        public List<MatchDTO> Data { get; set; }
    }
}
