using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamweely.Domain.Entities;

namespace Tamweely.Application.Interfaces;
public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user, IEnumerable<string> roles);
}
