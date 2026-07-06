using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConvertidorImagenes
{
    public class GroqService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string apiKey;
        private readonly string modelo = "llama-3.3-70b-versatile";
        private readonly string endpoint = "https://api.groq.com/openai/v1/chat/completions";

        private readonly string systemPrompt = @"Eres un asistente de Inteligencia Artificial experto en redactar y mejorar plantillas documentales.

Nunca converses ni respondas preguntas generales.
Tu unica funcion es generar o mejorar plantillas documentales.

Las variables deben ir entre corchetes, ejemplo: [NOMBRE], [FECHA].
Los nombres de variables deben ser en MAYUSCULAS y separados por guion bajo.

REGLAS IMPORTANTES PARA EL CONTENIDO:
1. Tu labor es CREAR y MEJORAR textos. Si el usuario te pide un cambio de tono, estilo o te pide que 'suene como' algo especifico (ej. un banco, un abogado), DEBES reescribir y enriquecer el texto para cumplir su peticion. NO te limites a copiar y pegar el borrador original.
2. Si el usuario te proporciona un texto que ya incluye variables entre [corchetes], DEBES conservar esos nombres de variables en tu version mejorada para no romper su logica.
3. Si necesitas estructurar datos (como listas de productos en una cotizacion, precios, o items repetitivos), UTILIZA tablas en formato Markdown (con el simbolo |). El sistema esta disenado para renderizar estas tablas de forma hermosa. Por ejemplo:
| Cantidad | Producto | Precio Unitario | Subtotal |
|---|---|---|---|
| [CANTIDAD] | [PRODUCTO] | $[PRECIO] | $[SUBTOTAL] |

Debes responder UNICAMENTE en JSON valido.
No escribas Markdown fuera del JSON.
No uses bloques de codigo.
No agregues texto antes ni despues del JSON.

El formato de respuesta es exactamente:
{
  ""Titulo"": ""nombre de la plantilla"",
  ""ContenidoBase"": ""texto completo de la plantilla con variables entre corchetes. Puedes incluir tablas markdown aqui si es necesario para listas repetitivas."",
  ""Campos"": [
    {
      ""Id"": ""[NOMBRE_VARIABLE]"",
      ""Etiqueta"": ""Nombre descriptivo del campo"",
      ""Tipo"": ""Texto""
    }
  ]
}

Los tipos validos para Tipo son: Texto, Fecha, Monto.

Si el usuario solicita algo distinto de generar una plantilla documental, responde exactamente:
{""error"":""Solicitud fuera del alcance.""}

No escribas ninguna otra palabra fuera del JSON.";

        public GroqService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<PlantillaModel> GenerarPlantillaAsync(string descripcionUsuario)
        {
            string userPrompt = "Genera una plantilla documental.\n\n"
                + "Debe contener variables entre [].\n"
                + "Devuelve el JSON con Titulo, ContenidoBase y Campos.\n\n"
                + "Solicitud del usuario:\n" + descripcionUsuario;

            var requestBody = new
            {
                model = modelo,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.7,
                max_tokens = 2048
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

            // Extraer el contenido del mensaje
            JObject jsonResponse = JObject.Parse(responseText);
            string contenidoIA = jsonResponse["choices"][0]["message"]["content"].ToString().Trim();

            // Limpiar posibles bloques de codigo markdown
            if (contenidoIA.StartsWith("`"))
            {
                int firstNewline = contenidoIA.IndexOf('\n');
                if (firstNewline > 0) contenidoIA = contenidoIA.Substring(firstNewline + 1);
                if (contenidoIA.EndsWith("`"))
                    contenidoIA = contenidoIA.Substring(0, contenidoIA.LastIndexOf("`"));
                contenidoIA = contenidoIA.Trim();
            }

            // Verificar si hay error
            JObject parsed = JObject.Parse(contenidoIA);
            if (parsed["error"] != null)
            {
                throw new Exception(parsed["error"].ToString());
            }

            PlantillaModel plantilla = JsonConvert.DeserializeObject<PlantillaModel>(contenidoIA);
            return plantilla;
        }
        public async Task<string> MejorarRedaccionAsync(string textoActual, string solicitudUsuario)
        {
            string systemPrompt = @"Eres un redactor documental senior.
No eres un chatbot.
No eres un corrector ortogrÃ¡fico.
No eres un asistente conversacional.
Tu funciÃ³n consiste Ãºnicamente en reescribir documentos.
Siempre debes mejorar significativamente el contenido.
Puedes:
- ampliar el texto
- reorganizar pÃ¡rrafos
- cambiar completamente el estilo
- cambiar el tono
- hacer el documento mucho mÃ¡s profesional
Nunca debes responder preguntas.
Nunca debes explicar.
Nunca debes conversar.
Conserva exactamente las variables entre [].
Devuelve Ãºnicamente el documento final.

Ejemplo
Entrada:
Se recibiÃ³ el pago de [MONTO].
Solicitud:
Haz que parezca emitido por un banco.
Salida esperada:
BANCO [EMPRESA]

CONFIRMACIÃ“N DE PAGO

Estimado(a) [CLIENTE]:

Nos complace informarle que el pago por un importe de [MONTO] ha sido recibido satisfactoriamente y registrado en nuestros sistemas.

Agradecemos la confianza depositada en [EMPRESA].

Atentamente

Departamento de AtenciÃ³n al Cliente";

            string prompt = $@"
DOCUMENTO ORIGINAL
------------------------------------------------
{textoActual}
------------------------------------------------

OBJETIVO
{solicitudUsuario}

Puedes modificar completamente la redacciÃ³n.
Debes utilizar el lenguaje adecuado para el objetivo.
Puedes agregar:
- encabezado
- saludo
- pie de pÃ¡gina
- aviso institucional

Conserva exactamente las variables entre [].
Devuelve Ãºnicamente el texto final reescrito, sin ninguna otra palabra o bloque de cÃ³digo.
";

            var requestBody = new
            {
                model = modelo,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = prompt }
                },
                temperature = 0.85,
                presence_penalty = 0.8,
                frequency_penalty = 0.4,
                max_tokens = 2048
            };

            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.SendAsync(request);
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            string responseText = Encoding.UTF8.GetString(bytes);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error de Groq API: " + responseText);

            JObject jsonResponse = JObject.Parse(responseText);
            string resultado = jsonResponse["choices"][0]["message"]["content"].ToString().Trim();

            // Limpiar markdown si lo hay
            if (resultado.StartsWith("`"))
            {
                int firstNewline = resultado.IndexOf('\n');
                if (firstNewline > 0) resultado = resultado.Substring(firstNewline + 1);
                if (resultado.EndsWith("`"))
                    resultado = resultado.Substring(0, resultado.LastIndexOf("`"));
                resultado = resultado.Trim();
            }

            return resultado;
        }
    }
}

