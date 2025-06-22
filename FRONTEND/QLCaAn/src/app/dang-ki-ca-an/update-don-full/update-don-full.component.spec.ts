import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDonFullComponent } from './update-don-full.component';

describe('UpdateDonFullComponent', () => {
  let component: UpdateDonFullComponent;
  let fixture: ComponentFixture<UpdateDonFullComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateDonFullComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDonFullComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
