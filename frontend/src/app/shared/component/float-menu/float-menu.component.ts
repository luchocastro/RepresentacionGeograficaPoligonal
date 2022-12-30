import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-float-menu',
  templateUrl: './float-menu.component.html',
  styleUrls: ['./float-menu.component.css'],
})
export class FloatMenuComponent {
  @Input() url: string;

  constructor(private router: Router) { }

  redirectTo() {
    this.router.navigate([this.url]);
  }
}
