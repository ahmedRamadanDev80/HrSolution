import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { JobTitlesService } from '../../core/services/jobTitle.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { JobTitleDto } from '../../shared/models/jobTitle.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-jobtitle-form',
    templateUrl: './jobtitle-form.component.html',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
})
export class JobTitleFormComponent implements OnInit {
    form: FormGroup;
    jobTitleId: number | null = null;

    constructor(
        private fb: FormBuilder,
        private jobTitleService: JobTitlesService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        this.form = this.fb.group({
            name: ['', Validators.required],
        });
    }

    ngOnInit(): void {
        this.jobTitleId = +this.route.snapshot.paramMap.get('id')!;
        if (this.jobTitleId) {
            this.jobTitleService.getJobTitles().subscribe((jobs) => {
                const job = jobs.find((j) => j.id === this.jobTitleId);
                if (job) this.form.patchValue({ name: job.name });
            });
        }
    }

    save() {
        const dto: JobTitleDto = this.form.value;
        if (this.jobTitleId) {
            this.jobTitleService.updateJobTitle(this.jobTitleId, dto)
                .subscribe(() => this.router.navigate(['/jobtitles']));
        } else {
            this.jobTitleService.createJobTitle(dto)
                .subscribe(() => this.router.navigate(['/jobtitles']));
        }
    }
}
