import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PackageVersionComponent } from './package-version.component';
import { AuthGuard, PermissionGuard } from '@abp/ng.core';

const routes: Routes = [{ path: '', component: PackageVersionComponent ,
canActivate: [AuthGuard, PermissionGuard],
data: {
  requiredPolicy: 'FDSService.Packages', // policy key for your component
}}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PackageVersionRoutingModule { }
