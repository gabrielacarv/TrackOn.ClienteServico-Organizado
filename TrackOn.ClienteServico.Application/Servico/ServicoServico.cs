using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Infra;

namespace TrackOn.ClienteServico.Application.Servico;

public class ServicoServico : IServicoServico
{
    private readonly AplicacaoDbContexto _context;

    public ServicoServico(AplicacaoDbContexto context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServicoDTO>> ObterServicosPorClienteAsync(int clienteId)
    {
        return await _context.Servicos
            .AsNoTracking()
            .Where(s => s.ClienteId == clienteId && s.Ativo)
            .Select(s => new ServicoDTO
            {
                Id = s.Id,
                EnderecoUrl = s.Url,
                Tipo = s.Tipo,
                ClienteId = s.ClienteId,
                CriadoEm = s.CriadoEm,
                Ativo = s.Ativo
            })
            .ToListAsync();
    }

    public async Task<ServicoDTO?> ObterServicoPorIdAsync(int id)
    {
        var servico = await _context.Servicos.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (servico is null)
        {
            return null;
        }

        return new ServicoDTO
        {
            Id = servico.Id,
            EnderecoUrl = servico.Url,
            Tipo = servico.Tipo,
            ClienteId = servico.ClienteId,
            CriadoEm = servico.CriadoEm,
            Ativo = servico.Ativo
        };
    }

    public async Task<ServicoDTO> CriarServicoAsync(CriarServicoDTO dto)
    {
        var novoServico = new TrackOn.ClienteServico.Core.Entidades.Servico
        {
            Url = dto.EnderecoUrl,
            Tipo = dto.Tipo,
            ClienteId = dto.ClienteId,
            CriadoEm = DateTime.UtcNow,
            Ativo = true
        };

        await _context.Servicos.AddAsync(novoServico);
        await _context.SaveChangesAsync();

        return new ServicoDTO
        {
            Id = novoServico.Id,
            EnderecoUrl = novoServico.Url,
            Tipo = novoServico.Tipo,
            ClienteId = novoServico.ClienteId,
            CriadoEm = novoServico.CriadoEm,
            Ativo = novoServico.Ativo
        };
    }

    public async Task<ServicoDTO?> AtualizarServicoAsync(int id, AtualizarServicoDTO dto)
    {
        var servico = await _context.Servicos.FindAsync(id);

        if (servico is null || !servico.Ativo)
        {
            return null;
        }

        servico.Url = dto.EnderecoUrl;
        servico.Tipo = dto.Tipo;

        await _context.SaveChangesAsync();

        return new ServicoDTO
        {
            Id = servico.Id,
            EnderecoUrl = servico.Url,
            Tipo = servico.Tipo,
            ClienteId = servico.ClienteId,
            CriadoEm = servico.CriadoEm,
            Ativo = servico.Ativo
        };
    }

    public async Task<bool> DeletarServicoAsync(int id)
    {
        var servico = await _context.Servicos.FindAsync(id);

        if (servico is null || !servico.Ativo)
        {
            return false;
        }

        servico.Ativo = false;
        await _context.SaveChangesAsync();
        return true;
    }
}