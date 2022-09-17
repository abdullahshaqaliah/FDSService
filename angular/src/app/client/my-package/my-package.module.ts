import { NgModule } from '@angular/core';

import { MyPackageRoutingModule } from './my-package-routing.module';
import { MyPackageComponent } from './my-package.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    MyPackageComponent
  ],
  imports: [
    SharedModule,
    MyPackageRoutingModule
  ]
})
export class MyPackageModule { }
