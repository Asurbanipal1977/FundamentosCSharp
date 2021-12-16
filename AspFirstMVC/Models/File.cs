using System;
using System.Collections.Generic;

#nullable disable

namespace AspFirstMVC.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public byte[] FileDb { get; set; }
        public string Path { get; set; }
    }
}
