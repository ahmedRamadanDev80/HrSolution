import { Employee } from "./employee.model";

export interface EmployeePagedResult {
    items: Employee[];
    total: number;
    page: number;
    pageSize: number;
}