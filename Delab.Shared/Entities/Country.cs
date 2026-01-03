using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    // Con el display el nombre del campo toma  la posicion 0 y el 1 en el primer dato de atributo
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Display(Name = "Pais")]
    public string Name { get; set; } = null!;

    [MaxLength(10, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Display(Name = "Cod Phone")]
    public string? CodPhone { get; set; }

    // Relacion con el Estado o departamento/State
    public ICollection<State>? States { get; set; }

    // Relacion con la Corporacion/Corporation
    public ICollection<Corporation>? Corporations { get; set; }
}