using WebAPI.Contexts;
using WebAPI.Domains;
using WebAPI.Interfaces;

namespace WebAPI.Repositories
    {
    public class ExameRepository : IExameRepository
        {
        public VitalContext ctx = new VitalContext();

        public List<Exame> BuscarPorIdConsulta(Guid idConsulta)
            {
            try
                {
                return ctx.Exames
                    .Where(x => x.ConsultaId == idConsulta)
                    .ToList();
                }
            catch (Exception)
                {
                throw;
                }
            }

        public void Cadastrar(Exame exame)
            {
            try
                {
                ctx.Exames.Add(exame);
                ctx.SaveChanges();

                }
            catch (Exception)
                {
                throw;
                }
            }

        public Exame BuscarPorId(Guid id)
            {
            try
                {
                return ctx.Exames.FirstOrDefault(ctx => ctx.Id == id)!;
                }
            catch (Exception)
                {

                throw;
                }
            }

        public void Delete(Guid id)
            {
            try
                {
                ctx.Exames.Remove(BuscarPorId(id));
                ctx.SaveChanges();
                }
            catch (Exception)
                {

                throw;
                }
            }
        }
    }