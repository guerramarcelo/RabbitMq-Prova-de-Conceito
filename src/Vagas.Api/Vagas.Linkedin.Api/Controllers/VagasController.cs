using LinkedinVagas.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace VagasLinkedin.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VagasController : ControllerBase
    {

        private readonly ILogger<VagasController> _logger;

        public VagasController(ILogger<VagasController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(VagaLinkedin vaga)
        {
            _logger.LogInformation($"Chegou vaga {vaga.Nome}");
            await Task.Delay(TimeSpan.FromSeconds(1));
            return Ok(vaga);
        }
    }
}