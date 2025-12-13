import { Component, signal, computed, inject } from '@angular/core';
import { JobTitlesService } from '../../core/services/jobTitle.service';
import { JobTitle } from '../../shared/models/jobTitle.model';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-jobtitle-list',
    templateUrl: './jobtitle-list.component.html',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
})
export class JobTitleListComponent {
    private service = inject(JobTitlesService);

    jobTitles = signal<JobTitle[]>([]);
    total = signal(0);
    page = signal(1);
    pageSize = signal(10);
    pageSizes = [10, 50, 500];

    totalPages = computed(() => Math.ceil(this.total() / this.pageSize()));
    pages = computed(() => Array.from({ length: this.totalPages() }, (_, i) => i + 1));

    constructor() {
        this.loadJobTitles();
    }

    loadJobTitles() {
        this.service.getJobTitles().subscribe((data) => {
            this.jobTitles.set(data);
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

    deleteJobTitle(id: number) {
        if (!confirm('Are you sure you want to delete this job title?')) return;
        this.service.deleteJobTitle(id).subscribe(() => this.loadJobTitles());
    }
}
