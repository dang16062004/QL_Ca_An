import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDonOnlyComponent } from './update-don-only.component';

describe('UpdateDonOnlyComponent', () => {
  let component: UpdateDonOnlyComponent;
  let fixture: ComponentFixture<UpdateDonOnlyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateDonOnlyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDonOnlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
