import { Component, Inject, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule, 
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatCheckboxModule
  ],
  templateUrl: './product-dialog.html',
  styleUrl: './product-dialog.scss'
})
export class ProductDialogComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;

  private fb = inject(FormBuilder);
  
constructor(
    public dialogRef: MatDialogRef<ProductDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Product | null,
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      
      price: [0, [
        Validators.required, 
        Validators.min(0.01),
        Validators.pattern(/^\d+(\.\d{1,2})?$/) 
      ]],
      
      description: ['', [Validators.required]],
      
      isAvailable: [true]
    });
  }
  ngOnInit(): void {
    if (this.data) {
      this.isEditMode = true;
      this.form.patchValue(this.data);
    }
  }

  onCancel(): void {
    this.dialogRef.close(null); 
  }

  onSave(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }
}