﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
