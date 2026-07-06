using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertidorImagenes
{
    public static class MarkdownParser
    {
        public static string ToHtml(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown)) return "";

            string html = markdown;

            // Escapar HTML basico
            html = html.Replace("<", "&lt;").Replace(">", "&gt;");

            // Bold
            html = Regex.Replace(html, @"\*\*(.*?)\*\*", "<b>$1</b>");

            // Tablas (Procesamiento basico)
            html = ParseTables(html);

            return html;
        }

        private static string ParseTables(string text)
        {
            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();
            bool inTable = false;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                
                if (line.StartsWith("|") && line.EndsWith("|"))
                {
                    if (!inTable)
                    {
                        sb.AppendLine("<table style='width:100%; border-collapse: collapse; margin-top: 10px; margin-bottom: 10px; font-family: Segoe UI, sans-serif;'>");
                        inTable = true;
                    }

                    // Es una linea separadora? ej: |---|---|
                    if (Regex.IsMatch(line, @"^\|[\-\s\|]+\|$"))
                    {
                        continue;
                    }

                    string[] cells = line.Trim('|').Split('|');
                    sb.AppendLine("<tr>");
                    foreach (var cell in cells)
                    {
                        // Si es la primera fila de la tabla, hacerla header
                        if (inTable && i > 0 && !lines[i-1].StartsWith("|"))
                            sb.AppendLine($"<th style='border: 1px solid #dee2e6; padding: 8px; background-color: #f8f9fa; text-align: left;'>{cell.Trim()}</th>");
                        else if (inTable && i == 0) // Primera linea del texto es tabla
                            sb.AppendLine($"<th style='border: 1px solid #dee2e6; padding: 8px; background-color: #f8f9fa; text-align: left;'>{cell.Trim()}</th>");
                        else
                            sb.AppendLine($"<td style='border: 1px solid #dee2e6; padding: 8px;'>{cell.Trim()}</td>");
                    }
                    sb.AppendLine("</tr>");
                }
                else
                {
                    if (inTable)
                    {
                        sb.AppendLine("</table>");
                        inTable = false;
                    }
                    if (line == "") sb.AppendLine("<br>");
                    else sb.AppendLine(line + "<br>");
                }
            }

            if (inTable) sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
