using System;

namespace TrackOn.ClienteServico.Core;

public abstract class Base
{
    public int Id { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; }
}