using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TDD_Opdracht.Models;

namespace TDD_Opdracht.DbContext
{

        public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
                : base(options)
            {

            }

            public virtual Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }

        }
    }

