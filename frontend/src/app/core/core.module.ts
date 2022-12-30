import { NgModule, Optional, SkipSelf } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthGuard } from '@app/guard/auth.guard';
import { ApiService } from '@app/service/api.service';
import { UserService } from '@app/service/user.service';
import { JwtService } from '@app/service/jwt.service';
import { throwIfAlreadyLoaded } from '@app/guard/module-import.guard';

import { TokenInterceptor } from '@app/interceptor/token.interceptor';
import { ErrorInterceptor } from '@app/interceptor/error.interceptor';

import { NgxSpinnerModule } from 'ngx-spinner';
import { NoAuthGuard } from './guard/no-auth.guard';

@NgModule({
  imports: [HttpClientModule, NgxSpinnerModule],
  providers: [
    ApiService,
    AuthGuard,
    NoAuthGuard,
    JwtService,
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}
