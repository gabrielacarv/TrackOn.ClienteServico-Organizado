using System;

namespace TrackOn.ClienteServico.Core.Entidades.DTOs;

public sealed class PingPorHoraDTO
{
    public int Hora { get; init; }
    public int QuantidadeDePings { get; init; }
}

public sealed class DistribuicaoFalhaDTO
{
    public string EnderecoUrl { get; init; } = string.Empty;
    public int QuantidadeFalhas { get; init; }
}

public sealed class PercentualFalhaDTO
{
    public DateTime Data { get; init; }
    public double PercentualDeFalhas { get; init; }
}

public sealed class TotalPingServicoDTO
{
    public string NomeServico { get; init; } = string.Empty;
    public int TotalDePings { get; init; }
}

public sealed class TotalPingFalhoServicoDTO
{
    public string NomeServico { get; init; } = string.Empty;
    public int PingsFalhos { get; init; }
}

public sealed class IncidentDTO
{
    public int Id { get; init; }
    public bool Ativo { get; init; }
    public int? CodigoStatus { get; init; }
    public string DescricaoStatus { get; init; } = string.Empty;
    public string CausaRaiz { get; init; } = string.Empty;
    public DateTime Inicio { get; init; }
    public string EnderecoUrl { get; init; } = string.Empty;
}

public sealed class MotivoFalhaDTO
{
    public string Motivo { get; init; } = string.Empty;
    public int Quantidade { get; init; }
}

public sealed class HeatmapFalhaDTO
{
    public DayOfWeek DiaDaSemana { get; init; }
    public int Hora { get; init; }
    public int QuantidadeFalhas { get; init; }
}

public sealed class UptimeDiarioDTO
{
    public DateTime Data { get; init; }
    public int IdServico { get; init; }
    public double PorcentagemDeDisponibilidade { get; init; }
}

public sealed class ProporcaoUptimeDowntimeDTO
{
    public string NomeServico { get; init; } = string.Empty;
    public int QuantidadeDisponivel { get; init; }
    public int QuantidadeIndisponivel { get; init; }
}

public sealed class UltimoPingDTO
{
    public DateTime HoraPing { get; init; }
    public string? Observacao { get; init; }
}

public sealed class UptimeHoraDTO
{
    public int Hora { get; init; }
    public int Total { get; init; }
    public int QuantidadeSucesso { get; init; }
    public int QuantidadeFalha { get; init; }
}

public sealed class ResponseTimeDTO
{
    public DateTime DataHora { get; init; }
    public long? Latencia { get; init; }
}