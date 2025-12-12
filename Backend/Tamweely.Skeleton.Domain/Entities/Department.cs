using Tamweely.Domain.Common;

namespace Tamweely.Domain.Entities;
public class Department : BaseEntity, ISoftDelete 
{
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}