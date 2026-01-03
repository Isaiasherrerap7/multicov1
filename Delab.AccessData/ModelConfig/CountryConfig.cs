using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

// Configuracion y reglas de la entidad Country
public class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        // Configuracion de la tabla con el Id
        builder.HasKey(e => e.CountryId);
        // Configuracion de la tabla con el nombre del pais unico
        builder.HasIndex(e => e.Name).IsUnique();
    }
}