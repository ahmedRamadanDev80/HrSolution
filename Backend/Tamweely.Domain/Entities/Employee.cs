using System;
using Tamweely.Domain.Common;

namespace Tamweely.Domain.Entities;
public class Employee : BaseEntity,ISoftDelete
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int JobTitleId { get; set; }
    public JobTitle? JobTitle { get; set; }
    public bool IsActive { get; set; } = true;
    public string FullName => $"{FirstName} {LastName}";
}
