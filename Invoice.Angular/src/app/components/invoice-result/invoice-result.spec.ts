import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceResult } from './invoice-result';

describe('InvoiceResult', () => {
  let component: InvoiceResult;
  let fixture: ComponentFixture<InvoiceResult>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceResult],
    }).compileComponents();

    fixture = TestBed.createComponent(InvoiceResult);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
