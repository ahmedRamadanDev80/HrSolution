import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { EmployeeListComponent } from './features/employees/employee-list.component';
import { EmployeeFormComponent } from './features/employees/employee-form.component';
import { DepartmentListComponent } from './features/department/department-list.component';
import { DepartmentFormComponent } from './features/department/department-form.component';
import { JobTitleListComponent } from './features/jobTitle/jobTitle-list.component';
import { JobTitleFormComponent } from './features/jobTitle/jobTitle-form.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    // Auth
    { path: 'login', component: LoginComponent },

    // Employees
    { path: 'employees', component: EmployeeListComponent, canActivate: [authGuard] },
    { path: 'employees/create', component: EmployeeFormComponent, canActivate: [authGuard] },
    { path: 'employees/edit/:id', component: EmployeeFormComponent, canActivate: [authGuard] },

    // Departments
    { path: 'departments', component: DepartmentListComponent, canActivate: [authGuard] },
    { path: 'departments/new', component: DepartmentFormComponent, canActivate: [authGuard] },
    { path: 'departments/edit/:id', component: DepartmentFormComponent, canActivate: [authGuard] },

    // Job Titles
    { path: 'jobtitles', component: JobTitleListComponent, canActivate: [authGuard] },
    { path: 'jobtitles/new', component: JobTitleFormComponent, canActivate: [authGuard] },
    { path: 'jobtitles/edit/:id', component: JobTitleFormComponent, canActivate: [authGuard] },

    // Default
    { path: '', redirectTo: 'employees', pathMatch: 'full' },
    { path: '**', redirectTo: 'employees' },
];
