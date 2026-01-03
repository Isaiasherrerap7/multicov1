using MudBlazor;

namespace Delab.Frontend.Pages.Isla
{
    public partial class IndexIsla
    {
        // Datos ficticios usando objetos anónimos
        private readonly List<dynamic> Summaries = new()
    {
        new
        {
            Title = "Total de Expedientes",
            Value = "3",
            Subtitle = "3 este mes",
            Icon = Icons.Material.Filled.Description,
            IconColor = Color.Info
        },
        new
        {
            Title = "En Proceso",
            Value = "3",
            Subtitle = "Expedientes activos",
            Icon = Icons.Material.Filled.Schedule,
            IconColor = Color.Warning
        },
        new
        {
            Title = "Completados",
            Value = "0",
            Subtitle = "Finalizados exitosamente",
            Icon = Icons.Material.Filled.CheckCircle,
            IconColor = Color.Success
        },
        new
        {
            Title = "Usuarios Activos",
            Value = "3",
            Subtitle = "1 analistas, 0 investigadores",
            Icon = Icons.Material.Filled.Group,
            IconColor = Color.Dark
        }
    };

        private readonly List<dynamic> Activities = new()
    {
        new { Title = "Expediente 2025-0003 - ASIGNADO", When = "Hace 1 día" },
        new { Title = "Expediente 2025-0002 - ASIGNADO", When = "Hace 2 días" },
        new { Title = "Expediente 2025-0001 - ASIGNADO", When = "Hace 2 días" }
    };

        private readonly List<dynamic> Alerts = new()
    {
        new
        {
            Title = "Expedientes Urgentes",
            Description = "Requieren atención inmediata",
            Count = 1,
            Severity = Severity.Error
        },
        new
        {
            Title = "En Proceso",
            Description = "Expedientes siendo trabajados",
            Count = 3,
            Severity = Severity.Info
        },
        new
        {
            Title = "Usuarios Activos",
            Description = "Usuarios conectados hoy",
            Count = 3,
            Severity = Severity.Success
        }
    };

        // Helper method for icon background with theme awareness
        private string IconStyle() =>
            $"width:48px;height:48px;border-radius:8px;background:transparent;";

        // Helper method to get chip color based on severity
        private Color GetChipColor(Severity severity) => severity switch
        {
            Severity.Error => Color.Error,
            Severity.Warning => Color.Warning,
            Severity.Info => Color.Info,
            Severity.Success => Color.Success,
            _ => Color.Default
        };
    }
}