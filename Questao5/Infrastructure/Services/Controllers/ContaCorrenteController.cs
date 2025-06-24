using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Request;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Validators;
using Questao5.Application.Commands.Validator;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{numero}/saldo")]
        public async Task<IActionResult> ObterSaldo(int numero)
        {
            var validator = new GetSaldoRequestValidator();
            var validationResult = validator.Validate(new GetSaldoRequest { NumeroConta = numero });

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _mediator.Send(new GetSaldoRequest { NumeroConta = numero });
            return Ok(result);
        }


        [HttpPost("movimento")]
        public async Task<IActionResult> RegistrarMovimento([FromBody] RegistrarMovimentoRequest request)
        {
            var validator = new RegistrarMovimentoRequestValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}
