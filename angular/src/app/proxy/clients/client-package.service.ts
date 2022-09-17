import type { ClientPackageDto, ClientPackageVersionDownloadDto, CreateClientPackageDto, GetClientPackageInputDto } from './dtos/models';
import { RestService } from '@abp/ng.core';
import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IRemoteStreamContent } from '../volo/abp/content/models';

@Injectable({
  providedIn: 'root',
})
export class ClientPackageService {
  apiName = 'Default';

  create = (input: CreateClientPackageDto) =>
    this.restService.request<any, ClientPackageDto>({
      method: 'POST',
      url: '/api/app/client-package',
      body: input,
    },
    { apiName: this.apiName });

  delete = (id: number) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/client-package/${id}`,
    },
    { apiName: this.apiName });

  downloadFile = (id: string) =>
    this.restService.request<any, IRemoteStreamContent>({
      method: 'POST',
      url: `/api/app/client-package/${id}/download-file`,
    },
    { apiName: this.apiName });

  downloadFromUrl = (id: string) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: `/api/app/client-package/${id}/download-from-url`,
    },
    { apiName: this.apiName });

  getList = (input: GetClientPackageInputDto) =>
    this.restService.request<any, PagedResultDto<ClientPackageDto>>({
      method: 'GET',
      url: '/api/app/client-package',
      params: { clientId: input.clientId, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName });

  getMyPackageList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<ClientPackageDto>>({
      method: 'GET',
      url: '/api/app/client-package/my-package-list',
      params: { skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName });

  getPackageVersionCanUpdateList = (packageId: string) =>
    this.restService.request<any, ListResultDto<ClientPackageVersionDownloadDto>>({
      method: 'GET',
      url: `/api/app/client-package/package-version-can-update-list/${packageId}`,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
