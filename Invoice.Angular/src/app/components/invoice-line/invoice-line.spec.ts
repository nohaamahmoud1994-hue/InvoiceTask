import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceLine } from './invoice-line';

describe('InvoiceLine', () => {
  let component: InvoiceLine;
  let fixture: ComponentFixture<InvoiceLine>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceLine],
    }).compileComponents();

    fixture = TestBed.createComponent(InvoiceLine);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
