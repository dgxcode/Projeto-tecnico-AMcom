﻿namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public char TipoMovimento { get; set; } // 'C' ou 'D'
        public decimal Valor { get; set; }
    }
}
