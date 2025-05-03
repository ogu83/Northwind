import { Component, OnInit }        from '@angular/core';
import { ActivatedRoute, Router, RouterModule }     from '@angular/router';
import { CommonModule }     from '@angular/common';
import { CategoryService }  from '../../services/category.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category } from '../../models/category.model';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatLabel,
    RouterModule
  ]
})
export class CategoryEditComponent implements OnInit {
  categoryForm!: FormGroup;
  id!: number;

  constructor(
    private fb: FormBuilder,
    private svc: CategoryService,
    private route: ActivatedRoute,
    public  router: Router
  ) {}

  ngOnInit() {
    this.id = +this.route.snapshot.paramMap.get('id')!;
    this.categoryForm = this.fb.group({
      categoryId:   [{ value: this.id, disabled: true }],
      categoryName: ['', Validators.required],
      description:  ['']
    });

    this.svc.getCategory(this.id).subscribe(cat =>
      this.categoryForm.patchValue(cat)
    );
  }

  onCancel() { this.router.navigate(['/categories']); }

  onSubmit() {
    if (!this.categoryForm.valid) { return; }

    const updated: Category = {
      ...this.categoryForm.getRawValue(),
      categoryId: this.id
    };
    this.svc.saveCategory(updated).subscribe(() =>
      this.router.navigate(['/categories'])
    );
  }
}