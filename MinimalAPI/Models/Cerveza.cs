using System;
using System.Collections.Generic;

namespace MinimalAPI.Models
{
    public partial class Cerveza
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public int Alcohol { get; set; }
        public int Cantidad { get; set; }
    }
}
