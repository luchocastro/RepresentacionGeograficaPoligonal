import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  navItems = [
    { link: '/dashboard/home', title: 'Home' },
    { link: '/about', title: 'About' }
  ];

  constructor() { }

  ngOnInit() {
  }
}
