import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientVersionComponent } from './client-version.component';

describe('ClientVersionComponent', () => {
  let component: ClientVersionComponent;
  let fixture: ComponentFixture<ClientVersionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientVersionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientVersionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
