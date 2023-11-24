namespace Vagas.Api.Entities
{
    public class Vaga
    {
        public Vaga()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
