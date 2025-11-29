import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login'; 
import { ProductListComponent } from './features/products/product-list/product-list';
import { UserListComponent } from './features/admin/user-list/user-list';
import { RegisterComponent } from './features/auth/register/register';
import { ResetPasswordComponent } from './features/auth/reset-password/reset-password';

import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { 
    path: 'login',
    component: LoginComponent,
    canActivate: [guestGuard] 
  },

  { 
    path: 'register', 
    component: RegisterComponent, 
    canActivate: [guestGuard] 
  },

  { 
    path: 'reset-password', 
    component: ResetPasswordComponent, 
    canActivate: [guestGuard]
  },
  
  { 
    path: 'products', 
    component: ProductListComponent,
    canActivate: [authGuard]
  },

  { 
    path: 'users', 
    component: UserListComponent,
    canActivate: [authGuard] 
  }
];