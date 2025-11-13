using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Core.Entidades.DTOs;

namespace TrackOn.ClienteServico.Application.Interfaces;

public interface IPainelServico
{
    Task<IEnumerable<PingPorHoraDTO>> ObterPingsPorHoraAsync(int clienteId, int? servicoId);
    Task<IEnumerable<DistribuicaoFalhaDTO>> ObterDistribuicaoFalhasAsync(int clienteId, int? servicoId);
    Task<IEnumerable<PercentualFalhaDTO>> ObterPercentualFalhasAsync(int clienteId, int? servicoId);
    Task<IEnumerable<TotalPingServicoDTO>> ObterTotalPingsPorServicoAsync(int clienteId, int? servicoId);
    Task<IEnumerable<TotalPingFalhoServicoDTO>> ObterTotalPingsFalhosPorServicoAsync(int clienteId, int? servicoId);
    Task<IEnumerable<UptimeDiarioDTO>> ObterUptimeDiarioPorServicoAsync(int clienteId, int? servicoId);
    Task<IEnumerable<ProporcaoUptimeDowntimeDTO>> ObterProporcaoUptimeDowntimeAsync(int clienteId, int? servicoId);
    Task<IEnumerable<HeatmapFalhaDTO>> ObterPadraoFalhasHeatmapAsync(int clienteId, int? servicoId);
    Task<UltimoPingDTO?> ObterUltimoRegistroPingAsync(int clienteId, int? servicoId);
    Task<IReadOnlyList<UltimoPingDTO>> ObterHistoricoSaudeAsync(int? clienteId, int? servicoId);
    Task<IEnumerable<UptimeHoraDTO>> ObterUptime24hAsync(int clienteId, int? servicoId = null);
    Task<IEnumerable<IncidentDTO>> ObterIncidentesAsync(int clienteId, int? servicoId);
    Task<IEnumerable<ResponseTimeDTO>> ObterResponseTimeAsync(int clienteId, int? servicoId = null, int dias = 30);
    Task<List<MotivoFalhaDTO>> ObterMotivosFalhasAsync(int clienteId, int? servicoId);
    Task<List<PingPorHoraDTO>> BuscarFalhasPorHoraAsync(int clienteId, int? servicoId);
}