using System;
using System.Collections.Generic;
using System.Text;

namespace FundamentosCSharp.Models
{
    public class Bar
    {  
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Cerveza> CervezaList { get; set; }

        public Bar(string Name)
        {
            this.Name = Name;   
        }
    }
}
