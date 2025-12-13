import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';
import { Employee } from '../../shared/models/employee.model';
import { EmployeeQueryParams } from '../../shared/models/employeeQueryParams.model';
import { EmployeePagedResult } from '../../shared/models/employeePagedResult.model';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import * as XLSX from 'xlsx';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
    private api = inject(ApiService);

    // GET /api/Employees?queryParams
    getEmployees(queryParams?: EmployeeQueryParams): Observable<EmployeePagedResult> {
        return this.api.get<EmployeePagedResult>('/api/Employees', queryParams);
    }

    // GET /api/Employees/{id}
    getEmployee(id: number): Observable<Employee> {
        return this.api.get<Employee>(`/api/Employees/${id}`);
    }

    // POST /api/Employees
    createEmployee(dto: any): Observable<void> {
        return this.api.post<void>('/api/Employees', dto);
    }

    // PUT /api/Employees/{id}
    updateEmployee(id: number, dto: any): Observable<void> {
        return this.api.put<void>(`/api/Employees/${id}`, dto);
    }

    // DELETE /api/Employees/{id}
    deleteEmployee(id: number): Observable<void> {
        return this.api.delete<void>(`/api/Employees/${id}`);
    }

}