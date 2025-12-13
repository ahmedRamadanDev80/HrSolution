import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_URL } from '../../shared/utils/constants';

export interface LoginResponse {
    token: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {

    constructor(private http: HttpClient) { }

    private readonly apiUrl = `${API_URL}/api/Auth`;

    private tokenSubject = new BehaviorSubject<string | null>(
        localStorage.getItem('token')
    );

    token$ = this.tokenSubject.asObservable();


    login(username: string, password: string): Observable<LoginResponse> {
        return this.http
            .post<LoginResponse>(`${this.apiUrl}/Login`, { username, password })
            .pipe(
                tap(res => {
                    localStorage.setItem('token', res.token);
                    this.tokenSubject.next(res.token);
                })
            );
    }

    logout(): void {
        localStorage.removeItem('token');
        this.tokenSubject.next(null);
    }

    get token(): string | null {
        return this.tokenSubject.value;
    }

    isAuthenticated(): boolean {
        return !!this.token;
    }
}
