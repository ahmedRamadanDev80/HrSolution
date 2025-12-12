namespace Tamweely.Application.DTOs;
public class CreateOrEditEmployeeDto 
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
    public int JobTitleId { get; set; } }