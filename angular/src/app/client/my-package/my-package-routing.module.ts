import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyPackageComponent } from './my-package.component';
import { AuthGuard, PermissionGuard } from '@abp/ng.core';

const routes: Routes = [{ path: '', component: MyPackageComponent,
canActivate: [AuthGuard, PermissionGuard] }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MyPackageRoutingModule { }
