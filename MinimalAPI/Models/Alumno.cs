using System;
using System.Collections.Generic;

namespace MinimalAPI.Models
{
    public partial class Alumno
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public string FechaNacimiento { get; set; } = null!;
        public long? Pais { get; set; }
        public string? EstadoCivil { get; set; }

        public virtual Paise? PaisNavigation { get; set; }
    }
}
