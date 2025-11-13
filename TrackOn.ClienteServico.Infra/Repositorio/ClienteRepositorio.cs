using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Interfaces;

namespace TrackOn.ClienteServico.Infra.Repositorio;

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly AplicacaoDbContexto _contexto;

    public ClienteRepositorio(AplicacaoDbContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Cliente?> ObterClientePorIdAsync(int id)
    {
        return await _contexto.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> ObterClientePorEmailAsync(string email)
    {
        return await _contexto.Clientes.SingleOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync()
    {
        return await _contexto.Clientes.AsNoTracking().ToListAsync();
    }

    public async Task AdicionarClienteAsync(Cliente cliente)
    {
        await _contexto.Clientes.AddAsync(cliente);
        await _contexto.SaveChangesAsync();
    }

    public async Task AtualizarClienteAsync(Cliente cliente)
    {
        _contexto.Clientes.Update(cliente);
        await _contexto.SaveChangesAsync();
    }
}