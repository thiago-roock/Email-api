using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Email.Domain.ExternalServices;
using Email.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Email.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

       
        [HttpPost("Enviar")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult EnviaEmail([FromBody]EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //email destino, assunto do email, mensagem a enviar
                   
                    _emailSender.SendEmailAsync(email.Destino, email.Assunto, email.Mensagem).GetAwaiter();
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }
       
    }
}