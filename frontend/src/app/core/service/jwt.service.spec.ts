import { TestBed } from '@angular/core/testing';
import { JwtService } from './jwt.service';

describe('JwtService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should create an instance', () => {
    expect(new JwtService()).toBeTruthy();
  });
});
