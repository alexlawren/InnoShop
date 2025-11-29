import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router'; 
import { AuthService } from '../../../core/services/auth.service';

import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-reset-password',
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
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.scss'
})
export class ResetPasswordComponent implements OnInit {
  newPassword = '';
  confirmPassword = ''; 
  email = '';
  token = '';
  
  isLoading = false;
  hidePassword = true;

  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar); 

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });
  }

  onSubmit() {
    if (this.newPassword !== this.confirmPassword) {
      this.showError('Пароли не совпадают!');
      return;
    }

    if (!this.token || !this.email) {
      this.showError('Некорректная ссылка восстановления. Запросите сброс заново.');
      return;
    }

    this.isLoading = true;

    this.authService.resetPassword({
      email: this.email,
      token: this.token,
      newPassword: this.newPassword
    }).subscribe({
      next: () => {
        this.isLoading = false;
        
        this.snackBar.open('Пароль успешно изменен! Теперь вы можете войти.', undefined, {
          duration: 5000,
          panelClass: ['success-snackbar'],
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });

        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);

        const message = err.error?.detail || 'Ошибка сброса. Возможно, ссылка устарела.';
        this.showError(message);
      }
    });
  }
  
  private showError(message: string) {
    this.snackBar.open(message, undefined, {
      duration: 5000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
}