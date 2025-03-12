import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule, Routes, provideRouter, withComponentInputBinding } from '@angular/router';

import { AppComponent } from './app.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PeopleListComponent } from './components/people-list/people-list.component';
import { PersonFormComponent } from './components/person-form/person-form.component';
import { PeopleSearchFormComponent } from './components/people-search-form/people-search-form.component';
import { NotificationBoxComponent } from './components/notification-box/notification-box.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: '', component: DashboardComponent, pathMatch: 'full' },
  { path: 'people/:id', component: DashboardComponent, pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({ declarations: [
    AppComponent,
    DashboardComponent,
    PeopleSearchFormComponent,
    PeopleListComponent,
    PersonFormComponent,
    NotificationBoxComponent
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    provideRouter(routes, withComponentInputBinding())
  ]
})
export class AppModule { }
