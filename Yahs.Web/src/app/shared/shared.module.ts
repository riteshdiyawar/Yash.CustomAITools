import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { FooterComponent } from '../components/footer/footer.component';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  exports: [NavbarComponent, FooterComponent]
})
export class SharedModule {}
