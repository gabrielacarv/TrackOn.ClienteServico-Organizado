using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades.DTOs;

namespace TrackOn.ClienteServico.Application.Interfaces;

public interface IServicoServico
{
    Task<IEnumerable<ServicoDTO>> ObterServicosPorClienteAsync(int clienteId);
    Task<ServicoDTO?> ObterServicoPorIdAsync(int id);
    Task<ServicoDTO> CriarServicoAsync(CriarServicoDTO dto);
    Task<ServicoDTO?> AtualizarServicoAsync(int id, AtualizarServicoDTO dto);
    Task<bool> DeletarServicoAsync(int id);
}