using Refit;
using Vagas.Consumer;
using Vagas.Consumer.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddRefitClient<ILinkedinService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:50001"));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
