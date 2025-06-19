import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheocanhanComponent } from './theocanhan.component';

describe('TheocanhanComponent', () => {
  let component: TheocanhanComponent;
  let fixture: ComponentFixture<TheocanhanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TheocanhanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheocanhanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
