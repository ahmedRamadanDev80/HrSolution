import { Component, OnInit, computed, inject } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
  standalone: true,
  imports: [NgIf, RouterModule, NgFor],
})
export class NavbarComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);

  constructor() { }

  ngOnInit() {
  }

  // Computed signal for auth state
  isAuthenticated = computed(() => this.authService.isAuthenticated());

  // Navigation items
  navItems = [
    { label: 'Employees', path: '/employees' },
    { label: 'Departments', path: '/departments' },
    { label: 'Job Titles', path: '/jobtitles' },
  ];

  login() {
    this.router.navigate(['/login']);
  }

  logout() {
    this.authService.logout(); // implement later
    this.router.navigate(['/login']);
  }

}
