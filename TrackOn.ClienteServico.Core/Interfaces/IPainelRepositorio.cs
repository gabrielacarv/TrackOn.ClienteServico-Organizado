using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades.DTOs;

namespace TrackOn.ClienteServico.Core.Interfaces;

public interface IPainelRepositorio
{
    Task<List<PingPorHoraDTO>> BuscarPingsPorHoraAsync();
    Task<List<DistribuicaoFalhaDTO>> BuscarDistribuicaoFalhasAsync();
    Task<List<PercentualFalhaDTO>> BuscarPercentualFalhasAsync();
    Task<List<TotalPingServicoDTO>> BuscarTotalPingsPorServicoAsync();
    Task<List<TotalPingFalhoServicoDTO>> BuscarPingsFalhasAsync();
    Task<IEnumerable<IncidentDTO>> ObterIncidentesAsync(int clienteId, int? servicoId, int dias);
    Task<List<MotivoFalhaDTO>> ObterMotivosFalhasAsync(int clienteId, int? servicoId, int dias);
    Task<List<PingPorHoraDTO>> BuscarFalhasPorHoraAsync(int clienteId, int? servicoId, int dias);
}