﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Layer.Entities.Identity
{
    public class SeedUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public Address? Address { get; set; }
    }
}
