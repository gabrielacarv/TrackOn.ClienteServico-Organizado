namespace TrackOn.ClienteServico.Core.Entidades.DTOs;

public class AtualizarClienteDTO
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
}

public class AlterarSenhaDTO
{
    public string SenhaAtual { get; set; } = string.Empty;
    public string NovaSenha { get; set; } = string.Empty;
}