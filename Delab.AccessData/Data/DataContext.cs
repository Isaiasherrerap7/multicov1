using Delab.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Delab.AccessData.Data;

// 1.Para la clase DataContex heredamos de DbContext(Despues se cancela dbcontex por Identity como extension de manejo)
// 2. Creamos el Constructor apartir del DbContext
public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // 3. Creamos las propiedades DbSet para cada entidad para usar en la base de datos
    // Manejo de Usuarios
    public DbSet<UserRoleDetails> UserRoleDetails => Set<UserRoleDetails>();

    // Entidades de la Aplicacion

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<SoftPlan> SoftPlans => Set<SoftPlan>();
    public DbSet<Corporation> Corporations => Set<Corporation>();
    public DbSet<Manager> Managers => Set<Manager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Para tomar los calores de ConfigEntities
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}