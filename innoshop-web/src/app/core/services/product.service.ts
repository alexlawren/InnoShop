import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'; 
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/paged-result.model'; 
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.productApiUrl}/products`;

  getProducts(pageIndex: number, pageSize: number, searchText?: string, minPrice?: number, maxPrice?: number) {
    let params = new HttpParams()
      .set('pageNumber', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    if (searchText) params = params.set('searchText', searchText);
    if (minPrice) params = params.set('minPrice', minPrice.toString());
    if (maxPrice) params = params.set('maxPrice', maxPrice.toString());

    return this.http.get<PagedResult<Product>>(`${this.apiUrl}/search`, { params });
  }

  searchProducts(query: string, pageIndex: number, pageSize: number) {
    let params = new HttpParams()
      .set('searchText', query)
      .set('pageNumber', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResult<Product>>(`${this.apiUrl}/search`, { params });
  }

  createProduct(product: Partial<Product>) {
    return this.http.post<string>(this.apiUrl, product);
  }

  updateProduct(id: string, product: Partial<Product>) {
    return this.http.put(`${this.apiUrl}/${id}`, product);
  }

  deleteProduct(id: string) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}