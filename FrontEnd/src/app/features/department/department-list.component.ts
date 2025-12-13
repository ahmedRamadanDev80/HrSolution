import { Component, signal, computed } from '@angular/core';
import { DepartmentService } from '../../core/services/department.service';
import { Department } from '../../shared/models/department.model';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-department-list',
    templateUrl: './department-list.component.html',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
})
export class DepartmentListComponent {
    private service = new DepartmentService(); // or inject if using Angular DI

    departments = signal<Department[]>([]);
    total = signal(0);
    page = signal(1);
    pageSize = signal(10);
    pageSizes = [10, 50, 500];

    // computed total pages
    totalPages = computed(() => Math.ceil(this.total() / this.pageSize()));

    pages = computed(() => Array.from({ length: this.totalPages() }, (_, i) => i + 1));

    constructor(private departmentService: DepartmentService) {
        this.loadDepartments();
    }

    loadDepartments() {
        this.departmentService.getDepartments().subscribe((data) => {
            this.departments.set(data);
            this.total.set(data.length);
        });
    }

    pageChanged(newPage: number) {
        if (newPage < 1 || newPage > this.totalPages()) return;
        this.page.set(newPage);
    }

    pageSizeChanged(size: number) {
        this.pageSize.set(+size);
        this.page.set(1);
    }

    deleteDepartment(id: number) {
        if (!confirm('Are you sure you want to delete this department?')) return;
        this.departmentService.deleteDepartment(id).subscribe(() => this.loadDepartments());
    }
}
