import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { CreateOrUpdatePackageVersionDto, GetPackageVersionInputDto, PackageVersionDto } from '../packages/dtos/models';

@Injectable({
  providedIn: 'root',
})
export class PackageVersionService {
  apiName = 'Default';

  create = (input: CreateOrUpdatePackageVersionDto) =>
    this.restService.request<any, PackageVersionDto>({
      method: 'POST',
      url: '/api/app/package-version',
      params: { name: input.name, packageId: input.packageId, dependOnVersionId: input.dependOnVersionId, versionNumber: input.versionNumber, availableDate: input.availableDate, isActive: input.isActive, type: input.type, urlPath: input.urlPath, attachmentId: input.attachmentId, channels: input.channels, countries: input.countries },
    },
    { apiName: this.apiName });

  delete = (id: string) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/package-version/${id}`,
    },
    { apiName: this.apiName });

  get = (id: string) =>
    this.restService.request<any, PackageVersionDto>({
      method: 'GET',
      url: `/api/app/package-version/${id}`,
    },
    { apiName: this.apiName });

  getList = (input: GetPackageVersionInputDto) =>
    this.restService.request<any, PagedResultDto<PackageVersionDto>>({
      method: 'GET',
      url: '/api/app/package-version',
      params: { filter: input.filter, packageId: input.packageId, countryId: input.countryId, channelId: input.channelId, type: input.type, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  update = (id: string, input: CreateOrUpdatePackageVersionDto) =>
    this.restService.request<any, PackageVersionDto>({
      method: 'PUT',
      url: `/api/app/package-version/${id}`,
      params: { name: input.name, packageId: input.packageId, dependOnVersionId: input.dependOnVersionId, versionNumber: input.versionNumber, availableDate: input.availableDate, isActive: input.isActive, type: input.type, urlPath: input.urlPath, attachmentId: input.attachmentId, channels: input.channels, countries: input.countries },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
