using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Core.Exceptions;

namespace TrackOn.ClienteServico.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteServico _clienteServico;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(IClienteServico clienteServico, ILogger<ClienteController> logger)
    {
        _clienteServico = clienteServico;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterClientePorId(int id)
    {
        var cliente = await _clienteServico.ObterClientePorIdAsync(id);
        if (cliente is null)
        {
            return NotFound();
        }

        return Ok(new { cliente.Id, cliente.Nome, cliente.Email, cliente.Ativo });
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> ObterClientePorEmail(string email)
    {
        var cliente = await _clienteServico.ObterClientePorEmailAsync(email);
        if (cliente is null)
        {
            return NotFound();
        }

        return Ok(new { cliente.Id, cliente.Nome, cliente.Email, cliente.Ativo });
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosOsClientes()
    {
        var clientes = await _clienteServico.ObterTodosClientesAsync();
        if (!clientes.Any())
        {
            return NoContent();
        }

        var resposta = clientes.Select(cliente => new { cliente.Id, cliente.Nome, cliente.Email, cliente.Ativo });
        return Ok(resposta);
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarCliente([FromBody] CriarClienteDTO clienteDTO)
    {
        var cliente = await _clienteServico.AdicionarClienteAsync(clienteDTO);
        var resposta = new { cliente.Id, cliente.Nome, cliente.Email };
        return CreatedAtAction(nameof(ObterClientePorId), new { id = cliente.Id }, resposta);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarCliente(int id, [FromBody] AtualizarClienteDTO clienteDTO)
    {
        try
        {
            await _clienteServico.AtualizarClienteAsync(id, clienteDTO);
            return NoContent();
        }
        catch (ClienteNaoEncontradoException ex)
        {
            _logger.LogWarning(ex, "Tentativa de atualizar cliente inexistente: {ClienteId}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
            return Problem("Não foi possível atualizar o cliente no momento.");
        }
    }

    [HttpPut("{id:int}/senha")]
    public async Task<IActionResult> AlterarSenha(int id, [FromBody] AlterarSenhaDTO dto)
    {
        try
        {
            await _clienteServico.AlterarSenhaAsync(id, dto);
            return NoContent();
        }
        catch (ClienteNaoEncontradoException ex)
        {
            _logger.LogWarning(ex, "Tentativa de alterar senha de cliente inexistente: {ClienteId}", id);
            return NotFound();
        }
        catch (SenhaAtualIncorretaException ex)
        {
            _logger.LogWarning(ex, "Senha atual incorreta fornecida para o cliente {ClienteId}", id);
            return BadRequest(new { erro = "A senha atual informada é inválida." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar senha do cliente {ClienteId}", id);
            return Problem("Não foi possível alterar a senha no momento.");
        }
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Autenticar([FromBody] LoginClienteDTO clienteDto)
    {
        var autenticado = await _clienteServico.AutenticarClienteAsync(clienteDto.Email, clienteDto.Senha);
        if (!autenticado)
        {
            return Unauthorized();
        }

        return Ok();
    }
}