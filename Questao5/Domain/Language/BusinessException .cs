﻿
namespace Questao5.Domain.Language
{
    public class BusinessException : Exception
    {
        public string Tipo { get; }

        public BusinessException(string message, string tipo) : base(message)
        {
            Tipo = tipo;
        }
    }
}
