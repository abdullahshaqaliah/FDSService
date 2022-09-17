import type { SelectItemDto } from './dtos/models';
import { RestService } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DataLookupService {
  apiName = 'Default';

  getChannelsList = () =>
    this.restService.request<any, ListResultDto<SelectItemDto>>({
      method: 'GET',
      url: '/api/app/data-lookup/channels-list',
    },
    { apiName: this.apiName });

  getCountiresList = () =>
    this.restService.request<any, ListResultDto<SelectItemDto>>({
      method: 'GET',
      url: '/api/app/data-lookup/countires-list',
    },
    { apiName: this.apiName });

  getPackageVersionsList = (packageId: string) =>
    this.restService.request<any, SelectItemDto[]>({
      method: 'GET',
      url: `/api/app/data-lookup/package-versions-list/${packageId}`,
    },
    { apiName: this.apiName });

  getPackagesList = () =>
    this.restService.request<any, SelectItemDto[]>({
      method: 'GET',
      url: '/api/app/data-lookup/packages-list',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
