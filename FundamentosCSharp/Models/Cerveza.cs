using System;
using System.Collections.Generic;
using System.Text;

namespace FundamentosCSharp.Models
{
    public class Cerveza : Bebida, IBebidaAlcoholica
    {
        public Cerveza() : base("", 0)
        {

        }
        public Cerveza(int Cantidad, string Nombre="Cerveza") : base(Nombre, Cantidad)
        {
        }

        public string Marca { get; set; }
        public int Alcohol { get ; set ; }

        public void CantidadMax()
        {
            Console.WriteLine("La cantidad máxima son 8 latas");
        }
    }
}
