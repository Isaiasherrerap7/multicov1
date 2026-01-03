using Delab.Frontend.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Delab.Frontend.AuthenticationProviders;

// Implementación de un proveedor de autenticación basado en JWT AuthenticationStateProvider con la ayuda de este paquete NuGe Podemos implementar un proveedor de autenticación personalizado que maneje tokens JWT para autenticar usuarios en una aplicación Blazor en cascada desde app razor.
public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
{
    private readonly IJSRuntime _jSRuntime;
    private readonly HttpClient _httpClient;
    private readonly string _tokenKey;
    private readonly AuthenticationState _anonimous;

    // Constructor que recibe las dependencias necesarias
    public AuthenticationProviderJWT(IJSRuntime jSRuntime, HttpClient httpClient)
    {
        _jSRuntime = jSRuntime;
        _httpClient = httpClient;
        _tokenKey = "TOKEN_KEY";
        _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    // Método para obtener el estado de autenticación actual
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jSRuntime.GetLocalStorage(_tokenKey);
        if (token is null)
        {
            return _anonimous;
        }

        return BuildAuthenticationState(token.ToString()!);
    }

    // Método para iniciar sesión con un token JWT
    public async Task LoginAsync(string token)
    {
        await _jSRuntime.SetLocalStorage(_tokenKey, token);
        var authState = BuildAuthenticationState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    // Método para cerrar sesión
    public async Task LogoutAsync()
    {
        await _jSRuntime.RemoveLocalStorage(_tokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));
    }

    // Método privado para construir el estado de autenticación a partir del token
    private AuthenticationState BuildAuthenticationState(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var claims = ParseClaimsFromJWT(token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }

    // Método privado para parsear los claims desde el token JWT
    private IEnumerable<Claim> ParseClaimsFromJWT(string token)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var unserializedToken = jwtSecurityTokenHandler.ReadJwtToken(token);
        return unserializedToken.Claims;
    }
}