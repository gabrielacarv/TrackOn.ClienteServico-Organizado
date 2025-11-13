namespace TrackOn.ClienteServico.Core.Exceptions;

public sealed class SenhaAtualIncorretaException : DomainException
{
    public SenhaAtualIncorretaException()
        : base("A senha atual informada não confere com a senha cadastrada.")
    {
    }
}