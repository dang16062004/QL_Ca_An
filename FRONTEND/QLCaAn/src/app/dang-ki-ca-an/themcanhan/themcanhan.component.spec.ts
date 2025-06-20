import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThemcanhanComponent } from './themcanhan.component';

describe('ThemcanhanComponent', () => {
  let component: ThemcanhanComponent;
  let fixture: ComponentFixture<ThemcanhanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ThemcanhanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThemcanhanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
