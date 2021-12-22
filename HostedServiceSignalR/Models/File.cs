using System;
using System.Collections.Generic;

#nullable disable

namespace HostedServiceSignalR.Models
{
    public partial class File
    {
        public int Id { get; set; }
        public byte[] FileDb { get; set; }
        public string Path { get; set; }
    }
}
