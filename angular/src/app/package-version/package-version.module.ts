import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { PackageVersionRoutingModule } from './package-version-routing.module';
import { PackageVersionComponent } from './package-version.component';


@NgModule({
  declarations: [
    PackageVersionComponent
  ],
  imports: [
    SharedModule,
    PackageVersionRoutingModule
    ]
})
export class PackageVersionModule { }
