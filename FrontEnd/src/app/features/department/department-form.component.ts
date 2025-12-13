import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DepartmentService } from '../../core/services/department.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DepartmentDto } from '../../shared/models/department.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-department-form',
    templateUrl: './department-form.component.html',
    standalone: true,
    imports: [CommonModule, FormsModule,ReactiveFormsModule,RouterModule],
})
export class DepartmentFormComponent implements OnInit {
    form: FormGroup;
    departmentId: number | null = null;

    constructor(
        private fb: FormBuilder,
        private departmentService: DepartmentService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.form = this.fb.group({
            name: ['', Validators.required],
        });
    }

    ngOnInit(): void {
        this.departmentId = +this.route.snapshot.paramMap.get('id')!;
        if (this.departmentId) {
            this.departmentService.getDepartments().subscribe((deps) => {
                const dep = deps.find((d) => d.id === this.departmentId);
                if (dep) this.form.patchValue({ name: dep.name });
            });
        }
    }

    save() {
        const dto: DepartmentDto = this.form.value;
        if (this.departmentId) {
            this.departmentService.updateDepartment(this.departmentId, dto).subscribe(() => this.router.navigate(['/departments']));
        } else {
            this.departmentService.createDepartment(dto).subscribe(() => this.router.navigate(['/departments']));
        }
    }
}
