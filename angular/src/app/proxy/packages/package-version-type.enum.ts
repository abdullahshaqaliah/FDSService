import { mapEnumToOptions } from '@abp/ng.core';

export enum PackageVersionType {
  File = 0,
  Url = 1,
}

export const packageVersionTypeOptions = mapEnumToOptions(PackageVersionType);
