﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Utils.Mail;

namespace WebAPI.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RecuperarSenhaController : ControllerBase
        {
        private readonly VitalContext _context;
        private readonly EmailSendingService _sendMail;

        public RecuperarSenhaController(VitalContext context, EmailSendingService sendMail)
            {
            _context = context;
            _sendMail = sendMail;
            }


        [HttpPost]
        public async Task<IActionResult> SendRecoveryCodePassword(string email)
            {
            try
                {
                var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                    {
                    return NotFound("Usuário não encontrado!");
                    }

                //gerar um código com 4 algarismos
                Random random = new();
                int recoveryCode = random.Next(1000, 9999);

                user.CodRecupSenha = recoveryCode;

                await _context.SaveChangesAsync();

                await _sendMail.SendRecovery(user.Email!, recoveryCode);

                return Ok("Código enviado com sucesso!");
                }
            catch (Exception ex)
                {
                return BadRequest(ex.Message);
                }
            }

        [HttpPost("Validar")]
        public async Task<IActionResult> ValidadeRecoveryCode(string email, int codigo)
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
                if (user.CodRecupSenha != codigo)
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

            //try
            //    {
            //    var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            //    if (user == null)
            //        {
            //        return NotFound("Usuário não encontrado!");
            //        }

            //    if (codigo != user!.CodRecupSenha)
            //        {
            //        return BadRequest("Código errado!");
            //        }

            //    user.CodRecupSenha = null;

            //    await _context.SaveChangesAsync();

            //    return Ok("Código correto!");

            //    }
            //catch (Exception ex)
            //    {
            //    return BadRequest(ex.Message);
            //    }
            }
        }
    }
