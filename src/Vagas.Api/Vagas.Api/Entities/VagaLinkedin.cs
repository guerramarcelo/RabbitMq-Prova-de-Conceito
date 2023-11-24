namespace Vagas.Api.Entities
{
    public class VagaLinkedin
    {
        public VagaLinkedin()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid IdIntegracao { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
