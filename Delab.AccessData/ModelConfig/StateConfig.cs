using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

// Configuracion y reglas de la entidad State
public class StateConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        // Configuracion de la tabla con el Id
        builder.HasKey(e => e.StateId);
        // Configuracion de la tabla con el nombre del estado unico y por pais
        builder.HasIndex(e => new { e.Name, e.CountryId }).IsUnique();
        // Proteccion de borrado en cascada
        builder.HasOne(e => e.Country)
            .WithMany(e => e.States)
            .OnDelete(DeleteBehavior.Restrict);
    }
}