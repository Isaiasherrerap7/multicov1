using Delab.AccessData.Data;
using Delab.Helpers;
using Delab.Shared.Entities;
using Delab.Shared.Enum;

namespace Delab.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;

    public SeedDb(DataContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    // Cuando arranque el backend valida si existe la base de datos y si no existe la crea
    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountries();
        await CheckSoftPlan();
        await CheckRolesAsync();
        await CheckUserAsync("Intelnego", "SPI", "isaiasherrerap7@gmail.com", "+57 3007535633", UserType.Admin);
    }

    private async Task CheckSoftPlan()
    {
        if (!_context.SoftPlans.Any())
        {
            //Alimentando Planes
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 1 Mes",
                Price = 50,
                Meses = 1,
                ClinicsCount = 2,
                Active = true
            });
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 6 Mes",
                Price = 300,
                Meses = 6,
                ClinicsCount = 10,
                Active = true
            });
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 12 Mes",
                Price = 600,
                Meses = 12,
                ClinicsCount = 100,
                Active = true
            });
            await _context.SaveChangesAsync();
        }
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email,
            string phone, UserType userType)
    {
        User user = await _userHelper.GetUserAsync(email);
        if (user == null)
        {
            user = new()
            {
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}",
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                JobPosition = "Administrador",
                UserFrom = "SeedDb",
                UserRoleDetails = new List<UserRoleDetails> { new UserRoleDetails { UserType = userType } },
                Active = true,
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());

            //Para Confirmar automaticamente el Usuario y activar la cuenta
            string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            await _userHelper.ConfirmEmailAsync(user, token);
            await _userHelper.AddUserClaims(userType, email);
        }
        return user;
    }

    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        await _userHelper.CheckRoleAsync(UserType.Usuario.ToString());
        await _userHelper.CheckRoleAsync(UserType.Auxiliar.ToString());
        await _userHelper.CheckRoleAsync(UserType.Cajero.ToString());
        await _userHelper.CheckRoleAsync(UserType.Tecnico.ToString());
        await _userHelper.CheckRoleAsync(UserType.Clinica.ToString());
    }

    private async Task CheckCountries()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                Name = "Colombia",
                CodPhone = "+57",
                States = new List<State>
                {
                    new State
                    {
                        Name = "Antioquia",
                        Cities = new List<City>
                        {
                            new City { Name = "Medellin" },
                            new City { Name = "Bello" },
                            new City { Name = "Itagui" },
                            new City { Name = "Envigado" },
                            new City { Name = "Sabaneta" },
                        }
                    },
                    new State
                    {
                        Name = "Cundinamarca",
                        Cities = new List<City>
                        {
                            new City { Name = "Bogota" },
                            new City { Name = "Chia" },
                            new City { Name = "Cajica" },
                            new City { Name = "Zipaquira" },
                            new City { Name = "Soacha" }
                        }
                    }
                }
            });

            _context.Countries.Add(new Country
            {
                Name = "Mexico",
                CodPhone = "+52",
                States = new List<State>
                {
                    new State
                    {
                        Name = "Jalisco",
                        Cities = new List<City>
                        {
                            new City { Name = "Guadalajara" },
                            new City { Name = "Zapopan" },
                            new City { Name = "Tlaquepaque" },
                            new City { Name = "Tlajomulco de Zuniga" },
                            new City { Name = "El Salto" }
                        }
                    },
                    new State
                    {
                        Name = "Nuevo Leon",
                        Cities = new List<City>
                        {
                            new City { Name = "Monterrey" },
                            new City { Name = "San Pedro Garza Garcia" },
                            new City { Name = "Santa Catarina" },
                            new City { Name = "Apodaca" },
                            new City { Name = "Escobedo" }
                        }
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}