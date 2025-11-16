using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClubApp.Models
{
    public class SocioInfo
    {
        public int NroSocio { get; set; }
        public string NombreCompleto { get; set; }
        public string DNI { get; set; }
        public string EstadoPago { get; set; }
        public byte[] FotoCarnet { get; set; }
        public string CarnetCodigo { get; set; }
    }
}
