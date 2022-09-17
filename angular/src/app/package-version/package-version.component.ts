import { Component, Injector, OnInit } from '@angular/core';
import { ListService, PagedResultDto, Rest } from '@abp/ng.core';
import { BaseComponent } from '../base.component';
import { PackageVersionService } from '@proxy/package-versions/package-version.service'
import { DataLookupService } from '@proxy/data-lookups'
import { SelectItemDto } from '@proxy/data-lookups/dtos'
import { PackageVersionDto, GetPackageVersionInputDto, PackageDto, CreateOrUpdatePackageVersionDto } from '@proxy/packages/dtos'
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-package-version',
  templateUrl: './package-version.component.html',
  styleUrls: ['./package-version.component.scss'],
  providers: [ListService]
})
export class PackageVersionComponent extends BaseComponent implements OnInit {
  versions = { items: [], totalCount: 0 } as PagedResultDto<PackageVersionDto>;
  selectedpackage = {} as PackageVersionDto;
  inputFilter = { packageId: null, countryId: null, channelId: null, type: null } as GetPackageVersionInputDto;

  countries = [] as SelectItemDto[];
  channels = [] as SelectItemDto[];
  packages = [] as SelectItemDto[];
  isSearchModalOpen = false;
  isModalOpen = false;
  inProgress = false;
  file: File;
  form: FormGroup;
  dpendOnVersions: SelectItemDto[];
  constructor(
    injector: Injector,
    public readonly list: ListService<GetPackageVersionInputDto>,
    private currentService: PackageVersionService,
    private dataLookupService: DataLookupService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private datePipe: DatePipe
  ) {
    super(injector);

    this.route.queryParams
      .subscribe(params => {
        if (params?.packageId) {
          this.inputFilter.packageId = params.packageId;
        }
      })

  }

  ngOnInit() {
    const packageStreamCreator = (query) => this.currentService.getList({ ...query, ...this.inputFilter });

    this.list.hookToQuery(packageStreamCreator).subscribe((response) => {
      this.versions = response;
    });

  }
  showSearchModal() {
    this.GetLookupData().then ((values)=> {
      this.isSearchModalOpen = true;

    })
  }
  create() {
    this.selectedpackage = {} as PackageVersionDto;

    if (this.inputFilter.packageId) {
      this.getVersions(this.inputFilter.packageId).then(()=> {
        this.GetLookupData().then ((values)=> {
          this.buildForm();
          this.isModalOpen = true;
        });
      });
    }
    else {
      this.GetLookupData().then ((values)=> {
        this.buildForm();
        this.isModalOpen = true;
      });
    }


  }
  edit(id: string) {
    this.GetLookupData().then ((values)=> {
      this.currentService.get(id).subscribe((result) => {
        this.selectedpackage = result;
        this.getVersions(this.selectedpackage.packageId).then(()=> {
          this.buildForm();
          this.isModalOpen = true;
        });
  
      });
    });

  }
  
  buildForm() {

    this.form = this.fb.group({
      name:
        [this.selectedpackage.name || '',
        Validators.compose(
          [Validators.minLength(3), Validators.maxLength(250), Validators.required])
        ],
      packageId: [this.selectedpackage.packageId || this.inputFilter.packageId, Validators.required],
      versionNumber: [this.selectedpackage.versionNumber || null, [Validators.required, Validators.pattern("^[0-9]*$")]],
      availableDate: [
        this.selectedpackage.availableDate ? this.datePipe.transform(this.selectedpackage.availableDate,"yyyy-MM-dd") : null,
        Validators.required,
      ],
      type: [this.selectedpackage?.type?.toString() || "", [Validators.required]],
      countries: [this.selectedpackage?.countries?.map(x=> x.toString()) || null, [Validators.required]],
      channels: [this.selectedpackage?.channels?.map(x=> x.toString()) || null, [Validators.required]],
      urlPath: [this.selectedpackage.urlPath || null, []],
      attachmentId: [this.selectedpackage.attachmentId || null],
      dependOnVersionId: [this.selectedpackage.dependOnVersionId || null],
      isActive: [this.selectedpackage.isActive || false, Validators.required]
    });
  }

   GetLookupData() {
    if (this.countries.length) {
     return Promise.resolve([this.countries,this.channels,this.packages]);
    }
    else {

      return new Promise((resolve, reject) => { 
        Promise.all([this.getCountries(), this.getChannles(), this.getPackages()]).then((values) => {
          resolve(values);
        });
      });
    }

  }

  
  getCountries() {
    return new Promise((resolve, reject) => { 
      this.dataLookupService.getCountiresList().subscribe((result) => {
        this.countries = result.items;
        resolve(result.items);
      });
    });

  }
  getChannles() {
    return new Promise((resolve, reject) => { 
      this.dataLookupService.getChannelsList().subscribe((result) => {
        this.channels = result.items;
        resolve(result.items);
      });
    });
  }
  getPackages() {
    return new Promise((resolve, reject) => { 
      this.dataLookupService.getPackagesList().subscribe((result) => {
        this.packages = result;
        resolve(result);
      });
    });   

  }
  save() {
    if (this.form.invalid) {
      return;
    }
    this.inProgress = false;



    const formulario = new FormData();

    const formData = this.form.value;

    Object.keys(formData).forEach((key) => {
      if (formData[key]) {
        if (Array.isArray(formData[key])) {

          for (var i = 0; i < formData[key].length; i++) {
            formulario.append(`${key}[]`, formData[key][i]);
          }

        }
        else {
          formulario.append(key, formData[key]);

        }

      }
    });
    formulario.append('content', this.file);

    const request: Rest.Request<any> = {
      method: this.selectedpackage.id ? 'PUT' : "Post",
      url: this.selectedpackage.id ? '/api/app/package-version/' + this.selectedpackage.id : "/api/app/package-version",
      body: formulario,
      headers: {
        contentType: "multipart/form-data",
        accept: "*"
      }
    };

    this.rest.request<null, PackageVersionDto>(request).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
      this.successSaved();

    });

  }

  onFileChange(event) {

    if (event.target.files.length > 0) {
      this.file = event.target.files[0];

    }
  }

  getVersions(packageId: string) {
    return new Promise((resolve, reject) => {

      this.dataLookupService.getPackageVersionsList(packageId).subscribe((r) => {
        this.dpendOnVersions = r;
        resolve(true);
      });
    })
  }
  search() {
    this.isSearchModalOpen = false;
    this.list.get();
  }


  delete(version:PackageVersionDto) {
    this.confirmDelete(version.name).subscribe(r => {
      if (r) {
        this.currentService.delete(version.id).subscribe(() => {
          this.list.get()
          this.successDeleted();

        });
      }
    });


  }
}
