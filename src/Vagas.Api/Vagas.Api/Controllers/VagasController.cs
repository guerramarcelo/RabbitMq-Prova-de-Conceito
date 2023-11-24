using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vagas.Api.Entities;
using Vagas.Consumer.Services;

namespace Vagas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VagasController : ControllerBase
    {

        private readonly ILogger<VagasController> _logger;
        private readonly ILinkedinService linkedinService;

        public VagasController(ILogger<VagasController> logger, ILinkedinService linkedinService)
        {
            _logger = logger;
            this.linkedinService = linkedinService;
        }

        [HttpPost]
        public IActionResult Post(Vaga vaga)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "integracao-linkedin",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                        
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(vaga));

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "integracao-linkedin",
                                 basicProperties: null,
                                 body: body);

            return Ok(vaga);
        }

        [HttpPost("sincrono")]
        public async Task<IActionResult> PostSincrono(Vaga vaga)
        {
            var vagaLkd = await linkedinService.CadastrarVaga(new VagaLinkedin()
            {
                IdIntegracao = vaga.Id,
                Descricao = vaga.Descricao,
                Nome = vaga.Nome
            });

            _logger.LogInformation("INTEGRADO COM LINKEDINHO");

            return Ok(vagaLkd);
        }
    }
}