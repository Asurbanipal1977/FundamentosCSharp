using System;
using System.Collections.Generic;

namespace MinimalAPI.Models
{
    public partial class Paise
    {
        public Paise()
        {
            Alumnos = new HashSet<Alumno>();
        }

        public long Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}
