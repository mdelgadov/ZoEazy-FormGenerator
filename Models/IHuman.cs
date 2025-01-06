using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

public interface IHuman
{
    [Required] string? FirstName { get; set; }

    string? MiddleName { get; set; }

    [Required] string? LastName { get; set; }

    [EmailAddress] string? Email { get; set; }

    Gender Gender { get; set; }

}