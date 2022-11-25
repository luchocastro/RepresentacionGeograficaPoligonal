import { NgModule } from '@angular/core';
import { MyModalComponent } from './modal/my-modal.component';
import { HomeComponent } from './page/home.component';
import { HomeRoutingModule } from './home.routing';
import { SharedModule } from '@shared/shared.module';
import { ProjectItemComponent } from './page/project-item/project-item.component';
import { ProjectDetailsComponent } from './page/project-details/project-details.component';
import { HomeAuthResolver } from './home-auth-resolver.service';

@NgModule({
    declarations: [
        HomeComponent,
        MyModalComponent,
        ProjectItemComponent,
        ProjectDetailsComponent
    ],
    imports: [
        SharedModule,
        HomeRoutingModule
    ],
    exports: [],
    providers: [
        HomeAuthResolver
    ],
    entryComponents: [
        MyModalComponent
    ]
})
export class HomeModule { }
