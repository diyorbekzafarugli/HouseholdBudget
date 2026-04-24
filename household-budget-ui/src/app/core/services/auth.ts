
import { Injectable, signal, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';
import { ApiResponse } from '../models/api.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = 'https://localhost:7099/api/auth';
  private readonly TOKEN_KEY = 'access_token';
  private readonly REFRESH_KEY = 'refresh_token';
  private readonly USER_KEY = 'user';

  private platformId = inject(PLATFORM_ID);
  private http = inject(HttpClient);
  private router = inject(Router);

  private get isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  currentUser = signal<AuthResponse | null>(this.getStoredUser());

  register(request: RegisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/register`, request).pipe(
      tap(res => { if (res.success && res.data) this.storeTokens(res.data); })
    );
  }

  login(request: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/login`, request).pipe(
      tap(res => { if (res.success && res.data) this.storeTokens(res.data); })
    );
  }

  refreshToken(): Observable<ApiResponse<AuthResponse>> {
    const refreshToken = this.isBrowser ? localStorage.getItem(this.REFRESH_KEY) : null;
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/refresh-token`, { token: refreshToken }).pipe(
      tap(res => { if (res.success && res.data) this.storeTokens(res.data); })
    );
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.REFRESH_KEY);
      localStorage.removeItem(this.USER_KEY);
    }
    this.currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return this.isBrowser ? localStorage.getItem(this.TOKEN_KEY) : null;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private storeTokens(data: AuthResponse): void {
    if (this.isBrowser) {
      localStorage.setItem(this.TOKEN_KEY, data.accessToken);
      localStorage.setItem(this.REFRESH_KEY, data.refreshToken);
      localStorage.setItem(this.USER_KEY, JSON.stringify(data));
    }
    this.currentUser.set(data);
  }

  private getStoredUser(): AuthResponse | null {
    if (!this.isBrowser) return null;
    const user = localStorage.getItem(this.USER_KEY);
    return user ? JSON.parse(user) : null;
  }
}