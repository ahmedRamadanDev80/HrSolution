export interface EmployeeQueryParams {
    search?: string;        // search by name, email, or mobile
    departmentId?: number;
    jobTitleId?: number;
    dateFrom?: string;
    dateTo?: string;
    page?: number;
    pageSize?: number;
    sortField?: string;
    sortOrder?: 'asc' | 'desc';
}