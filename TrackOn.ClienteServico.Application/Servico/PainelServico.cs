using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Core.Interfaces;
using TrackOn.ClienteServico.Infra;

namespace TrackOn.ClienteServico.Application.Servico;

public class PainelServico : IPainelServico
{
    private readonly AplicacaoDbContexto _context;
    private readonly IPainelRepositorio _painelRepositorio;

    public PainelServico(AplicacaoDbContexto context, IPainelRepositorio painelRepositorio)
    {
        _context = context;
        _painelRepositorio = painelRepositorio;
    }

    public async Task<IEnumerable<PingPorHoraDTO>> ObterPingsPorHoraAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(l => l.HoraPing.Hour)
            .Select(g => new PingPorHoraDTO
            {
                Hora = g.Key,
                QuantidadeDePings = g.Count()
            })
            .OrderBy(x => x.Hora)
            .ToListAsync();
    }

    public async Task<IEnumerable<DistribuicaoFalhaDTO>> ObterDistribuicaoFalhasAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .Where(l => !EF.Functions.Like(l.Observacao.ToLower(), "%ok%"))
            .GroupBy(l => l.Servico.Url)
            .Select(g => new DistribuicaoFalhaDTO
            {
                EnderecoUrl = g.Key ?? string.Empty,
                QuantidadeFalhas = g.Count()
            })
            .OrderByDescending(x => x.QuantidadeFalhas)
            .ToListAsync();
    }

    public async Task<IEnumerable<PercentualFalhaDTO>> ObterPercentualFalhasAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(l => l.HoraPing.Date)
            .Select(g => new PercentualFalhaDTO
            {
                Data = g.Key,
                PercentualDeFalhas = g.Any()
                    ? g.Count(l => !EF.Functions.Like(l.Observacao.ToLower(), "%ok%")) * 100.0 / g.Count()
                    : 0
            })
            .OrderBy(x => x.Data)
            .ToListAsync();
    }

    public async Task<IEnumerable<TotalPingServicoDTO>> ObterTotalPingsPorServicoAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(l => l.Servico.Url)
            .Select(g => new TotalPingServicoDTO
            {
                NomeServico = g.Key ?? string.Empty,
                TotalDePings = g.Count()
            })
            .OrderByDescending(x => x.TotalDePings)
            .ToListAsync();
    }

    public async Task<IEnumerable<TotalPingFalhoServicoDTO>> ObterTotalPingsFalhosPorServicoAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .Where(l => !EF.Functions.Like(l.Observacao.ToLower(), "%ok%"))
            .GroupBy(l => l.Servico.Url)
            .Select(g => new TotalPingFalhoServicoDTO
            {
                NomeServico = g.Key ?? string.Empty,
                PingsFalhos = g.Count()
            })
            .OrderByDescending(x => x.PingsFalhos)
            .ToListAsync();
    }

    public async Task<IEnumerable<UptimeDiarioDTO>> ObterUptimeDiarioPorServicoAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(p => new { Data = p.HoraPing.Date, p.ServicoId })
            .Select(g => new UptimeDiarioDTO
            {
                Data = g.Key.Data,
                IdServico = g.Key.ServicoId,
                PorcentagemDeDisponibilidade = Math.Round(
                    g.Count(p => EF.Functions.Like(p.Observacao.ToLower(), "%ok%")) * 100.0 / g.Count(),
                    2)
            })
            .OrderBy(r => r.Data)
            .ThenBy(r => r.IdServico)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProporcaoUptimeDowntimeDTO>> ObterProporcaoUptimeDowntimeAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(p => new { p.ServicoId, p.Servico.Url })
            .Select(g => new ProporcaoUptimeDowntimeDTO
            {
                NomeServico = g.Key.Url ?? string.Empty,
                QuantidadeDisponivel = g.Count(p => EF.Functions.Like(p.Observacao.ToLower(), "%ok%")),
                QuantidadeIndisponivel = g.Count(p => EF.Functions.Like(p.Observacao.ToLower(), "%falh%"))
            })
            .OrderByDescending(r => r.QuantidadeIndisponivel)
            .ToListAsync();
    }

    public async Task<IEnumerable<HeatmapFalhaDTO>> ObterPadraoFalhasHeatmapAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .Where(p => EF.Functions.Like(p.Observacao.ToLower(), "%falh%"))
            .Select(p => new { p.HoraPing })
            .GroupBy(p => new { p.HoraPing.DayOfWeek, p.HoraPing.Hour })
            .Select(g => new HeatmapFalhaDTO
            {
                DiaDaSemana = g.Key.DayOfWeek,
                Hora = g.Key.Hour,
                QuantidadeFalhas = g.Count()
            })
            .OrderBy(x => x.DiaDaSemana)
            .ThenBy(x => x.Hora)
            .ToListAsync();
    }

    public async Task<UltimoPingDTO?> ObterUltimoRegistroPingAsync(int clienteId, int? servicoId, int dias)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .OrderByDescending(p => p.HoraPing)
            .Select(p => new UltimoPingDTO
            {
                HoraPing = p.HoraPing,
                Observacao = p.Observacao
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<UltimoPingDTO>> ObterHistoricoSaudeAsync(int? clienteId, int? servicoId, int dias)
    {
        var resolvedClienteId = await ResolverClienteIdAsync(clienteId);
        if (resolvedClienteId is null)
        {
            return Array.Empty<UltimoPingDTO>();
        }

        return await FiltrarLogs(resolvedClienteId.Value, servicoId, dias)
            .OrderByDescending(p => p.HoraPing)
            .Take(20)
            .Select(p => new UltimoPingDTO
            {
                HoraPing = p.HoraPing,
                Observacao = p.Observacao
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<UptimeHoraDTO>> ObterUptime24hAsync(int clienteId, int? servicoId = null, int dias = 1)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .GroupBy(l => l.HoraPing.Hour)
            .Select(g => new UptimeHoraDTO
            {
                Hora = g.Key,
                Total = g.Count(),
                QuantidadeSucesso = g.Count(x => EF.Functions.Like(x.Observacao.ToLower(), "%ok%")),
                QuantidadeFalha = g.Count(x => EF.Functions.Like(x.Observacao.ToLower(), "%falha%"))
            })
            .OrderBy(x => x.Hora)
            .ToListAsync();
    }

    public Task<IEnumerable<IncidentDTO>> ObterIncidentesAsync(int clienteId, int? servicoId, int dias)
    {
        return _painelRepositorio.ObterIncidentesAsync(clienteId, servicoId, dias);
    }

    public async Task<IEnumerable<ResponseTimeDTO>> ObterResponseTimeAsync(int clienteId, int? servicoId = null, int dias = 30)
    {
        return await FiltrarLogs(clienteId, servicoId, dias)
            .OrderBy(l => l.HoraPing)
            .Select(l => new ResponseTimeDTO
            {
                DataHora = l.HoraPing,
                Latencia = l.LatenciaMs
            })
            .ToListAsync();
    }

    public Task<List<MotivoFalhaDTO>> ObterMotivosFalhasAsync(int clienteId, int? servicoId, int dias)
    {
        return _painelRepositorio.ObterMotivosFalhasAsync(clienteId, servicoId, dias);
    }

    public Task<List<PingPorHoraDTO>> BuscarFalhasPorHoraAsync(int clienteId, int? servicoId, int dias)
    {
        return _painelRepositorio.BuscarFalhasPorHoraAsync(clienteId, servicoId, dias);
    }

    private IQueryable<LogPing> FiltrarLogs(int clienteId, int? servicoId, int? dias)
    {
        var query = _context.LogsPing
            .AsNoTracking()
            .Where(l => l.Servico.ClienteId == clienteId);

        if (servicoId.HasValue)
        {
            query = query.Where(l => l.ServicoId == servicoId.Value);
        }

        if (dias.HasValue && dias.Value > 0)
        {
            var limite = DateTime.UtcNow.AddDays(-dias.Value);
            query = query.Where(l => l.HoraPing >= limite);
        }

        return query;
    }

    private async Task<int?> ResolverClienteIdAsync(int? clienteId)
    {
        if (clienteId.HasValue && clienteId.Value > 0)
        {
            return clienteId.Value;
        }

        return await _context.Clientes
            .AsNoTracking()
            .Select(c => (int?)c.Id)
            .FirstOrDefaultAsync();
    }
}
