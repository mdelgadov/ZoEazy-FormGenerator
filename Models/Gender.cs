using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

public enum Gender
{
    [Display(Name = "Undefined", ResourceType = typeof(ResourceForm))]
    undefined,

    [Display(Name = "Male", ResourceType = typeof(ResourceForm))]
    male,

    [Display(Name = "Female", ResourceType = typeof(ResourceForm))]
    female
}