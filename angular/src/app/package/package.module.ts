import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PackageRoutingModule } from './package-routing.module';
import { PackageComponent } from './package.component';


@NgModule({
  declarations: [
    PackageComponent
  ],
  imports: [
    SharedModule,
    PackageRoutingModule
  ]
})
export class PackageModule { }
