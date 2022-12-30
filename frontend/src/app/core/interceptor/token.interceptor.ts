import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtService } from '../service/jwt.service';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private jwtService: JwtService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.log("intercept");
    const headersConfig = {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    };

    const token = this.jwtService.getToken();
    const auth = 'Authorization';
    if (token) {
      headersConfig[auth] = `Bearer ${token}`;
    }

    const request = req.clone({ setHeaders: headersConfig });
    console.log(request);
    return next.handle(request);
  }
}
