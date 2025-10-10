import { Routes } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
// import { DashboardComponent } from './components/dashboard/dashboard.component';
import { TileDetailsComponent } from './components/tile-details/tile-details.component';

import { ContactComponent } from './components/contact/contact.component';

// import { SettingsComponent } from './components/settings/settings.component';
import { AboutComponent } from './components/about/about.component';

// import { ProfileComponent } from './components/profile/profile.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', loadChildren: () => import('./components/dashboard/dashboard.module').then(m => m.DashboardModule) },
  { path: 'tile-details/:id', component: TileDetailsComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'settings', loadChildren: () => import('./components/settings/settings.module').then(m => m.SettingsModule) },
  { path: 'about', component: AboutComponent },
  { path: 'profile', loadChildren: () => import('./components/profile/profile.module').then(m => m.ProfileModule) },
  { path: 'project-detail', component:ProjectDetailComponent },
  { path: '**', component: NotFoundComponent } // 404 Not Found route
  


];
