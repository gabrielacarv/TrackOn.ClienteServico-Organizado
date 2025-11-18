using System;

namespace TrackOn.ClienteServico.Core.Entidades;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashSenha { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; }

    public void AtualizarDados(string? nome, string? email)
    {
        if (!string.IsNullOrWhiteSpace(nome))
            Nome = nome;

        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
    }
}