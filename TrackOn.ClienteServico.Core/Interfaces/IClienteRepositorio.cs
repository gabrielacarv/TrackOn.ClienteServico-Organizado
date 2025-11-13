using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades;

namespace TrackOn.ClienteServico.Core.Interfaces;

public interface IClienteRepositorio
{
    Task<Cliente?> ObterClientePorIdAsync(int id);
    Task<Cliente?> ObterClientePorEmailAsync(string email);
    Task<IEnumerable<Cliente>> ObterTodosClientesAsync();
    Task AdicionarClienteAsync(Cliente cliente);
    Task AtualizarClienteAsync(Cliente cliente);
}