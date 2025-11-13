namespace TrackOn.ClienteServico.Core.Exceptions;

public sealed class ClienteNaoEncontradoException : DomainException
{
    public ClienteNaoEncontradoException(int clienteId)
        : base($"Cliente com o identificador {clienteId} não foi encontrado.")
    {
    }
}