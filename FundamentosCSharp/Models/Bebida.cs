using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FundamentosCSharp.Models
{
    public class Bebida
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }

        public Bebida(string Nombre, int Cantidad)
        {
            this.Cantidad = Cantidad;
            this.Nombre = Nombre;
        }

        public void Beber (int Cantidad)
        {
            this.Cantidad -= Cantidad;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
