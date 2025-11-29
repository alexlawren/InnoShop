import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar'; // <--- 1. ИМПОРТ

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule,
    MatChipsModule,
    MatTooltipModule,
    MatSnackBarModule 
  ],
  templateUrl: './user-list.html',
  styleUrl: './user-list.scss'
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  totalCount = 0;
  pageSize = 10;
  pageIndex = 1;
  isLoading = false;

  displayedColumns: string[] = ['name', 'email', 'role', 'status', 'actions'];

  private userService = inject(UserService);
  private cdr = inject(ChangeDetectorRef);
  private snackBar = inject(MatSnackBar); 

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    setTimeout(() => {
        this.isLoading = true;
        this.cdr.detectChanges();
    }, 0);

    this.userService.getAllUsers(this.pageIndex, this.pageSize).subscribe({
      next: (result) => {
        this.users = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showError('Не удалось загрузить список пользователей');
        this.cdr.detectChanges();
      }
    });
  }

  toggleStatus(user: User) {
    const oldStatus = user.isActive;
    const newStatus = !oldStatus;

    user.isActive = newStatus;
    
    const statusText = newStatus ? 'разблокирован' : 'заблокирован';

    this.userService.updateStatus(user.id, newStatus).subscribe({
      next: () => {
        this.showSuccess(`Пользователь ${user.name} ${statusText}.`);
      },
      error: (err) => {
        console.error('Ошибка обновления статуса', err);
        user.isActive = oldStatus;
        this.cdr.detectChanges();
        this.showError(`Не удалось изменить статус пользователя.`);
      }
    });
  }

  deleteUser(user: User) {
    if (confirm(`Вы уверены, что хотите удалить пользователя "${user.name}"? Это действие необратимо.`)) {
      this.userService.deleteUser(user.id).subscribe({
        next: () => {
          this.showSuccess('Пользователь успешно удален.');
          this.loadUsers(); 
        },
        error: (err) => {
          this.showError('Ошибка при удалении пользователя.', err);
        }
      });
    }
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadUsers();
  }
  
  private showSuccess(message: string) {
    this.snackBar.open(message, undefined, {
      duration: 3000,
      panelClass: ['success-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  private showError(message: string, err?: any) {
    if (err) console.error(err);
    this.snackBar.open(message, undefined, {
      duration: 4000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
}