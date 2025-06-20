import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaptheComponent } from './tapthe.component';

describe('TaptheComponent', () => {
  let component: TaptheComponent;
  let fixture: ComponentFixture<TaptheComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TaptheComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaptheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
