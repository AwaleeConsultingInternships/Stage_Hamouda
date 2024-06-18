using MathematicalTools;
using Newtonsoft.Json;
using System.Text;

public class ChartHtmlGenerator
{
    public string GenerateHtml(List<Point> xyValues)
    {
        // Convert xyValues to JSON format
        string jsonData = JsonConvert.SerializeObject(xyValues);

        // Use StringBuilder for efficient string concatenation
        StringBuilder htmlBuilder = new StringBuilder();
        htmlBuilder.AppendLine("<!DOCTYPE html>");
        htmlBuilder.AppendLine("<html lang=\"en\">");
        htmlBuilder.AppendLine("<head>");
        htmlBuilder.AppendLine("  <meta charset=\"UTF-8\">");
        htmlBuilder.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        htmlBuilder.AppendLine("  <title>Dynamic Legend Action</title>");
        htmlBuilder.AppendLine("  <script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>");
        htmlBuilder.AppendLine("  <style>");
        htmlBuilder.AppendLine("    .resizable {");
        htmlBuilder.AppendLine("      resize: both;");
        htmlBuilder.AppendLine("      overflow: auto;");
        htmlBuilder.AppendLine("      border: 1px solid #ccc;");
        htmlBuilder.AppendLine("      padding: 10px;");
        htmlBuilder.AppendLine("    }");
        htmlBuilder.AppendLine("    .resizable canvas {");
        htmlBuilder.AppendLine("      width: 100% !important;");
        htmlBuilder.AppendLine("      height: 100% !important;");
        htmlBuilder.AppendLine("    }");
        htmlBuilder.AppendLine("  </style>");
        htmlBuilder.AppendLine("</head>");
        htmlBuilder.AppendLine("<body>");
        htmlBuilder.AppendLine("  <div class=\"resizable\" style=\"width: 80%; height: 400px; margin: auto;\">");
        htmlBuilder.AppendLine("    <canvas id=\"myChart\"></canvas>");
        htmlBuilder.AppendLine("  </div>");
        htmlBuilder.AppendLine("  <script>");
        htmlBuilder.AppendLine("    const xyValues = " + jsonData + ";");
        htmlBuilder.AppendLine("    document.addEventListener('DOMContentLoaded', function () {");
        htmlBuilder.AppendLine("      const labels = xyValues.map(point => point.X);");
        htmlBuilder.AppendLine("      const data = xyValues.map(point => point.Y);");
        htmlBuilder.AppendLine("      const ctx = document.getElementById('myChart').getContext('2d');");
        htmlBuilder.AppendLine("      const myChart = new Chart(ctx, {");
        htmlBuilder.AppendLine("        type: 'line',");
        htmlBuilder.AppendLine("        data: {");
        htmlBuilder.AppendLine("          labels: labels,");
        htmlBuilder.AppendLine("          datasets: [{");
        htmlBuilder.AppendLine("            label: 'Discount Curve',");
        htmlBuilder.AppendLine("            data: data,");
        htmlBuilder.AppendLine("            borderColor: 'rgba(75, 192, 192, 1)',");
        htmlBuilder.AppendLine("            borderWidth: 1,");
        htmlBuilder.AppendLine("            fill: false");
        htmlBuilder.AppendLine("          }]");
        htmlBuilder.AppendLine("        },");
        htmlBuilder.AppendLine("        options: {");
        htmlBuilder.AppendLine("          responsive: true,");
        htmlBuilder.AppendLine("          maintainAspectRatio: false,");
        htmlBuilder.AppendLine("          scales: {");
        htmlBuilder.AppendLine("            x: {");
        htmlBuilder.AppendLine("              beginAtZero: true");
        htmlBuilder.AppendLine("            }");
        htmlBuilder.AppendLine("          },");
        htmlBuilder.AppendLine("          plugins: {");
        htmlBuilder.AppendLine("            legend: {");
        htmlBuilder.AppendLine("              display: true");
        htmlBuilder.AppendLine("            },");
        htmlBuilder.AppendLine("            tooltip: {");
        htmlBuilder.AppendLine("              enabled: true");
        htmlBuilder.AppendLine("            }");
        htmlBuilder.AppendLine("          }");
        htmlBuilder.AppendLine("        }");
        htmlBuilder.AppendLine("      });");
        htmlBuilder.AppendLine("    });");
        htmlBuilder.AppendLine("  </script>");
        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        return htmlBuilder.ToString();
    }

    public void WriteHtmlToFile(List<Point> xyValues, string filePath)
    {
        try
        {
            string htmlContent = GenerateHtml(xyValues);

            File.WriteAllText(filePath, htmlContent);
            Console.WriteLine($"HTML file created at: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}