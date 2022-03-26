using System.Text;
using CadastroClienteService;
using CadastroClienteService.Data;
using CadastroClienteService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniValidation;

const string VERSION = "v1";
const string BASE_ENDPOINT = $"/api/{VERSION}";

var key = Encoding.ASCII.GetBytes(Settings.Secret);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<CadastroClienteDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        Description = "Autenticação Bearer com token JWT",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
            },
            new List<string>()
        }
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UsePathBase("/swagger/index.html");
app.UseAuthentication();
app.UseAuthorization();

#region CRUD
app.MapGet($"{BASE_ENDPOINT}/clientes", async (CadastroClienteDbContext context) =>
{
    var clientes = await context.Clientes.ToListAsync();

    return Results.Ok(clientes);
})
.RequireAuthorization()
.Produces<Cliente>();

app.MapGet($"{BASE_ENDPOINT}/clientes" + "/{id}", async (Guid id, CadastroClienteDbContext context) =>
{
    var cliente = await context.Clientes.FindAsync(id);

    if (cliente == null)
        return Results.NotFound(new { message = "Cliente não encontrado" });

    return Results.Ok(cliente);
})
.RequireAuthorization()
.Produces<Cliente>();

app.MapPost($"{BASE_ENDPOINT}/clientes", async (Cliente cliente, CadastroClienteDbContext context) =>
{
    MiniValidator.TryValidate(cliente, out var errors);

    var clienteDb = await context.Clientes.FirstOrDefaultAsync(
        x => x.CpfCnpj == cliente.CpfCnpj
        || x.RgInscEst == cliente.RgInscEst
        || x.Telefone == cliente.Telefone
        || x.Celular == cliente.Celular);

    if (clienteDb != null)
        return Results.BadRequest(new { Message = "Cliente já cadastrado" });

    cliente.CreatedAt = DateTime.UtcNow;

    if (errors.Count > 0)
        return Results.BadRequest(errors);
    else
    {
        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        return Results.Created($"{BASE_ENDPOINT}/clientes/{cliente.Id}", cliente);
    }
})
.RequireAuthorization()
.Produces<Cliente>(201);

app.MapPut($"{BASE_ENDPOINT}/clientes", async (Cliente cliente, CadastroClienteDbContext context) =>
{
    var dbCliente = await context.Clientes.FindAsync(cliente.Id);

    if (dbCliente == null)
        return Results.NotFound(new { message = "Cliente não encontrado" });

    dbCliente.NomeRazaoSocial = cliente.NomeRazaoSocial;
    dbCliente.CpfCnpj = cliente.CpfCnpj;
    dbCliente.RgInscEst = cliente.RgInscEst;
    dbCliente.Logradouro = cliente.Logradouro;
    dbCliente.Complemento = cliente.Complemento;
    dbCliente.Numero = cliente.Numero;
    dbCliente.Bairro = cliente.Bairro;
    dbCliente.Cidade = cliente.Cidade;
    dbCliente.Cep = cliente.Cep;
    dbCliente.Estado = cliente.Estado;
    dbCliente.Telefone = cliente.Telefone;
    dbCliente.Celular = cliente.Celular;
    dbCliente.UpdatedAt = DateTime.UtcNow;

    await context.SaveChangesAsync();

    return Results.Ok(dbCliente);
})
.RequireAuthorization()
.Produces<Cliente>();

app.MapDelete($"{BASE_ENDPOINT}/clientes" + "/{id}", async (Guid id, CadastroClienteDbContext context) =>
{
    var cliente = await context.Clientes.FindAsync(id);

    if (cliente == null)
        return Results.NotFound(new { message = "Cliente não encontrado" });

    context.Clientes.Remove(cliente);
    await context.SaveChangesAsync();

    return Results.NoContent();
})
.RequireAuthorization()
.Produces<Cliente>(204);
#endregion

app.Run();
