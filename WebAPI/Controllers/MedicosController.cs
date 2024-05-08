using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Contexts;
using WebAPI.Domains;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Utils.BlobStorage;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {

        VitalContext ctx = new VitalContext();


        private IMedicoRepository _medicoRepository;
        public MedicosController()
        {
            _medicoRepository = new MedicoRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_medicoRepository.ListarTodos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorId")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task <IActionResult> Post([FromForm] MedicoViewModel medicoModel)
        {
            //Objeto a ser cadastrado
            Usuario user = new Usuario();

            //Recebe os valores e preenche as propriedades do objeto
            user.Nome = medicoModel.Nome;
            user.Email = medicoModel.Email;
            user.TipoUsuarioId = medicoModel.IdTipoUsuario;

            //Define o nome do container do blob
            var containerName = "containervitalhubg7t";

            //String de conexão da Azure
            var connectingString = "DefaultEndpointsProtocol=https;AccountName=blobvitalhubg7t;AccountKey=ec0OT+Nif/vpyai1sCHs84Y2bsBUyFHtXRYOaDVegEIzXGLlqITvHFgT2OnYMfL+9i5f0pIl0oHi+AStTOW3AA==;EndpointSuffix=core.windows.net";

            //Aqui vamos chamar o método para upload de imagem
            user.Foto = await AzureBlobStorageHelper.UploadImageBlobAsync(medicoModel.Arquivo!, connectingString, containerName);

            //Aqui vamos chamar o método para upload da imagem
            user.Foto = medicoModel.Foto;

            user.Senha = medicoModel.Senha;

            user.Medico = new Medico();
            user.Medico.Crm = medicoModel.Crm;
            user.Medico.EspecialidadeId = medicoModel.EspecialidadeId;


            user.Medico.Endereco = new Endereco();
            user.Medico.Endereco.Logradouro = medicoModel.Logradouro;
            user.Medico.Endereco.Numero = medicoModel.Numero;
            user.Medico.Endereco.Cep = medicoModel.Cep;

            _medicoRepository.Cadastrar(user);

            return Ok();
        }

        [HttpGet("BuscarPorIdClinica")]
        public IActionResult GetByIdClinica(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.ListarPorClinica(id)); ;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorData")]
        public IActionResult GetByDate(DateTime data, Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorData(data, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut]
        public IActionResult UpdateProfile(Guid idUsuario, MedicoViewModel medico)
        {
            try
            {
                return Ok(_medicoRepository.AtualizarPerfil(idUsuario, medico));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}