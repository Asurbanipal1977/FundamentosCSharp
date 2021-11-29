using System;
using System.Collections.Generic;
using System.Text;

namespace FundamentosCSharp.Models
{
    public class Vino : Bebida, IBebidaAlcoholica
    {
        public Vino(int Cantidad, string Nombre = "Vino") : base(Nombre, Cantidad)
        {
        }

        public string Marca { get; set; }
        public int Alcohol { get; set; }

        public void CantidadMax()
        {
            Console.WriteLine("La cantidad máxima son 3 copas");
        }
    }
}
