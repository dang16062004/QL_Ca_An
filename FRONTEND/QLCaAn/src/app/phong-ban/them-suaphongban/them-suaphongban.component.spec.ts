import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThemSuaphongbanComponent } from './them-suaphongban.component';

describe('ThemSuaphongbanComponent', () => {
  let component: ThemSuaphongbanComponent;
  let fixture: ComponentFixture<ThemSuaphongbanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ThemSuaphongbanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThemSuaphongbanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
