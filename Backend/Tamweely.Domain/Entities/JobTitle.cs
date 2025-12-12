using Tamweely.Domain.Common;
namespace Tamweely.Domain.Entities;
public class JobTitle : BaseEntity,ISoftDelete 
{
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}