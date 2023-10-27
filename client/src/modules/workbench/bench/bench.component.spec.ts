import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BenchComponent } from './bench.component';

describe('BenchComponent', () => {
  let component: BenchComponent;
  let fixture: ComponentFixture<BenchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [BenchComponent]
    });
    fixture = TestBed.createComponent(BenchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
