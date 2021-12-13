using System;
using System.Collections.Generic;

#nullable disable

namespace AspFirstMVC.Models
{
    public partial class Alumno
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string FechaNacimiento { get; set; }
        public long? Pais { get; set; }
        public string EstadoCivil { get; set; }

        public virtual Paise PaisNavigation { get; set; }
    }
}
