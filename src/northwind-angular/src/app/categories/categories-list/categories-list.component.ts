import { Component }        from '@angular/core';
import { Router, RouterModule }     from '@angular/router';
import { CommonModule }     from '@angular/common';
import { MatTableModule }   from '@angular/material/table';
import { CategoryService }  from '../../services/category.service';
import { Category } from '../../models/category.model';

@Component({
  standalone: true,
  selector: 'app-categories-list',
  templateUrl: './categories-list.component.html',
  imports: [
    CommonModule,        // for *ngFor, *ngIf
    MatTableModule,      // for mat-table
    RouterModule         // for [routerLink] or router.navigate
  ]
})
export class CategoriesListComponent {
  categories: Category[] = [];
  displayedColumns = ['categoryId','categoryName','description'];

  constructor(
    private svc: CategoryService,
    private router: Router
  ) {}

  ngOnInit() {
    this.svc.getCategories().subscribe(data => this.categories = data);
  }

  onEdit(id: number) {
    this.router.navigate(['/categories/edit', id]);
  }
}
