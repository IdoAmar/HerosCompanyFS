import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HerosPageComponent } from './heros-page.component';

describe('HerosPageComponent', () => {
  let component: HerosPageComponent;
  let fixture: ComponentFixture<HerosPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HerosPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HerosPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
