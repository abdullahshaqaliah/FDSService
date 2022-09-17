import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';

import { ClientVersionRoutingModule } from './client-version-routing.module';
import { ClientVersionComponent } from './client-version.component';


@NgModule({
  declarations: [
    ClientVersionComponent
  ],
  imports: [
    SharedModule,
    ClientVersionRoutingModule
  ]
})
export class ClientVersionModule { }
