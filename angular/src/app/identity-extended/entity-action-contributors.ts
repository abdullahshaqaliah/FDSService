// src/app/identity-extended/entity-action-contributors.ts

import {
  eIdentityComponents,
  IdentityEntityActionContributors
} from '@abp/ng.identity';
import {
  IdentityUserDto,
} from '@abp/ng.identity/proxy';
import { EntityAction, EntityActionList } from '@abp/ng.theme.shared/extensions';
import { IdentityExtendedComponent } from './identity-extended.component';

const showPackageAction = new EntityAction<IdentityUserDto>({
  text: 'View client packages',
  action: data => {
    const component = data.getInjected(IdentityExtendedComponent);
    component.redirecttoPackagePage(data.record);
  },
});

export function customModalContributor(actionList: EntityActionList<IdentityUserDto>) {
  actionList.addTail(showPackageAction);
}

export const identityEntityActionContributors: IdentityEntityActionContributors = {
  // enum indicates the page to add contributors to
  [eIdentityComponents.Users]: [
    customModalContributor,
    // You can add more contributors here
  ],
};
