import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EmployeeService } from '../../core/services/employee.service';
import { CreateOrEditEmployee } from '../../shared/models/createOrEditEmployee.model';
import { Department } from '../../shared/models/department.model';
import { JobTitle } from '../../shared/models/jobTitle.model';
import { DepartmentService } from '../../core/services/department.service';
import { JobTitlesService } from '../../core/services/jobTitle.service';

@Component({
    selector: 'app-employee-form',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './employee-form.component.html',
})
export class EmployeeFormComponent implements OnInit {
    employeeService = inject(EmployeeService);
    departmentService = inject(DepartmentService);
    jobTitlesService = inject(JobTitlesService);

    route = inject(ActivatedRoute);
    router = inject(Router);

    id?: number;

    employee: CreateOrEditEmployee = {
        firstName: '',
        lastName: '',
        email: '',
        mobile: '',
        dateOfBirth: '',
        departmentId: undefined,
        jobTitleId: undefined
    };
    departments = signal<Department[]>([]);
    jobTitles = signal<JobTitle[]>([]);

    error = signal<string>('');

    ngOnInit() {
        this.loadLookups();
        this.id = Number(this.route.snapshot.paramMap.get('id'));
        if (this.id) {
            this.employeeService.getEmployee(this.id).subscribe({
                next: (res) => {
                    this.employee = {
                        firstName: res.fullName.split(' ')[0] || '',
                        lastName: res.fullName.split(' ')[1] || '',
                        email: res.email,
                        mobile: res.mobile,
                        dateOfBirth: res.dateOfBirth ? res.dateOfBirth.split('T')[0] : '',
                        departmentId: res.departmentId,
                        jobTitleId: res.jobTitleId
                    };
                }
            });

        }

    }

    save(form: NgForm) {
    if (form.invalid) {
        this.error.set('Please fill all required fields.');
        return;
    }

    const payload = this.employee;

    const handleError = (err: any) => {
        if (err.status === 400 && err.error?.errors) {
            // Check for email error
            if (err.error.errors.email && err.error.errors.email.length > 0) {
                this.error.set(err.error.errors.email[0]);
                return;
            }
            // Fallback: show the first validation error
            const firstKey = Object.keys(err.error.errors)[0];
            this.error.set(err.error.errors[firstKey][0]);
        } else {
            this.error.set('Unexpected server error.');
        }
    };

    if (this.id) {
        this.employeeService.updateEmployee(this.id, payload).subscribe({
            next: () => this.router.navigate(['/employees']),
            error: handleError
        });
    } else {
        this.employeeService.createEmployee(payload).subscribe({
            next: () => this.router.navigate(['/employees']),
            error: handleError
        });
    }
}


    private loadLookups() {
        this.departmentService.getDepartments(false).subscribe(d =>
            this.departments.set(d)
        );

        this.jobTitlesService.getJobTitles(false).subscribe(j =>
            this.jobTitles.set(j)
        );
    }
}
