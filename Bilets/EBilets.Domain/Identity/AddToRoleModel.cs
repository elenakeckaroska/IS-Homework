﻿using System.Collections.Generic;

namespace EBilets.Domain.Identity
{
    public class AddToRoleModel
    {

        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string SelectedRole { get; set; }

    }
}
