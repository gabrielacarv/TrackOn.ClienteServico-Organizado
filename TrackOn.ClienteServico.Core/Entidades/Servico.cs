using System.Collections.Generic;

namespace TrackOn.ClienteServico.Core.Entidades;

public class Servico : Base
{
    public string Url { get; set; } = string.Empty;
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public int Tipo { get; set; }
    public ICollection<LogPing> LogsPing { get; set; } = new List<LogPing>();
}