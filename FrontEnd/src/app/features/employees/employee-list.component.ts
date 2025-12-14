import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EmployeeService } from '../../core/services/employee.service';
import { Employee } from '../../shared/models/employee.model';
import { Department } from '../../shared/models/department.model';
import { JobTitle } from '../../shared/models/jobTitle.model';
import { DepartmentService } from '../../core/services/department.service';
import { JobTitlesService } from '../../core/services/jobTitle.service';

import { EmployeeQueryParams } from '../../shared/models/employeeQueryParams.model';
import { EmployeePagedResult } from '../../shared/models/employeePagedResult.model';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-employee-list',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './employee-list.component.html',
})
export class EmployeeListComponent implements OnInit {

    // Reactive state
    employees = signal<Employee[]>([]);
    departments = signal<Department[]>([]);
    jobTitles = signal<JobTitle[]>([]);

    total = signal(0);
    page = signal(1);
    pageSize = signal(10);
    pageSizes = [10, 50, 500];
    search = signal('');

    // Filters (plain properties for ngModel)
    departmentId: number | null = null;
    jobTitleId: number | null = null;
    DobFrom: string | null = null;
    DobTo: string | null = null;

    // Toggle for advanced filters
    showFilters = signal(false);

    constructor(private employeeService: EmployeeService, private departmentService: DepartmentService,
        private jobTitlesService: JobTitlesService, private http: HttpClient) { }

    ngOnInit() {
        this.loadEmployees();
        this.loadLookups();
    }

    toggleFilters() {
        this.showFilters.set(!this.showFilters());
    }

    private buildParams(): EmployeeQueryParams {
        const params: any = {
            page: this.page(),
            pageSize: this.pageSize(),
            search: this.search(),
        };

        if (this.departmentId != null) params.departmentId = this.departmentId;
        if (this.jobTitleId != null) params.jobTitleId = this.jobTitleId;
        if (this.DobFrom) params.DobFrom = this.DobFrom;
        if (this.DobTo) params.DobTo = this.DobTo;

        return params;
    }

    loadEmployees() {
        this.employeeService.getEmployees(this.buildParams())
            .subscribe((res: EmployeePagedResult) => {
                //console.log(res.items);
                this.employees.set(res.items);
                this.total.set(res.total);
                this.page.set(res.page);
                this.pageSize.set(res.pageSize);
            });
    }

    private loadLookups() {
        this.departmentService.getDepartments(false).subscribe(d =>
            this.departments.set(d)
        );

        this.jobTitlesService.getJobTitles(false).subscribe(j =>
            this.jobTitles.set(j)
        );
    }

    deleteEmployee(id: number) {
        if (!confirm('Are you sure to delete this employee?')) return;
        this.employeeService.deleteEmployee(id).subscribe(() => this.loadEmployees());
    }

    exportEmployees() {
        // Reuse the current filters
        const params = this.buildParams();

        this.http.get('https://localhost:7005/api/Employees/export', {
            params: params as any,  // HttpClient expects string values
            responseType: 'blob'
        }).subscribe({
            next: (blob: Blob) => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = 'Employees.xlsx';
                a.click();
                window.URL.revokeObjectURL(url);
            },
            error: err => {
                console.error(err);
            }
        });
    }


    // Compute total pages
    totalPages = computed(() => Math.ceil(this.total() / this.pageSize()));

    // Generate array of pages
    pages = computed(() =>
        Array.from({ length: this.totalPages() }, (_, i) => i + 1)
    );

    pageChanged(newPage: number) {
        if (newPage < 1 || newPage > this.totalPages()) return;
        this.page.set(newPage);
        this.loadEmployees();
    }

    pageSizeChanged(newSize: number) {
        this.pageSize.set(+newSize);
        this.page.set(1); // reset to first page
        this.loadEmployees();
    }

}
