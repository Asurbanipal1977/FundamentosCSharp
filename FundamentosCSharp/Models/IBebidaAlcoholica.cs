using System;
using System.Collections.Generic;
using System.Text;

namespace FundamentosCSharp.Models
{
    public interface IBebidaAlcoholica
    {
        public int Alcohol { get; set; }
        public void CantidadMax() { }
    }
}
