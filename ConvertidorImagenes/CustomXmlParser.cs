using System;
using System.Text;
using System.Xml.Linq;

namespace ConvertidorImagenes
{
    public static class CustomXmlParser
    {
        public static string ToHtml(string customXml)
        {
            if (string.IsNullOrWhiteSpace(customXml)) return "";

            string xmlSafe = "<root>" + customXml + "</root>";
            
            try
            {
                XElement root = XElement.Parse(xmlSafe);
                StringBuilder sb = new StringBuilder();
                ParseNode(root, sb);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"<div style='color:red; font-weight:bold;'>Error al parsear el diseño XML: {ex.Message}</div><pre>{System.Security.SecurityElement.Escape(customXml)}</pre>";
            }
        }

        private static void ParseNode(XElement element, StringBuilder sb)
        {
            foreach (var node in element.Nodes())
            {
                if (node is XText textNode)
                {
                    sb.Append(textNode.Value);
                }
                else if (node is XElement child)
                {
                    string tagName = child.Name.LocalName.ToLower();

                    switch (tagName)
                    {
                        case "documento":
                            sb.Append("<div style='background: white; padding: 40px; margin: 0 auto; max-width: 800px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); font-family: Arial, sans-serif;'>");
                            ParseNode(child, sb);
                            sb.Append("</div>");
                            break;
                            
                        case "titulo":
                            sb.Append("<h1 style='text-align: center; color: #2C3E50; font-size: 24pt; margin-bottom: 10px; border-bottom: 2px solid #34495E; padding-bottom: 10px;'>");
                            ParseNode(child, sb);
                            sb.Append("</h1>");
                            break;

                        case "subtitulo":
                            sb.Append("<h2 style='color: #7F8C8D; font-size: 16pt; margin-top: 5px; margin-bottom: 20px; text-align: center; font-weight: normal;'>");
                            ParseNode(child, sb);
                            sb.Append("</h2>");
                            break;

                        case "texto":
                            sb.Append("<p style='font-size: 11pt; line-height: 1.6; color: #333; margin-bottom: 15px; text-align: justify;'>");
                            ParseNode(child, sb);
                            sb.Append("</p>");
                            break;

                        case "tabla":
                            sb.Append("<table style='width: 100%; border-collapse: collapse; margin-bottom: 20px; font-size: 11pt;'>");
                            ParseNode(child, sb);
                            sb.Append("</table>");
                            break;

                        case "fila":
                            sb.Append("<tr>");
                            ParseNode(child, sb);
                            sb.Append("</tr>");
                            break;

                        case "celda":
                            sb.Append("<td style='border: 1px solid #BDC3C7; padding: 10px;'>");
                            ParseNode(child, sb);
                            sb.Append("</td>");
                            break;

                        case "imagen":
                            sb.Append("<div style='text-align: center; margin: 20px 0; padding: 20px; border: 2px dashed #BDC3C7; background: #F8F9FA; color: #7F8C8D; border-radius: 8px;'>");
                            sb.Append("<span style='font-size: 24px; display: block; margin-bottom: 10px;'>🖼️</span>");
                            sb.Append("<em>Imagen: ");
                            ParseNode(child, sb);
                            sb.Append("</em></div>");
                            break;

                        case "separador":
                            sb.Append("<hr style='border: 0; height: 1px; background-color: #E0E0E0; margin: 30px 0;' />");
                            break;

                        case "caja":
                            sb.Append("<div style='border: 1px solid #3498DB; background-color: #EBF5FB; padding: 20px; border-radius: 5px; margin-bottom: 20px;'>");
                            ParseNode(child, sb);
                            sb.Append("</div>");
                            break;

                        case "firma":
                            sb.Append("<div style='margin-top: 50px; text-align: center; width: 300px; display: inline-block;'>");
                            sb.Append("<div style='border-bottom: 1px solid #000; height: 40px; margin-bottom: 10px;'></div>");
                            sb.Append("<span style='font-size: 10pt; font-weight: bold;'>Firma: ");
                            ParseNode(child, sb);
                            sb.Append("</span>");
                            sb.Append("</div>");
                            break;

                        case "piepagina":
                            sb.Append("<div style='margin-top: 40px; padding-top: 10px; border-top: 1px solid #E0E0E0; text-align: center; font-size: 9pt; color: #95A5A6;'>");
                            ParseNode(child, sb);
                            sb.Append("</div>");
                            break;

                        case "saltopagina":
                            sb.Append("<div style='page-break-after: always; height: 2px; background: #E74C3C; margin: 20px 0;' title='Salto de Página'></div>");
                            break;

                        default:
                            ParseNode(child, sb);
                            break;
                    }
                }
            }
        }
    }
}
