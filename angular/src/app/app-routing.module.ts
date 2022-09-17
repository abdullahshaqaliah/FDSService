import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () =>
      import('./identity-extended/identity-extended.module')
        .then(m => m.IdentityExtendedModule),
  },
  // {
  //   path: 'identity',
  //   loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  // },
  // {
  //   path: 'tenant-management',
  //   loadChildren: () =>
  //     import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  // },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  { path: 'packages', loadChildren: () => import('./package/package.module').then(m => m.PackageModule) },
  { path: 'package-versions', loadChildren: () => import('./package-version/package-version.module').then(m => m.PackageVersionModule) },
  { path: 'client-versions/:id', loadChildren: () => import('./client-version/client-version.module').then(m => m.ClientVersionModule) },
  { path: 'client/my-packages', loadChildren: () => import('./client/my-package/my-package.module').then(m => m.MyPackageModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
