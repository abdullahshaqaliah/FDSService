// src/app/identity-extended/identity-extended.component.ts

import { IdentityUserDto } from '@abp/ng.identity/proxy';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-identity-extended',
  templateUrl: './identity-extended.component.html',
})
export class IdentityExtendedComponent {

  user: IdentityUserDto;
  constructor(private router: Router){}
  redirecttoPackagePage(record: IdentityUserDto) {
    this.router.navigate(['/client-versions',record.id])
  }
}
