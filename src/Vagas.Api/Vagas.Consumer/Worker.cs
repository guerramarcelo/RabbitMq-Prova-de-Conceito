using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Vagas.Api.Entities;
using Vagas.Consumer.Services;

namespace Vagas.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILinkedinService linkedinService;

        public Worker(ILogger<Worker> logger,
            ILinkedinService linkedinService)
        {
            _logger = logger;
            this.linkedinService = linkedinService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "integracao-linkedin",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var vaga = JsonSerializer.Deserialize<Vaga>(message);


                    var vagaLkd = await linkedinService.CadastrarVaga(
                        new LinkedinVagas.Api.Entities.VagaLinkedin()
                        {
                            IdIntegracao = vaga.Id,
                            Descricao = vaga.Descricao,
                            Nome = vaga.Nome
                        });

                    _logger.LogInformation("VAGA CADASTRADA NO LINKEDINHO");
                };

                channel.BasicConsume(queue: "integracao-linkedin",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}