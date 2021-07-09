using System.ComponentModel.DataAnnotations;
using System;

namespace CSharpBeltExamSajaAlhimiary.Models
{
    public class Enthusiast
    {
        [Key]
        public int EnthusiastId {get;set;}

        public int UserId {get;set;}
        public int HobbyId {get;set;}
        public User User {get;set;}
        public Hobby Hobby {get;set;}

    }
}