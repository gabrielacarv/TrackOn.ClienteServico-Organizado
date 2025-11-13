using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Entidades.DTOs;

namespace TrackOn.ClienteServico.Application.Interfaces;

public interface IClienteServico
{
    Task<Cliente?> ObterClientePorIdAsync(int id);
    Task<Cliente?> ObterClientePorEmailAsync(string email);
    Task<IEnumerable<Cliente>> ObterTodosClientesAsync();
    Task<Cliente> AdicionarClienteAsync(CriarClienteDTO clienteDto);
    Task AtualizarClienteAsync(int id, AtualizarClienteDTO clienteDto);
    Task<bool> AutenticarClienteAsync(string email, string senha);
    Task AlterarSenhaAsync(int id, AlterarSenhaDTO dto);
}