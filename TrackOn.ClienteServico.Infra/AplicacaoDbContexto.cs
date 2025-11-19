using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.Core.Entidades;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<DateTime, DateTime>(
            v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified),
            v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified)
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(converter);

                if (property.ClrType == typeof(DateTime?))
                {
                    var nullableConverter = new ValueConverter<DateTime?, DateTime?>(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Unspecified) : v
                    );

                    property.SetValueConverter(nullableConverter);
                }
            }
        }

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.HashSenha).HasColumnName("hashsenha");
            entity.Property(e => e.CriadoEm).HasColumnName("criado_em");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.ToTable("servico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url).HasColumnName("url");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.CriadoEm).HasColumnName("criado_em");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.UltimoStatusAtivo).HasColumnName("ultimo_status_ativo");
            entity.Property(e => e.UltimaNotificacao).HasColumnName("ultima_notificacao");
        });

        modelBuilder.Entity<LogPing>(entity =>
        {
            entity.ToTable("logsping");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Observacao).HasColumnName("observacao");
            entity.Property(e => e.HoraPing).HasColumnName("horaping");
            entity.Property(e => e.ServicoId).HasColumnName("servico_id");
            entity.Property(e => e.CriadoEm).HasColumnName("criado_em");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.LatenciaMs).HasColumnName("latenciams");
            entity.Property(e => e.MensagemErro).HasColumnName("mensagemerro");
            entity.Property(e => e.ConteudoResposta).HasColumnName("conteudoresposta");
            entity.Property(e => e.StatusCode).HasColumnName("statuscode");
            entity.Property(e => e.StatusDescricao).HasColumnName("statusdescricao");

            entity.HasOne(e => e.Servico)
                  .WithMany(s => s.LogsPing)
                  .HasForeignKey(e => e.ServicoId);
        });
    }
}
