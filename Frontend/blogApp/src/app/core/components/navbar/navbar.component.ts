import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../../features/auth/services/auth.service";
import {User} from "../../../features/auth/models/user.model";
import {Router} from "@angular/router";
import {Roles} from "../../../features/auth/models/roles.model";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit{
  user?: User;

  constructor(private authService: AuthService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.authService.user()
      .subscribe({
        next: response => {
          this.user = response;

        }
      });

    this.user = this.authService.getUser();
  }

  onLogout() {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }

  protected readonly Roles = Roles;
}
