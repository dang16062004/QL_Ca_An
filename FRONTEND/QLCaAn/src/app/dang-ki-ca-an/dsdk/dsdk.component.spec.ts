import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DSDKComponent } from './dsdk.component';

describe('DSDKComponent', () => {
  let component: DSDKComponent;
  let fixture: ComponentFixture<DSDKComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DSDKComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DSDKComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
