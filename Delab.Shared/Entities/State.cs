using System.ComponentModel.DataAnnotations;

namespace Delab.Shared.Entities;

public class State
{
    [Key]
    public int StateId { get; set; }

    // Id para la relacion con el pais/country
    public int CountryId { get; set; }

    [MaxLength(100, ErrorMessage = "El campo {0} no puede ser mayor de {1} caracteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Display(Name = "Depart/Estado")]
    public string Name { get; set; } = null!;

    // Relacion con el pais/country
    public Country? Country { get; set; }

    // Relacion con la ciudad/city
    public ICollection<City>? Cities { get; set; }
}