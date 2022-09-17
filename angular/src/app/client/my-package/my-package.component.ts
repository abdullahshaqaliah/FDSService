import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { ListService, PagedResultDto, Rest } from '@abp/ng.core';
import { ClientPackageService } from '@proxy/clients';
import { ClientPackageDto, ClientPackageVersionDownloadDto } from '@proxy/clients/dtos';
import { PackageVersionDto } from '@proxy/packages/dtos';
import { IRemoteStreamContent } from '@proxy/volo/abp/content';
// import { saveAs } from 'file-saver';
import * as fileSaver from 'file-saver';
@Component({
  selector: 'app-my-package',
  templateUrl: './my-package.component.html',
  styleUrls: ['./my-package.component.scss'],
  providers: [ListService],
})
export class MyPackageComponent extends BaseComponent implements OnInit {
  @ViewChild('softwareupdate') softwareupdate;
  clientpackages = { items: [], totalCount: 0 } as PagedResultDto<ClientPackageDto>;
  versions:ClientPackageVersionDownloadDto[];
  packageSelected:string;
  isModalOpen:boolean=false;
  constructor
    (
      injector: Injector,
      public readonly list: ListService,
      private currentService: ClientPackageService
    ) {
    super(injector);
  }

  ngOnInit() {
    const packageStreamCreator = (query) => this.currentService.getMyPackageList(query);

    this.list.hookToQuery(packageStreamCreator).subscribe((response) => {
      this.clientpackages = response;
    });

  }
  checkPackageUpdate(data: ClientPackageDto) {
    this.currentService.getPackageVersionCanUpdateList(data.packageId).subscribe((result) => {
      if (!result.items) {
        this.Toaster.warn("There is no software update");
        return;
      }
      this.packageSelected=data.package;
      this.versions=result.items;
      this.isModalOpen=true;
      this.softwareupdate.options={ size: 'lg' };
    });
  }
  

   download(version:ClientPackageVersionDownloadDto): void {
    if (version.type==0) 
    {
      const request: Rest.Request<Blob> = {
        method:  "Post",
        reportProgress:true,
        url: `/api/app/client-package/${version.id}/download-file` ,
        responseType:'blob'
      };
  
      this.rest.request<Blob, Blob>(request).subscribe((file) => {
        fileSaver.saveAs(file,version.fileName);
        this.list.get();
        this.isModalOpen=false;
      });
    }
   
else 
{
  this.currentService.downloadFromUrl(version.id).subscribe((url)=> {
    window.location.href=url;
 }); 
}

  
  }
}
