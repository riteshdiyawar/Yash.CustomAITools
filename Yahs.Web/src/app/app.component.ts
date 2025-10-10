import { Component } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError, RouterOutlet } from '@angular/router';
import { NgIf, NgTemplateOutlet, AsyncPipe } from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { LoadingService } from './services/loading.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NgIf,
    AsyncPipe,
    NavbarComponent,
    FooterComponent,
    MatProgressBarModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Yash Technologies';
  showShell = false;
  isLoading;

  constructor(private router: Router, private loadingService: LoadingService) {
    this.isLoading = this.loadingService.isLoading;
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.loadingService.show();
      } else if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        setTimeout(() => { // TODO:: Just for validation, remove later.
          this.loadingService.hide();
        }, 1000); // 1 second delay for validation
      }
      if (event instanceof NavigationEnd) {
        this.updateShellVisibility(event.urlAfterRedirects);
      }
    });
  }

  private updateShellVisibility(url: string) {
    const noShellRoutes = ['/login', '/register', '/forgot-password'];
    this.showShell = !noShellRoutes.some(route => url.startsWith(route));
  }
}
