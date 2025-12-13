import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../../shared/utils/constants';

@Injectable({ providedIn: 'root' })
export class ApiService {
    private http = inject(HttpClient);

    get<T>(endpoint: string, params?: any): Observable<T> {
        return this.http.get<T>(`${API_URL}${endpoint}`, { params });
    }

    post<T>(endpoint: string, body: any): Observable<T> {
        return this.http.post<T>(`${API_URL}${endpoint}`, body);
    }

    put<T>(endpoint: string, body: any): Observable<T> {
        return this.http.put<T>(`${API_URL}${endpoint}`, body);
    }

    delete<T>(endpoint: string): Observable<T> {
        return this.http.delete<T>(`${API_URL}${endpoint}`);
    }
}
