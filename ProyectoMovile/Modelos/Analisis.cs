using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMovile.Modelos
{
    public class Analisis
    {
        public int ana_cod {  get; set; }
        
        public string ana_emocion { get; set; }
        public int cli_cod {  get; set; }
        public string cli_nombre {  get; set; }
        public string cli_apellido {  get; set; }
        public int usu_cod { get; set; }
        public string usu_correo { get; set; }

    }
}
