using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamweely.Application.DTOs;

namespace Tamweely.Application.Interfaces;
public interface IJobRepository
{
    Task<IReadOnlyList<JobTitleDto>> GetAllAsync(bool includeInactive = false);
    Task<int> CreateAsync(JobTitleDto dto);
    Task UpdateAsync(int id, JobTitleDto dto);
    Task SoftDeleteAsync(int id);

}
