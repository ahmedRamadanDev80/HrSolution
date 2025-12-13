import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { JobTitle, JobTitleDto } from '../../shared/models/jobTitle.model';

@Injectable({ providedIn: 'root' })
export class JobTitlesService {
    private api = inject(ApiService);

    // GET /api/JobTitles?includeInactive=true/false
    getJobTitles(includeInactive = false): Observable<JobTitle[]> {
        return this.api.get<JobTitle[]>('/api/Jobs', { includeInactive });
    }

    // GET /api/JobTitles/{id} (optional)
    getJobTitle(id: number): Observable<JobTitle> {
        return this.api.get<JobTitle>(`/api/Jobs/${id}`);
    }

    // POST /api/JobTitles
    createJobTitle(dto: JobTitleDto): Observable<void> {
        return this.api.post<void>('/api/Jobs', dto);
    }

    // PUT /api/JobTitles/{id}
    updateJobTitle(id: number, dto: JobTitleDto): Observable<void> {
        return this.api.put<void>(`/api/Jobs/${id}`, dto);
    }

    // DELETE /api/JobTitles/{id}
    deleteJobTitle(id: number): Observable<void> {
        return this.api.delete<void>(`/api/Jobs/${id}`);
    }
}
