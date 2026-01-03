using Delab.AccessData.Data;
using Delab.Backend.Data;
using Delab.Helpers;
using Delab.Shared.Entities;
using Delab.Shared.ResponsesSec;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregamos el servicio para controlar las referencias ciclicas
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Servicio de Swagger funcion

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

// Conexion a Base de datos
builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer("name=DefaultConnection", option => option.MigrationsAssembly("Delab.Backend")));

// Sistema de seguridad autenticacion y autorizacion con el backend
//Para realizar logueo de los usuarios
builder.Services.AddIdentity<User, IdentityRole>(cfg =>
{
    //Agregamos Validar Correo para dar de alta al Usuario
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;

    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireUppercase = false;
    //Sistema para bloquear por 5 minutos al usuario por intento fallido
    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  //TODO: Cambiar Tiempo de Bloqueo a Usuarios
    cfg.Lockout.AllowedForNewUsers = true;
}).AddDefaultTokenProviders()  //Complemento Validar Correo
  .AddEntityFrameworkStores<DataContext>();

// Seguridad para la api controladores manejador jwtkey

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

//Configuracion de la Clase SendGridSetting para transportar los valores del AppSetting
builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));

builder.Services.AddTransient<SeedDb>(); // Agregamos el servicio para inicializar la base
builder.Services.AddScoped<IUtilityTools, UtilityTools>(); // Agregamos el servicio UtilityTools
builder.Services.AddScoped<IUserHelper, UserHelper>(); // Agregamos el servicio UserHelper
builder.Services.AddScoped<IFileStorage, FileStorage>(); // Agregamos el servicio FileStorage

//Inicio de Area de los Serviciios
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", corsBuilder =>
    {
        string allowedOrigins = "http://localhost:8080"; // Docker/Producción
        
        corsBuilder.WithOrigins(allowedOrigins)
             .AllowAnyHeader()
             .AllowAnyMethod()
             .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
    });
});

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (IServiceScope? scope = scopedFactory?.CreateScope())
    {
        SeedDb? seedDb = scope?.ServiceProvider.GetService<SeedDb>();
        seedDb?.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Llamar el Servicio de CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

// Servir archivos estáticos (Frontend Blazor WASM)
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Ruta por defecto para SPA (redirigir a index.html para rutas no encontradas)
app.MapFallbackToFile("index.html");

app.Run();
