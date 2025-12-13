import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Department, DepartmentDto } from '../../shared/models/department.model';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
    private api = inject(ApiService);

    // GET /api/Departments?includeInactive=true/false
    getDepartments(includeInactive = false): Observable<Department[]> {
        return this.api.get<Department[]>('/api/Departments', { includeInactive });
    }

    // GET /api/Departments/{id} (optional, if needed)
    getDepartment(id: number): Observable<Department> {
        return this.api.get<Department>(`/api/Departments/${id}`);
    }

    // POST /api/Departments
    createDepartment(dto: DepartmentDto): Observable<void> {
        return this.api.post<void>('/api/Departments', dto);
    }

    // PUT /api/Departments/{id}
    updateDepartment(id: number, dto: DepartmentDto): Observable<void> {
        return this.api.put<void>(`/api/Departments/${id}`, dto);
    }

    // DELETE /api/Departments/{id}
    deleteDepartment(id: number): Observable<void> {
        return this.api.delete<void>(`/api/Departments/${id}`);
    }
}
