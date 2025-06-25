import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateNhanvienComponent } from './create-nhanvien.component';

describe('CreateNhanvienComponent', () => {
  let component: CreateNhanvienComponent;
  let fixture: ComponentFixture<CreateNhanvienComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateNhanvienComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateNhanvienComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
