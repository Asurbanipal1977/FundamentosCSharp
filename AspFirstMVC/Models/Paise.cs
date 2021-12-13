using System;
using System.Collections.Generic;

#nullable disable

namespace AspFirstMVC.Models
{
    public partial class Paise
    {
        public Paise()
        {
            Alumnos = new HashSet<Alumno>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
