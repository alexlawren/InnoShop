import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../core/services/auth.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-forgot-password-dialog',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatDialogModule, 
    MatButtonModule, 
    MatInputModule, 
    MatFormFieldModule,
    MatSnackBarModule 
  ],
  templateUrl: './forgot-password-dialog.html'
})
export class ForgotPasswordDialog {
  email = '';
  isLoading = false;
  
  private authService = inject(AuthService);
  private dialogRef = inject(MatDialogRef<ForgotPasswordDialog>);
  private snackBar = inject(MatSnackBar); 

  send() {
    if (!this.email) return;
    this.isLoading = true;

    this.authService.forgotPassword(this.email).subscribe({
      next: () => {
        this.isLoading = false;
        
        this.snackBar.open('Инструкция отправлена на почту.', 'ОК', {
          duration: 5000,
          panelClass: ['success-snackbar']
        });

        this.dialogRef.close();
      },
      error: (err) => {
        this.isLoading = false;
        
        this.snackBar.open('Ошибка. Возможно, email не найден.', 'Закрыть', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }
}