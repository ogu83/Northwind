// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter }       from '@angular/router';
import { importProvidersFrom } from '@angular/core';

// Angular Material + Forms + Common
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule }     from '@angular/forms';
import { MatTableModule }          from '@angular/material/table';
import { MatFormFieldModule }      from '@angular/material/form-field';
import { MatInputModule }          from '@angular/material/input';
import { MatButtonModule }         from '@angular/material/button';

import { AppComponent }            from './app/app.component';
import { routes }                  from './app/app-routing.module';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    provideRouter(routes),
    importProvidersFrom(
      BrowserAnimationsModule,
      ReactiveFormsModule,
      MatTableModule,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule
    )
  ]
}).catch(err => console.error(err));
