import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../models/auth.models';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
}

@Injectable({
  providedIn: 'root' 
})

export class AuthService {
    private http = inject(HttpClient)
    private router = inject(Router);

    private readonly TOKEN_KEY = 'innshop_token';

    private apiUrl = `${environment.userApiUrl}/users`;

    isAuth = signal<boolean>(!!localStorage.getItem(this.TOKEN_KEY));

    login(request: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          localStorage.setItem(this.TOKEN_KEY, response.token);
          this.isAuth.set(true); 
        })
      );
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    this.isAuth.set(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

register(request: RegisterRequest) {
  return this.http.post<string>(`${this.apiUrl}/register`, request, { 
      responseType: 'text' as 'json' 
  });
}

  forgotPassword(email: string) {
    return this.http.post(
      `${this.apiUrl}/forgot-password`, 
      { email }, 
      { responseType: 'text' } 
    );
  }

resetPassword(request: ResetPasswordRequest) {
    return this.http.post(
      `${this.apiUrl}/reset-password`, 
      request, 
      { responseType: 'text' } 
    );
  }
}



