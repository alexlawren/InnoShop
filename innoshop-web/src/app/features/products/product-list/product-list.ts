import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';      
import { MatTooltipModule } from '@angular/material/tooltip'; 
import { MatFormFieldModule } from '@angular/material/form-field'; 
import { MatInputModule } from '@angular/material/input'; 
import { MatDialog } from '@angular/material/dialog'; 
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar'; 

import { ProductDialogComponent } from '../product-dialog/product-dialog';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    MatTableModule, 
    MatPaginatorModule, 
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,   
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule 
  ],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  totalCount = 0;
  pageSize = 10;
  pageIndex = 1;
  isLoading = true;
  
  searchText = '';
  minPrice: number | null = null;
  maxPrice: number | null = null;

  displayedColumns: string[] = ['name', 'price', 'description', 'actions'];

  private productService = inject(ProductService);
  private cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar); 
  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    setTimeout(() => {
      this.isLoading = true;
      this.cdr.detectChanges();
    }, 0);

    this.productService.getProducts(
      this.pageIndex, 
      this.pageSize, 
      this.searchText, 
      this.minPrice || undefined, 
      this.maxPrice || undefined
    ).subscribe({
      next: (result) => {
        this.products = result.items;
        this.totalCount = result.totalCount;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showError('Не удалось загрузить список товаров.');
        this.cdr.detectChanges();
      }
    });
  }

  onSearch() {
    this.pageIndex = 1; 
    this.loadProducts();
  }

  clearFilters() {
    this.searchText = '';
    this.minPrice = null;
    this.maxPrice = null;
    this.onSearch();
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadProducts();
  }


  openAddDialog() {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.createProduct(result).subscribe({
          next: () => {
            this.showSuccess('Товар успешно создан!');
            this.loadProducts(); 
          },
          error: (err) => this.showError('Ошибка при создании товара.', err)
        });
      }
    });
  }

  openEditDialog(product: Product) {
    const dialogRef = this.dialog.open(ProductDialogComponent, {
      width: '500px',
      data: product 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const updateCommand = { ...result, productId: product.id };

        this.productService.updateProduct(product.id, updateCommand).subscribe({
          next: () => {
            this.showSuccess('Товар успешно обновлен!');
            this.loadProducts();
          },
          error: (err) => this.showError('Ошибка при обновлении товара.', err)
        });
      }
    });
  }

  deleteProduct(product: Product) {
    if (confirm(`Вы уверены, что хотите удалить "${product.name}"?`)) {
      this.productService.deleteProduct(product.id).subscribe({
        next: () => {
          this.showSuccess('Товар удален.');
          this.loadProducts();
        },
        error: (err) => {
          if (err.status === 403) {
            this.showError('У вас нет прав удалять чужой товар!');
          } else {
            this.showError('Не удалось удалить товар.', err);
          }
        }
      });
    }
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
    console.error(err);
    this.snackBar.open(message, undefined, {
      duration: 4000,
      panelClass: ['error-snackbar'],
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
}