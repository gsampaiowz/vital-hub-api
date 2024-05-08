using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Domains;
using WebAPI.Utils.Mail;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecuperarSenhaController : ControllerBase
    {

        private readonly VitalContext _context;
        private readonly EmailSendingService _emailSendingService;

        public RecuperarSenhaController(VitalContext context, EmailSendingService emailSendingService)
        {
            _context = context;
            _emailSendingService = emailSendingService;
        }

        [HttpPost]

        public async Task<IActionResult> SendRecoveryCodePassword(string email)
        {
            try
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email)!;

                if (user == null)
                {
                    return NotFound("Usuario não encontrado");
                }

                //Gerar um código com 4 algarismo
                Random random = new Random();
                int recoveryCode = random.Next(1000, 9999);

                user.CodRecupSenha = recoveryCode;

                await _context.SaveChangesAsync();

                await _emailSendingService.SendRecovery(user.Email!, recoveryCode);

                return Ok("Código enviado com sucesso!");

            }
            catch (Exception error)
            {

                return BadRequest(error.Message);
            }
        }

        [HttpPost("RotaDeRecuperacaoDeSenha")]

        public async Task<IActionResult> ValidarSenha(string email, int recoveryCode)
        {

            try
            {
                //Entra no Campo usuário e pera para retornar o email
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email)!;

                //Se o código aleatório enviado para o email, for igual ao CodRecupSenha que está dentro do usuário 
                if (user == null)
                {
                    //Retornar Código válido
                    return NotFound("Código válido");

                }
                if (user.CodRecupSenha != recoveryCode)
                { 
                    return BadRequest("Código de recuperação inválido");
                }
                 
                user.CodRecupSenha = null; 

                await _context.SaveChangesAsync();

                return Ok("Código de recuperação válido");


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
