import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { ActivatedRoute } from '@angular/router'; 
import { of } from 'rxjs'; 

import { ResetPasswordComponent } from './reset-password'; 
import { provideHttpClient } from '@angular/common/http';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResetPasswordComponent, BrowserAnimationsModule],
      providers: [
        provideHttpClient(),
        {
          provide: ActivatedRoute,
          useValue: {
            queryParams: of({ email: 'test@test.com', token: '123' })
          }
        }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});