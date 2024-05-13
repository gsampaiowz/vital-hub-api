using WebAPI.Domains;

namespace WebAPI.Interfaces
{
    public interface IExameRepository
    {
        public void Cadastrar(Exame exame);

        public List<Exame> BuscarPorIdConsulta(Guid idConsulta);

        public void Delete(Guid id);
    }
}