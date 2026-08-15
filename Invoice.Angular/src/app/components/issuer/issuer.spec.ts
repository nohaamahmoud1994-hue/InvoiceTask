import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Issuer } from './issuer';

describe('Issuer', () => {
  let component: Issuer;
  let fixture: ComponentFixture<Issuer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Issuer],
    }).compileComponents();

    fixture = TestBed.createComponent(Issuer);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
