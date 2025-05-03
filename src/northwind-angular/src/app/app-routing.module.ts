import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesListComponent } from './categories/categories-list/categories-list.component';
import { CategoryEditComponent }   from './categories/category-edit/category-edit.component';

export const routes: Routes = [
    { path: 'categories',          component: CategoriesListComponent },
    { path: 'categories/edit/:id', component: CategoryEditComponent },
    { path: '', redirectTo: 'categories', pathMatch: 'full' },
  ]

  @NgModule({
    imports: [
      RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
  })
  export class AppRoutingModule {}
