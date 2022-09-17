import type { CreateOrUpdatePackageDto, GetPackageInputDto, PackageDto } from './dtos/models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PackageService {
  apiName = 'Default';

  create = (input: CreateOrUpdatePackageDto) =>
    this.restService.request<any, PackageDto>({
      method: 'POST',
      url: '/api/app/package',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/package/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, PackageDto>({
      method: 'GET',
      url: `/api/app/package/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetPackageInputDto) =>
    this.restService.request<any, PagedResultDto<PackageDto>>({
      method: 'GET',
      url: '/api/app/package',
      params: { filter: input.filter, isActive: input.isActive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateOrUpdatePackageDto) =>
    this.restService.request<any, PackageDto>({
      method: 'PUT',
      url: `/api/app/package/${id}`,
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
