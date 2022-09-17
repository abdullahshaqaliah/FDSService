import { Component, Injector, OnInit } from '@angular/core';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { PackageService } from '@proxy/packages'
import { PackageDto, GetPackageInputDto } from '@proxy/packages/dtos'
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BaseComponent } from '../base.component';


@Component({
  selector: 'app-package',
  templateUrl: './package.component.html',
  styleUrls: ['./package.component.scss'],
  providers: [ListService],
})
export class PackageComponent extends BaseComponent implements OnInit {
  packages = { items: [], totalCount: 0 } as PagedResultDto<PackageDto>;
  selectedpackage = {} as PackageDto;
  inputFilter = {} as GetPackageInputDto;

  isSearchModalOpen = false;
  isModalOpen = false;
  form: FormGroup;

  constructor(
    injector: Injector,
    public readonly list: ListService<GetPackageInputDto>,
    private packageService: PackageService,
    private fb: FormBuilder) {
    super(injector);

  }
  ngOnInit() {
    const packageStreamCreator = (query) => this.packageService.getList({ ...query, ...this.inputFilter });

    this.list.hookToQuery(packageStreamCreator).subscribe((response) => {
      this.packages = response;
    });
  }
  create() {
    this.selectedpackage = {} as PackageDto;
    this.buildForm();
    this.isModalOpen = true;
  }
  edit(id: string) {
    this.packageService.get(id).subscribe((result) => {
      this.selectedpackage = result;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name:
        [this.selectedpackage.name || '',
        Validators.compose(
          [Validators.minLength(3), Validators.maxLength(250), Validators.required])
        ],
      isActive: [this.selectedpackage.isActive || false, Validators.required]
    });
  }

  delete(id: string, name: string) {
    this.confirmDelete(name).subscribe(r => {
      if (r) {
        this.packageService.delete(id).subscribe(() => {
          this.list.get()
          this.successDeleted();

        });
      }
    });


  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedpackage.id
      ? this.packageService.update(this.selectedpackage.id, this.form.value)
      : this.packageService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.successSaved();
    });

  }
  search() {
    this.isSearchModalOpen = false;
    this.list.get();
  }
}
