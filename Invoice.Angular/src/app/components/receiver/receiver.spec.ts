import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Receiver } from './receiver';

describe('Receiver', () => {
  let component: Receiver;
  let fixture: ComponentFixture<Receiver>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Receiver],
    }).compileComponents();

    fixture = TestBed.createComponent(Receiver);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
