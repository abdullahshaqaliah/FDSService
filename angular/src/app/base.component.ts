import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { LocalizationService, ConfigStateService, LocalizationWithDefault } from '@abp/ng.core';
import { Observable, Subject, Subscribable } from 'rxjs';
import { RestService } from '@abp/ng.core';



//ng generate component base --inline-template --inline-style --skip-tests --module app

export abstract class BaseComponent {
    confirmation: ConfirmationService
    localization: LocalizationService
    Config: ConfigStateService
    Toaster: ToasterService
    rest: RestService
    constructor(injector: Injector) {

        this.confirmation = injector.get(ConfirmationService);
        this.localization = injector.get(LocalizationService);
        this.Config = injector.get(ConfigStateService);
        this.Toaster = injector.get(ToasterService);
        this.rest = injector.get(RestService);
    }

    L(key: string | LocalizationWithDefault, ...interpolateParams: string[]) {
        return this.localization.instant(`FDSService::${key}`, ...interpolateParams)
    }
    successSaved() {
        this.Toaster.success(this.localization.instant('AbpSettingManagement::SuccessfullySaved'));
    }
    successDeleted() {
        this.Toaster.success(this.L("SuccessfullyDeleted"));
    }
    confirmDelete(title: string): Observable<boolean> {
        var isConfirm = new Subject<boolean>();
        this.confirmation.warn(this.L("DeletionWarningMessage", title), '::AreYouSure').subscribe((status) => {
            if (status === Confirmation.Status.confirm) {
                isConfirm.next(true);

            }
            else {
                isConfirm.next(false);
            }
        });
        return isConfirm.asObservable();
    }

}