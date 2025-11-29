import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/paged-result.model';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.userApiUrl}/users`;

  getAllUsers(pageIndex: number, pageSize: number) {
    let params = new HttpParams()
      .set('pageNumber', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<User>>(this.apiUrl, { params });
  }

  updateStatus(userId: string, isActive: boolean) {
    return this.http.patch(`${this.apiUrl}/${userId}/status`, { userId, isActive });
  }

  deleteUser(userId: string) {
    return this.http.delete(`${this.apiUrl}/${userId}`);
  }
  
  changeRole(userId: string, newRole: string) {
    return this.http.patch(`${this.apiUrl}/${userId}/role`, { userId, newRole });
  }
}