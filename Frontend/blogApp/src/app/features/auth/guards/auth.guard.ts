import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {CookieService} from "ngx-cookie-service";
import {AuthService} from "../services/auth.service";
import {jwtDecode} from "jwt-decode";
import {Roles} from "../models/roles.model";

export const authGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);
  const user = authService.getUser();

  // Check for JWT Token
  let token = cookieService.get('Authorization');

  if (token && user) {
    token = token.replace('Bearer', '');
    const decodedToken: any = jwtDecode(token);

    // Check Token expiration date
    const expirationDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();

    if (expirationDate < currentTime) {
      authService.logout();
      return router.createUrlTree(['/login'],
        {queryParams: {returnUrl: state.url}});
    } else {
      // Check for Admin roles
      if (user.roles.includes(Roles.Writer)) {
        return true;
      } else {
        alert('Unauthorized');
        return false;
      }
    }

  } else {
    authService.logout();
    return router.createUrlTree(['/login'],
      {queryParams: {returnUrl: state.url}});
  }
};
