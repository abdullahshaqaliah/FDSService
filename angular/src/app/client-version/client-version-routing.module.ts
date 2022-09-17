import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientVersionComponent } from './client-version.component';
import { AuthGuard, PermissionGuard } from '@abp/ng.core';

const routes: Routes = [{ path: '', component: ClientVersionComponent,
canActivate: [AuthGuard, PermissionGuard],
data: {
  requiredPolicy: 'FDSService.Clients.Packages', // policy key for your component
} }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientVersionRoutingModule { }
