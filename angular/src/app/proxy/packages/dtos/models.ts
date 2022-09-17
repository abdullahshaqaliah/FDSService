import type { PackageVersionType } from '../package-version-type.enum';
import type { IRemoteStreamContent } from '../../volo/abp/content/models';
import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateOrUpdatePackageVersionDto {
  name: string;
  packageId?: string;
  dependOnVersionId?: string;
  versionNumber: number;
  availableDate?: string;
  isActive: boolean;
  type: PackageVersionType;
  urlPath?: string;
  attachmentId?: string;
  content: IRemoteStreamContent;
  channels: number[];
  countries: number[];
}

export interface GetPackageVersionInputDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  packageId?: string;
  countryId?: number;
  channelId?: number;
  type?: PackageVersionType;
}

export interface PackageVersionDto extends AuditedEntityDto<string> {
  name?: string;
  packageId?: string;
  package?: string;
  dependOnVersionId?: string;
  versionNumber: number;
  availableDate?: string;
  isActive: boolean;
  type: PackageVersionType;
  typeLabel?: string;
  urlPath?: string;
  attachmentId?: string;
  countries: number[];
  channels: number[];
}

export interface CreateOrUpdatePackageDto {
  name: string;
  isActive: boolean;
}

export interface GetPackageInputDto extends PagedAndSortedResultRequestDto {
  filter?: string;
  isActive?: boolean;
}

export interface PackageDto extends AuditedEntityDto<string> {
  name?: string;
  isActive: boolean;
}
