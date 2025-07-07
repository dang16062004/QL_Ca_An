import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuanvComponent } from './suanv.component';

describe('SuanvComponent', () => {
  let component: SuanvComponent;
  let fixture: ComponentFixture<SuanvComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SuanvComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuanvComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
