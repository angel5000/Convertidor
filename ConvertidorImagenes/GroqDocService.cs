using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConvertidorImagenes
{
    public class GroqDocService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string apiKey;
        private readonly string modelo = "llama-3.3-70b-versatile";
        private readonly string endpoint = "https://api.groq.com/openai/v1/chat/completions";

        private readonly string systemPrompt = @"Eres un diseñador de documentos impresos.

No diseñes páginas web.
No utilices conceptos responsive ni elementos web.
Debes pensar como Microsoft Word.

Tu única función es estructurar plantillas de documentos utilizando un formato XML estricto propio y definir la configuración física de la hoja en metadatos JSON.

Las variables editables deben ir entre corchetes, ejemplo: [NOMBRE], [FECHA_ACTUAL].
Los nombres de variables deben ser en MAYÚSCULAS y separados por guion bajo.
NUNCA coloques variables dentro de los atributos del XML (ej. no hagas <Imagen src='[IMG]'/>). Las variables siempre van como contenido de texto de la etiqueta (ej. <Imagen>[IMG]</Imagen>).

REGLAS IMPORTANTES PARA EL CONTENIDO:
1. Genera una plantilla utilizando ÚNICAMENTE los siguientes componentes (etiquetas XML) en el ContenidoBase:
<Documento>, <Titulo>, <Subtitulo>, <Texto>, <Tabla>, <Fila>, <Celda>, <Imagen>, <Separador>, <Caja>, <Firma>, <SaltoPagina>.
2. IMPORTANTE: NO uses etiquetas <PiePagina>, <Encabezado>, ni generes números de página o fechas en el XML. Todo lo relacionado a encabezados (Header) y pie de página (Footer) debe configurarse exclusivamente en los objetos JSON 'Header' y 'Footer'.
3. El nodo raíz del ContenidoBase siempre debe ser <Documento>.
4. Conserva las variables requeridas por el usuario en el contenido principal.

Debes responder ÚNICAMENTE en JSON válido.
El XML generado va DENTRO del campo ContenidoBase como un string de una sola línea, correctamente escapado. No uses comillas dobles dentro del XML, usa comillas simples (' ') si necesitas poner atributos.
No agregues texto antes ni después del JSON. No envuelvas en bloques de código markdown.

El formato de respuesta es exactamente:
{
  ""Titulo"": ""nombre de la plantilla"",
  ""TipoDocumento"": ""Factura"",
  ""EstiloVisual"": ""Corporativo"",
  ""Configuracion"": {
    ""Orientacion"": ""Vertical"",
    ""TamanoPapel"": ""A4"",
    ""MargenSuperior"": 2.0,
    ""MargenInferior"": 2.0,
    ""MargenIzquierdo"": 2.0,
    ""MargenDerecho"": 2.0
  },
  ""Header"": {
    ""Visible"": true,
    ""MostrarLogo"": true,
    ""Alineacion"": ""Derecha"",
    ""TituloSecundario"": ""Nombre de Empresa""
  },
  ""Footer"": {
    ""Visible"": true,
    ""FormatoNumeroPagina"": ""Pagina X de Y"",
    ""Texto"": ""Documento confidencial"",
    ""MostrarFecha"": true
  },
  ""ContenidoBase"": ""<Documento><Titulo>TITULO DEL DOC</Titulo><Texto>Contenido...</Texto></Documento>"",
  ""Campos"": [
    {
      ""Id"": ""[NOMBRE_VARIABLE]"",
      ""Etiqueta"": ""Nombre descriptivo del campo"",
      ""Tipo"": ""Texto""
    }
  ]
}

Si el usuario solicita algo distinto de generar una plantilla, responde exactamente:
{""error"":""Solicitud fuera del alcance.""}

No escribas ninguna otra palabra fuera del JSON.";

        public GroqDocService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<PlantillaModel> GenerarPlantillaAsync(string descripcionUsuario)
        {
            string userPrompt = "Genera una plantilla documental estructurada. Define su configuración física, encabezados y pie de página en los metadatos JSON, y usa nuestro XML documental para el cuerpo.\n\n"
                + "Debe contener variables entre [].\n"
                + "Solicitud del usuario:\n" + descripcionUsuario;

            var requestBody = new
            {
                model = modelo,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.6,
                max_tokens = 3000
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            string responseText = Encoding.UTF8.GetString(bytes);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error de Groq API (" + (int)response.StatusCode + "): " + responseText);
            }

            JObject jsonResponse = JObject.Parse(responseText);
            string contenidoIA = jsonResponse["choices"][0]["message"]["content"].ToString().Trim();

            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(contenidoIA, @"\{.*\}", System.Text.RegularExpressions.RegexOptions.Singleline);
            if (match.Success)
            {
                contenidoIA = match.Value;
            }

            JObject parsed = JObject.Parse(contenidoIA);
            if (parsed["error"] != null)
            {
                throw new Exception(parsed["error"].ToString());
            }

            PlantillaModel plantilla = JsonConvert.DeserializeObject<PlantillaModel>(contenidoIA);
            return plantilla;
        }
    }
}

