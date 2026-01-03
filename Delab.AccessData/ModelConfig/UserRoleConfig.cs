using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delab.AccessData.ModelConfig;

public class UserRoleConfig : IEntityTypeConfiguration<UserRoleDetails>
{
    public void Configure(EntityTypeBuilder<UserRoleDetails> builder)
    {
        // Configuracion de la tabla con el Id
        builder.HasKey(e => e.UserRoleDetailsId);
        // Configuracion de la tabla
        builder.HasIndex(e => new { e.UserType, e.UserId }).IsUnique();
        // Proteccion de borrado en cascada
        builder.HasOne(e => e.User)
            .WithMany(e => e.UserRoleDetails)
            .OnDelete(DeleteBehavior.Restrict);
    }
}