using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpBeltExamSajaAlhimiary.Models
{
  public class User
  {
      [Key]
      public int UserId { get; set; }

      [Required]
      [MinLength(2, ErrorMessage = "First name must be at least 2 characters!")]
      [Display(Name = "First Name: ")]
      public string FirstName { get; set; }

      [Required]
      [MinLength(2, ErrorMessage = "Last name must be at least 2 characters!")]
      [Display(Name = "Last Name: ")]
      public string LastName { get; set; }

      [Required]
      [Display(Name="Username: ")]
      [MinLength(3)]
      [MaxLength(15)]
      public string Username { get; set; }

      [Required]
      [DataType(DataType.Password)]
      [MinLength(8, ErrorMessage = "Password must be at least 8 characters!")]
      [Display(Name="Password: ")]
      public string Password { get; set; }

      public DateTime CreatedAt { get; set; } = DateTime.Now;
      public DateTime UpdatedAt { get; set; } = DateTime.Now;

      [NotMapped]
      [Compare("Password", ErrorMessage = "Passwords must match!")]
      [DataType(DataType.Password)]
      [Display(Name="Confirm Password: ")]
      public string Confirm { get; set; }

      public List<Hobby> AddedHobies {get;set;}
      public List<Enthusiast> Enthus {get;set;}
  }
}