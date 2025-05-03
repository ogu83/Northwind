import { Injectable } from '@angular/core';
import { HttpClient }     from '@angular/common/http';
import { Observable }     from 'rxjs';
import { Category }       from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly baseUrl = 'http://localhost:5205/Category';

  constructor(private http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl);
  }

  getCategory(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.baseUrl}/${id}`);
  }

  saveCategory(cat: Category): Observable<Category> {
    return this.http.put<Category>(this.baseUrl, cat);
  }

  createCategory(cat: Category): Observable<Category> {
    return this.http.post<Category>(this.baseUrl, cat);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}?id=${id}`);
  }
}
