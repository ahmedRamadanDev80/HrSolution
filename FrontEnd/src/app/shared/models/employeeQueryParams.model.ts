export interface EmployeeQueryParams {
    search?: string;        // search by name, email, or mobile
    departmentId?: number;
    jobTitleId?: number;
    DobFrom?: string;
    DobTo?: string;
    page?: number;
    pageSize?: number;
    sortField?: string;
    sortOrder?: 'asc' | 'desc';
}