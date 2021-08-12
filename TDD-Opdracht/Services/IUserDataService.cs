using System;
using System.Collections.Generic;
using System.Text;
using TDD_Opdracht.Models;

namespace TDD_Opdracht.Services
{
    public interface IUserDataService
    {
        void Save(User user);
        bool Validate(User user);
    }
}
