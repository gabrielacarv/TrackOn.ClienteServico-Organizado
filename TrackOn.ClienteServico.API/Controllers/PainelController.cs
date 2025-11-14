using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Application.Interfaces;

namespace TrackOn.ClienteServico.API.Controllers;

[ApiController]
[Route("api/clientes/{clienteId:int}/painel")]
public class PainelController : ControllerBase
{
    private readonly IPainelServico _painelServico;

    public PainelController(IPainelServico painelServico)
    {
        _painelServico = painelServico;
    }

    [HttpGet("pings-por-hora")]
    public async Task<IActionResult> GetPingsPorHora(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterPingsPorHoraAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("distribuicao-falhas")]
    public async Task<IActionResult> GetDistribuicaoFalhas(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterDistribuicaoFalhasAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("percentual-falhas")]
    public async Task<IActionResult> GetPercentualFalhas(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterPercentualFalhasAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("total-pings-por-servico")]
    public async Task<IActionResult> GetTotalPingsPorServico(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterTotalPingsPorServicoAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("total-falhas-por-servico")]
    public async Task<IActionResult> GetTotalPingsFalhosPorServico(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterTotalPingsFalhosPorServicoAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("uptime-diario")]
    public async Task<IActionResult> GetUptimeDiarioPorServico(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterUptimeDiarioPorServicoAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("proporcao-uptime-downtime")]
    public async Task<IActionResult> GetProporcaoUptimeDowntime(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterProporcaoUptimeDowntimeAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("padrao-falhas")]
    public async Task<IActionResult> GetPadraoFalhasHeatmap(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterPadraoFalhasHeatmapAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("ultimo-ping")]
    public async Task<IActionResult> GetUltimoRegistroPing(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterUltimoRegistroPingAsync(clienteId, servicoId, dias);
        return data is null ? NotFound() : Ok(data);
    }

    [HttpGet("historico-saude")]
    public async Task<IActionResult> ObterHistoricoSaude([FromRoute] int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var data = await _painelServico.ObterHistoricoSaudeAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("uptime-24h")]
    public async Task<IActionResult> GetUptime24h(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 1)
    {
        var data = await _painelServico.ObterUptime24hAsync(clienteId, servicoId, dias);
        return Ok(data);
    }

    [HttpGet("incidentes")]
    public async Task<IActionResult> ObterIncidentes(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var result = await _painelServico.ObterIncidentesAsync(clienteId, servicoId, dias);
        return Ok(result);
    }

    [HttpGet("response-time")]
    public async Task<IActionResult> GetResponseTime(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var result = await _painelServico.ObterResponseTimeAsync(clienteId, servicoId, dias);
        return Ok(result);
    }

    [HttpGet("motivos-falhas")]
    public async Task<IActionResult> GetFailureReasons(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var result = await _painelServico.ObterMotivosFalhasAsync(clienteId, servicoId, dias);
        return Ok(result);
    }

    [HttpGet("falhas-por-hora")]
    public async Task<IActionResult> GetFalhasPorHora(int clienteId, [FromQuery] int? servicoId, [FromQuery] int dias = 30)
    {
        var resultado = await _painelServico.BuscarFalhasPorHoraAsync(clienteId, servicoId, dias);
        return Ok(resultado);
    }
}