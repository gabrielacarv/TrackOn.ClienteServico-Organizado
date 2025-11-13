using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Core.Interfaces;

namespace TrackOn.ClienteServico.Infra.Repositorio;

public class PainelRepositorio : IPainelRepositorio
{
    private readonly AplicacaoDbContexto _context;

    public PainelRepositorio(AplicacaoDbContexto context)
    {
        _context = context;
    }

    public async Task<List<PingPorHoraDTO>> BuscarPingsPorHoraAsync()
    {
        var limite = DateTime.UtcNow.AddHours(-24);

        return await _context.Set<LogPing>()
            .Where(p => p.HoraPing >= limite)
            .GroupBy(p => p.HoraPing.Hour)
            .Select(g => new PingPorHoraDTO
            {
                Hora = g.Key,
                QuantidadeDePings = g.Count()
            })
            .OrderBy(r => r.Hora)
            .ToListAsync();
    }

    public async Task<List<DistribuicaoFalhaDTO>> BuscarDistribuicaoFalhasAsync()
    {
        return await _context.Set<LogPing>()
            .Where(p => !EF.Functions.Like(p.Observacao, "%sucesso%"))
            .GroupBy(p => p.Servico.Url)
            .Select(g => new DistribuicaoFalhaDTO
            {
                EnderecoUrl = g.Key ?? string.Empty,
                QuantidadeFalhas = g.Count()
            })
            .OrderByDescending(r => r.QuantidadeFalhas)
            .ToListAsync();
    }

    public async Task<List<PercentualFalhaDTO>> BuscarPercentualFalhasAsync()
    {
        var limite = DateTime.UtcNow.AddDays(-30);

        return await _context.Set<LogPing>()
            .Where(p => p.HoraPing.Date >= limite.Date)
            .GroupBy(p => p.HoraPing.Date)
            .Select(g => new PercentualFalhaDTO
            {
                Data = g.Key,
                PercentualDeFalhas = g.Count(p => !EF.Functions.Like(p.Observacao, "%sucesso%")) * 100.0 / g.Count()
            })
            .OrderBy(r => r.Data)
            .ToListAsync();
    }

    public async Task<List<TotalPingServicoDTO>> BuscarTotalPingsPorServicoAsync()
    {
        return await _context.Set<LogPing>()
            .GroupBy(p => p.Servico.Url)
            .Select(g => new TotalPingServicoDTO
            {
                NomeServico = g.Key ?? string.Empty,
                TotalDePings = g.Count()
            })
            .OrderByDescending(r => r.TotalDePings)
            .ToListAsync();
    }

    public async Task<List<TotalPingFalhoServicoDTO>> BuscarPingsFalhasAsync()
    {
        return await _context.Set<LogPing>()
            .Where(p => !EF.Functions.Like(p.Observacao, "%sucesso%"))
            .GroupBy(p => p.Servico.Url)
            .Select(g => new TotalPingFalhoServicoDTO
            {
                NomeServico = g.Key ?? string.Empty,
                PingsFalhos = g.Count()
            })
            .OrderByDescending(r => r.PingsFalhos)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentDTO>> ObterIncidentesAsync(int clienteId, int? servicoId)
    {
        var query = _context.LogsPing
            .Where(l => l.Servico.ClienteId == clienteId && !l.Ativo);

        if (servicoId.HasValue)
        {
            query = query.Where(l => l.ServicoId == servicoId.Value);
        }

        return await query
            .OrderByDescending(l => l.HoraPing)
            .Take(5)
            .Select(l => new IncidentDTO
            {
                Id = l.Id,
                Ativo = l.Ativo,
                CodigoStatus = l.StatusCode,
                DescricaoStatus = l.StatusDescricao ?? string.Empty,
                CausaRaiz = l.MensagemErro ?? string.Empty,
                Inicio = l.HoraPing,
                EnderecoUrl = l.Servico.Url ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<List<MotivoFalhaDTO>> ObterMotivosFalhasAsync(int clienteId, int? servicoId)
    {
        var query = _context.LogsPing
            .Where(l => l.Servico.ClienteId == clienteId && !l.Ativo);

        if (servicoId.HasValue)
        {
            query = query.Where(l => l.ServicoId == servicoId.Value);
        }

        return await query
            .GroupBy(l => l.StatusDescricao ?? "Desconhecido")
            .Select(g => new MotivoFalhaDTO
            {
                Motivo = g.Key,
                Quantidade = g.Count()
            })
            .OrderByDescending(x => x.Quantidade)
            .Take(5)
            .ToListAsync();
    }

    public async Task<List<PingPorHoraDTO>> BuscarFalhasPorHoraAsync(int clienteId, int? servicoId)
    {
        var query = _context.LogsPing
            .Where(log => log.Servico.ClienteId == clienteId && EF.Functions.Like(log.Observacao, "%Falh%"));

        if (servicoId.HasValue)
        {
            query = query.Where(x => x.ServicoId == servicoId.Value);
        }

        var dados = await query
            .GroupBy(p => p.HoraPing.Hour)
            .Select(g => new { Hora = g.Key, Quantidade = g.Count() })
            .OrderBy(x => x.Hora)
            .ToListAsync();

        return dados
            .Select(x => new PingPorHoraDTO
            {
                Hora = x.Hora,
                QuantidadeDePings = x.Quantidade
            })
            .ToList();
    }
}