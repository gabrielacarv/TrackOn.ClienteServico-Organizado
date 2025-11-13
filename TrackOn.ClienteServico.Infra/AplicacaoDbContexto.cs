using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Core.Entidades;

namespace TrackOn.ClienteServico.Infra;

public class AplicacaoDbContexto : DbContext
{
    public AplicacaoDbContexto(DbContextOptions<AplicacaoDbContexto> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<LogPing> LogsPing => Set<LogPing>();
    public DbSet<Servico> Servicos => Set<Servico>();
}