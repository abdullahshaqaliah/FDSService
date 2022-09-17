import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/package-software',
        name: '::Menu:PackageSoftware',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/packages',
        name: '::Menu:Packages',
        parentName: '::Menu:PackageSoftware',
        layout: eLayoutType.application,
        requiredPolicy: 'FDSService.Packages',
      },   
      {
        path: '/package-versions',
        name: '::Menu:PackageVersions',
        parentName: '::Menu:PackageSoftware',
        layout: eLayoutType.application,
        requiredPolicy: 'FDSService.Packages',
      },  
      {
        path: '/client/my-packages',
        name: '::Menu:MyPackages',
        parentName: '::Menu:PackageSoftware',
        layout: eLayoutType.application
      },           
    ]);
  };
}
