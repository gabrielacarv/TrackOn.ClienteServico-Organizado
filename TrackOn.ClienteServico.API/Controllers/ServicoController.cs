using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Core.Entidades.DTOs;

namespace TrackOn.ClienteServico.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicoController : ControllerBase
{
    private readonly IServicoServico _servicoServico;
    private readonly ILogger<ServicoController> _logger;

    public ServicoController(IServicoServico servicoServico, ILogger<ServicoController> logger)
    {
        _servicoServico = servicoServico;
        _logger = logger;
    }

    [HttpGet("cliente/{clienteId:int}")]
    public async Task<IActionResult> ObterServicosPorCliente(int clienteId)
    {
        var servicos = await _servicoServico.ObterServicosPorClienteAsync(clienteId);
        if (!servicos.Any())
        {
            return NoContent();
        }

        return Ok(servicos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var servico = await _servicoServico.ObterServicoPorIdAsync(id);
        if (servico is null)
        {
            return NotFound();
        }

        return Ok(servico);
    }

    [HttpPost]
    public async Task<IActionResult> CriarServico([FromBody] CriarServicoDTO dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.EnderecoUrl))
        {
            return BadRequest("Os dados do serviço são inválidos.");
        }

        var servicoCriado = await _servicoServico.CriarServicoAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = servicoCriado.Id }, servicoCriado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> AtualizarServico(int id, [FromBody] AtualizarServicoDTO dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.EnderecoUrl))
        {
            return BadRequest("Os dados do serviço são inválidos.");
        }

        var servicoAtualizado = await _servicoServico.AtualizarServicoAsync(id, dto);
        if (servicoAtualizado is null)
        {
            _logger.LogWarning("Serviço {ServicoId} não encontrado para atualização.", id);
            return NotFound();
        }

        return Ok(servicoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletarServico(int id)
    {
        var sucesso = await _servicoServico.DeletarServicoAsync(id);

        if (!sucesso)
        {
            _logger.LogWarning("Serviço {ServicoId} não encontrado para exclusão.", id);
            return NotFound();
        }

        return NoContent();
    }
}