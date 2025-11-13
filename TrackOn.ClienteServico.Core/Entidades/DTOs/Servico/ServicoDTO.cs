using System;

namespace TrackOn.ClienteServico.Core.Entidades.DTOs;

public class ServicoDTO
{
    public int Id { get; set; }
    public string EnderecoUrl { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public int ClienteId { get; set; }
    public DateTime CriadoEm { get; set; }
    public bool Ativo { get; set; }
}

public class CriarServicoDTO
{
    public string EnderecoUrl { get; set; } = string.Empty;
    public int Tipo { get; set; }
    public int ClienteId { get; set; }
}

public class AtualizarServicoDTO
{
    public string EnderecoUrl { get; set; } = string.Empty;
    public int Tipo { get; set; }
}