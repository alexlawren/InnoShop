import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms';   
import { Router } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon'; 
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router'; 
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordDialog } from '../forgot-password-dialog/forgot-password-dialog';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  email = '';
  password = '';
  
  isLoading = false;
  errorMessage = '';
  hidePassword = true; 

  private authService = inject(AuthService);
  private router = inject(Router);
  private dialog = inject(MatDialog); 
  private cdr = inject(ChangeDetectorRef);

  onSubmit() {
    if (!this.email || !this.password) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/products']); 
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);
        this.errorMessage = 'Неверный email или пароль, либо сервер недоступен.';
        this.cdr.detectChanges(); 
      }
    });
  }

    openForgotPassword() {
    this.dialog.open(ForgotPasswordDialog, {
      width: '400px'
    });
  }
}