﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiKiedu.Model
{
    class User
    {
        //官方要求要加上virtual关键之
        public virtual int Id { get; set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public virtual DateTime Registerdate { get; set; }

    }
}
