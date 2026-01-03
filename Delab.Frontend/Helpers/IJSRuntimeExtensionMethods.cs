using Microsoft.JSInterop;

namespace Delab.Frontend.Helpers;

// Extensiones para IJSRuntime para manejar el almacenamiento local (localStorage) de la key del navegador
public static class IJSRuntimeExtensionMethods
{
    // Establece un ítem en el almacenamiento local
    public static ValueTask<object> SetLocalStorage(this IJSRuntime js, string key, string content)
    {
        return js.InvokeAsync<object>("localStorage.setItem", key, content);
    }

    // Obtiene un ítem del almacenamiento local
    public static ValueTask<object> GetLocalStorage(this IJSRuntime js, string key)
    {
        return js.InvokeAsync<object>("localStorage.getItem", key);
    }

    // Elimina un ítem del almacenamiento local
    public static ValueTask<object> RemoveLocalStorage(this IJSRuntime js, string key)
    {
        return js.InvokeAsync<object>("localStorage.removeItem", key);
    }
}