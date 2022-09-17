import { Component, Injector, OnInit } from '@angular/core';
import { BaseComponent } from '../base.component';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ClientPackageService } from '@proxy/clients/client-package.service'
import { ClientPackageDto, GetClientPackageInputDto, CreateClientPackageDto } from '@proxy/clients/dtos'
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SelectItemDto } from '@proxy/data-lookups/dtos';
import { DataLookupService } from '@proxy/data-lookups'
import { IdentityUserService, IdentityUserDto } from '@abp/ng.identity/proxy';
@Component({
  selector: 'app-client-version',
  templateUrl: './client-version.component.html',
  styleUrls: ['./client-version.component.scss'],
  providers: [ListService],
})
export class ClientVersionComponent extends BaseComponent implements OnInit {
  clientpackages = { items: [], totalCount: 0 } as PagedResultDto<ClientPackageDto>;
  inputFilter = {} as GetClientPackageInputDto;
  isModalOpen = false;
  form: FormGroup;
  packages = [] as SelectItemDto[];
  packageVersions = [] as SelectItemDto[];
  client: IdentityUserDto;
  constructor
    (
      injector: Injector,
      public readonly list: ListService<GetClientPackageInputDto>,
      private currentService: ClientPackageService,
      private route: ActivatedRoute,
      private fb: FormBuilder,
      private dataLookupService: DataLookupService,
      private identityUserService: IdentityUserService
    ) {
    super(injector);
    this.inputFilter.clientId = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit() {
    this.getClientInfo();
    const packageStreamCreator = (query) => this.currentService.getList({ ...query, ...this.inputFilter });

    this.list.hookToQuery(packageStreamCreator).subscribe((response) => {
      this.clientpackages = response;
    });

  }
  getClientInfo() {
    this.identityUserService.get(this.inputFilter.clientId).subscribe((client) => {
      this.client = client;
    });

  }
  create() {
    this.getPackages().then(() => {
      this.buildForm();
      this.isModalOpen = true;
    })

  }
  buildForm() {
    this.form = this.fb.group({
      packageId: [null, Validators.required],
      currentVersionId: [null, Validators.required],
      clientId: [this.inputFilter.clientId, Validators.required]
    });
  }

  getPackages() {
    return new Promise((resolve, reject) => {
      if (this.packages.length) return resolve(true);
      this.dataLookupService.getPackagesList().subscribe((result) => {
        this.packages = result;
        resolve(true);
      });
    })
  }

  getVersions(packageId: string) {
    this.dataLookupService.getPackageVersionsList(packageId).subscribe((r) => {
      this.packageVersions = r;
    })
  }

  save() {
    if (this.form.invalid) {
      return;
    }


    this.currentService.create(this.form.value).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.successSaved();
    });

  }

  delete(id: number, name: string) {
    this.confirmDelete(name).subscribe(r => {
      if (r) {
        this.currentService.delete(id).subscribe(() => {
          this.list.get()
          this.successDeleted();

        });
      }
    });


  }
}
