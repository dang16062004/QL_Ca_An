import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DanhsachphongbanComponent } from './danhsachphongban.component';

describe('DanhsachphongbanComponent', () => {
  let component: DanhsachphongbanComponent;
  let fixture: ComponentFixture<DanhsachphongbanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DanhsachphongbanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DanhsachphongbanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
