export interface CreateOrEditEmployee {
    id?: number;  
    firstName: string;
    lastName: string;
    email: string;
    mobile: string;
    dateOfBirth?: string;
    departmentId?: number;
    jobTitleId?: number;
}