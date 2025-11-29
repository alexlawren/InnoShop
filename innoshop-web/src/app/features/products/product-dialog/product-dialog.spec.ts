import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductDialogComponent } from './product-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ProductDialogComponent', () => {
  let component: ProductDialogComponent;
  let fixture: ComponentFixture<ProductDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDialogComponent, BrowserAnimationsModule],
      providers: [
        { provide: MatDialogRef, useValue: { close: () => {} } }, 
        { provide: MAT_DIALOG_DATA, useValue: {} } 
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});