using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertidorImagenes
{
    public class CampoPlantilla
    {
        public string Id { get; set; } // e.g. ""[NOMBRE]""
        public string Etiqueta { get; set; } // e.g. ""Nombre Completo""
        public string Tipo { get; set; } // e.g. ""Texto"", ""Fecha""
        public string ValorDefecto { get; set; } // Datos prellenados por la IA
    }

    public class ConfigHeader
    {
        public bool Visible { get; set; }
        public bool MostrarLogo { get; set; }
        public string Alineacion { get; set; } // Izquierda, Centro, Derecha
        public string TituloSecundario { get; set; }
    }

    public class ConfigFooter
    {
        public bool Visible { get; set; }
        public string FormatoNumeroPagina { get; set; } // ""Pagina X de Y"", ""X"", etc.
        public string Texto { get; set; }
        public bool MostrarFecha { get; set; }
    }

    public class ConfiguracionPapel
    {
        public string Orientacion { get; set; } // Vertical, Horizontal
        public string TamanoPapel { get; set; } // A4, Carta, Oficio, Legal
        public double MargenSuperior { get; set; }
        public double MargenInferior { get; set; }
        public double MargenIzquierdo { get; set; }
        public double MargenDerecho { get; set; }
    }

    public class PlantillaModel
    {
        public string Titulo { get; set; }
        public string TipoDocumento { get; set; } // Factura, Contrato, Reporte, etc.
        public string EstiloVisual { get; set; } // Corporativo, Minimalista, etc.
        
        public ConfiguracionPapel Configuracion { get; set; }
        public ConfigHeader Header { get; set; }
        public ConfigFooter Footer { get; set; }
        
        public string ContenidoBase { get; set; }
        public List<CampoPlantilla> Campos { get; set; }

        public PlantillaModel()
        {
            Campos = new List<CampoPlantilla>();
            Configuracion = new ConfiguracionPapel { Orientacion = "Vertical", TamanoPapel = "A4", MargenSuperior = 2.0, MargenInferior = 2.0, MargenDerecho = 2.0, MargenIzquierdo = 2.0 };
            Header = new ConfigHeader { Visible = false };
            Footer = new ConfigFooter { Visible = false };
        }
    }
}
