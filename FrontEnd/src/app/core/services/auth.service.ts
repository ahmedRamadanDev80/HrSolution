import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { API_URL } from '../../shared/utils/constants';

export interface LoginResponse {
    token: string;
}

export interface JwtPayload {
    email: string;
    sub: string;
    name: string;
    role?: string | string[];
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly apiUrl = `${API_URL}/api/Auth`;

    // token signal
    private _token = signal<string | null>(localStorage.getItem('token'));

    // public computed signals
    token = computed(() => this._token());
    isAuthenticated = computed(() => !!this._token());
    roles = computed(() => {
        if (!this._token()) return [];
        const decoded = jwtDecode<JwtPayload>(this._token()!);
        if (!decoded.role) return [];
        return Array.isArray(decoded.role) ? decoded.role : [decoded.role];
    });

    constructor(private http: HttpClient) { }

    login(username: string, password: string): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.apiUrl}/Login`, { username, password })
            .pipe(
                tap(res => {
                    localStorage.setItem('token', res.token);
                    this._token.set(res.token);
                })
            );
    }

    logout(): void {
        localStorage.removeItem('token');
        this._token.set(null);
    }

    hasRole(role: string): boolean {
        return this.roles().includes(role);
    }
}
