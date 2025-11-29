import { Component, inject, ChangeDetectorRef, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './register.html', 
  styleUrl: './register.scss',
  encapsulation: ViewEncapsulation.None 
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  isLoading = false;
  hidePassword = true;

  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar); 
  private cdr = inject(ChangeDetectorRef);

  onRegister(form: any) {
    if (form.invalid) return;

    setTimeout(() => {
        this.isLoading = true;
        this.cdr.detectChanges();
    }, 0);

    this.authService.register({
      name: this.name,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.isLoading = false;
        
        this.snackBar.open('Регистрация успешна! Перейдите на почту и следуйте инструкции.', undefined, {
          duration: 5000, 
          panelClass: ['success-snackbar'],
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });

        this.router.navigate(['/login']);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);

        let message = 'Ошибка регистрации';
        if (err.status === 409) {
          message = 'Этот Email уже занят!';
        } else if (err.error?.errors) {
          message = Object.values(err.error.errors).join(', ');
        } else if (err.error?.detail) {
          message = err.error.detail;
        }

        this.snackBar.open(message, undefined, {
          duration: 5000,
          panelClass: ['error-snackbar'],
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
        
        this.cdr.detectChanges();
      }
    });
  }
}