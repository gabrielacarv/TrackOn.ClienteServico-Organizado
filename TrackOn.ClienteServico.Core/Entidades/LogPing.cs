using System;

namespace TrackOn.ClienteServico.Core.Entidades;

public class LogPing
{
    public int Id { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public DateTime HoraPing { get; set; }
    public int ServicoId { get; set; }
    public DateTime CriadoEm { get; set; }
    public bool Ativo { get; set; }
    public Servico Servico { get; set; } = null!;
    public long? LatenciaMs { get; set; }
    public int? StatusCode { get; set; }
    public string? StatusDescricao { get; set; }
    public string? MensagemErro { get; set; }
    public string? ConteudoResposta { get; set; }
}