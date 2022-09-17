import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { PackageVersionType } from '../../packages/package-version-type.enum';

export interface ClientPackageDto extends AuditedEntityDto<number> {
  clientId?: string;
  packageId?: string;
  package?: string;
  currentVersion?: string;
}

export interface ClientPackageVersionDownloadDto extends AuditedEntityDto<string> {
  name?: string;
  versionNumber: number;
  type: PackageVersionType;
  fileName?: string;
}

export interface CreateClientPackageDto {
  clientId?: string;
  packageId?: string;
  currentVersionId?: string;
}

export interface GetClientPackageInputDto extends PagedAndSortedResultRequestDto {
  clientId?: string;
}
