using Npgsql;
using Microsoft.EntityFrameworkCore;
using TrackOn.ClienteServico.API.Extensions;
using TrackOn.ClienteServico.Application.Interfaces;
using TrackOn.ClienteServico.Application.Servico;
using TrackOn.ClienteServico.Core.Entidades;
using TrackOn.ClienteServico.Core.Interfaces;
using TrackOn.ClienteServico.Infra;
using TrackOn.ClienteServico.Infra.Repositorio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AplicacaoDbContexto>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddApiCors(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddScoped<IClienteServico, ClienteServico>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IPainelRepositorio, PainelRepositorio>();
builder.Services.AddScoped<IPainelServico, PainelServico>();
builder.Services.AddScoped<IServicoServico, ServicoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(ServiceCollectionExtensions.DefaultCorsPolicy);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();