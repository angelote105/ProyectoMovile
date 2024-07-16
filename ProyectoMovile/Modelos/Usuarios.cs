using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMovile.Modelos
{
    public class Usuarios
    {
        public int usu_cod { get; set; }
        public string usu_nombre { get; set; }
        public string usu_apellido { get; set; }
        public string usu_correo { get; set; }
        public string usu_clave { get; set; }
        public string per_nombre { get; set; }
        public int usu_estado { get; set; }
    }
}
