using Microsoft.AspNetCore.Mvc;
using RinhaBackendAPI.Business;
using RinhaBackendAPI.Models;
using RinhaBackendAPI.Repository;

namespace RinhaBackendAPI.Controllers;

[ApiController]
[Route("clientes")]
public class ClienteController : ControllerBase
{
    private readonly ILogger<ClienteController> _logger;
    private readonly ClienteRepository _clienteRepository;
    private readonly TransacaoBusiness _transacaoBusiness;

    public ClienteController(
        ILogger<ClienteController> logger,
        ClienteRepository clienteRepository,
        TransacaoBusiness transacaoBusiness)
    {
        _logger = logger;
        _clienteRepository = clienteRepository;
        _transacaoBusiness = transacaoBusiness;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            return Ok(_clienteRepository.Obter(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Um problema ocorreu ao processar a sua solicitação.");
        }
    }

    [HttpPost("{id}/transacoes")]
    public IActionResult PostTransacao(int id, [FromBody] Transacao transacao)
    {
        try
        {
            transacao.ClienteId = id;
            _transacaoBusiness.Save(transacao);
            var cliente = _clienteRepository.Obter(id);
            return Ok(new { cliente.Limite, cliente.Saldo });
        }
        catch (CustomerNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InsufficientFundsException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (TransactionValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Um problema ocorreu ao processar a sua solicitação.");
        }
    }

    [HttpGet("{id}/extrato")]
    public IActionResult GetExtrato(int id)
    {
        try
        {
            var cliente = _clienteRepository.ObterComTransacoes(id);
            if (cliente == null)
                return NotFound($"Cliente { id } não encontado.");

            var extrato = new {
                Saldo = new {
                    Total = cliente.Saldo,
                    DataExtrato = DateTime.Now,
                    Limite = cliente.Limite
                },
                UltimasTransacoes = cliente.Transacoes
            };

            return Ok(extrato);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, "Um problema ocorreu ao processar a sua solicitação.");
        }

    }
}
