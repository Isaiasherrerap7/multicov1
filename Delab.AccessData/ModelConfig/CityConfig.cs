using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class CityConfig : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        // Configuracion de la tabla con el Id
        builder.HasKey(e => e.CityId);
        // Configuracion de la tabla con el nombre de la ciudad unico y por estado
        builder.HasIndex(e => new { e.Name, e.StateId }).IsUnique();
        // Proteccion de borrado en cascada
        builder.HasOne(e => e.State)
            .WithMany(e => e.Cities)
            .OnDelete(DeleteBehavior.Restrict);
    }
}