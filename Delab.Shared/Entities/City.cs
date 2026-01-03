using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class City
{
    [Key]
    public int CityId { get; set; }

    // Id para la relacion con el Estado o departamento/State
    public int StateId { get; set; }

    [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Ciudad")]
    public string Name { get; set; } = null!;

    // Relacion con el Estado o departamento/State
    public State? State { get; set; }
}