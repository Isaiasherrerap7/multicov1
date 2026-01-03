namespace Delab.Frontend.AuthenticationProviders;

// Interfaz que define los métodos para el servicio de inicio y cierre de sesión
public interface ILoginService
{
    Task LoginAsync(string token);

    Task LogoutAsync();
}