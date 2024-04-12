using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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
        public async Task<IActionResult> Post([FromForm] MedicoViewModel medicoModel)
            {
            try
                {
                //objeto a ser cadastrado
                Usuario user = new Usuario
                    {
                    //recebe os valores e preenche as propriedades do objeto
                    Nome = medicoModel.Nome,
                    Email = medicoModel.Email,
                    TipoUsuarioId = medicoModel.IdTipoUsuario
                    };

                //define do nome do container do blob
                var containerName = "containervitalhubg7t";

                //define a string de conexão
                var connectionString = "DefaultEndpointsProtocol=https;AccountName=blobvitalhubg7t;AccountKey=ec0OT+Nif/vpyai1sCHs84Y2bsBUyFHtXRYOaDVegEIzXGLlqITvHFgT2OnYMfL+9i5f0pIl0oHi+AStTOW3AA==;EndpointSuffix=core.windows.net";

                //aqui vamos chamar o método para upload da imagem
                user.Foto = await AzureBlobStorageHelper.UploadImageBlobAsync(medicoModel.Arquivo!, connectionString, containerName);

                user.Senha = medicoModel.Senha;

                user.Medico = new Medico
                    {
                    Crm = medicoModel.Crm,
                    EspecialidadeId = medicoModel.EspecialidadeId,


                    Endereco = new Endereco
                        {
                        Logradouro = medicoModel.Logradouro,
                        Numero = medicoModel.Numero,
                        Cep = medicoModel.Cep
                        }
                    };

                _medicoRepository.Cadastrar(user);

                return Ok();
                }
            catch (Exception ex)
                {
                return BadRequest(ex.Message);
                }
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

        [Authorize]
        [HttpPut]
        public IActionResult UpdateProfile(MedicoViewModel medico)
            {
            try
                {
                Guid idUsuario = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_medicoRepository.AtualizarPerfil(idUsuario, medico));

                }
            catch (Exception ex)
                {
                return BadRequest(ex.Message);
                }
            }
        }
    }