using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Entidades.DTOs;
using TrackOn.ClienteServico.Core.Exceptions;
using TrackOn.ClienteServico.Core.Interfaces;

namespace TrackOn.ClienteServico.Application.Servico;

public class ClienteServico : IClienteServico
{
    private readonly IClienteRepositorio _repositorioCliente;

    public ClienteServico(IClienteRepositorio repositorioCliente)
    {
        _repositorioCliente = repositorioCliente;
    }

    public Task<Cliente?> ObterClientePorIdAsync(int id)
    {
        return _repositorioCliente.ObterClientePorIdAsync(id);
    }

    public Task<Cliente?> ObterClientePorEmailAsync(string email)
    {
        return _repositorioCliente.ObterClientePorEmailAsync(email);
    }

    public Task<IEnumerable<Cliente>> ObterTodosClientesAsync()
    {
        return _repositorioCliente.ObterTodosClientesAsync();
    }

    public async Task<Cliente> AdicionarClienteAsync(CriarClienteDTO clienteDto)
    {
        var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(clienteDto.Senha);
        var cliente = new Cliente
        {
            Nome = clienteDto.Nome,
            Email = clienteDto.Email,
            HashSenha = senhaCriptografada,
            CriadoEm = DateTime.UtcNow,
            Ativo = true
        };

        await _repositorioCliente.AdicionarClienteAsync(cliente);
        return cliente;
    }

    public async Task AtualizarClienteAsync(int id, AtualizarClienteDTO clienteDto)
    {
        var cliente = await _repositorioCliente.ObterClientePorIdAsync(id);
        if (cliente is null)
        {
            throw new ClienteNaoEncontradoException(id);
        }

        cliente.Nome = clienteDto.Nome ?? cliente.Nome;
        cliente.Email = clienteDto.Email ?? cliente.Email;

        await _repositorioCliente.AtualizarClienteAsync(cliente);
    }

    public async Task<bool> AutenticarClienteAsync(string email, string senha)
    {
        var cliente = await _repositorioCliente.ObterClientePorEmailAsync(email);
        return cliente is not null && BCrypt.Net.BCrypt.Verify(senha, cliente.HashSenha);
    }

    public async Task AlterarSenhaAsync(int id, AlterarSenhaDTO dto)
    {
        var cliente = await _repositorioCliente.ObterClientePorIdAsync(id);
        if (cliente is null)
        {
            throw new ClienteNaoEncontradoException(id);
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.SenhaAtual, cliente.HashSenha))
        {
            throw new SenhaAtualIncorretaException();
        }

        cliente.HashSenha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
        await _repositorioCliente.AtualizarClienteAsync(cliente);
    }
}