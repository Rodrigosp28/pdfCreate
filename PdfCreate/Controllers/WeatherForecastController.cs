using Microsoft.AspNetCore.Mvc;
using PdfCreate.Service;

namespace PdfCreate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("createpfd")]
        public async Task<IActionResult> createpdf([FromQuery] int cantidadProducto, [FromQuery] string marca)
        {
            var _reporteService = new ReporteService();
            string color = "#C61935";
            List<Producto> lista = new List<Producto>();

            for (int i = 1;i < cantidadProducto; i++)
            {
                lista.Add(new Producto() { Id = i.ToString(), Nombre = "Producto_"+i.ToString(), fecha_pago="2026-09-01",Precio = 336.59*(i*10) });
            }

            switch(marca.ToUpper())
            {
                case "NISSAN":
                    color = "#C61935";
                    break;
                case "RENAULT":
                    color = "#F08A0C ";
                    break;
                case "MITSUBISHI":
                    color = "#E60012";
                    break;
                case "INFINITI":
                    color = "#9c9c9c";
                    break;
                case "SEMINUEVOS":
                    color = "#C3002F";
                    break;
                default:
                    color = "#9b9b9b";
                    break;
            }

            var data = _reporteService.GenerarPdfDinamico(lista,color);
            return File(data, "application/pdf", "reporte.pdf");
        }
    }
}
