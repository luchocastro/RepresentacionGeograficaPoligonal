import { TestBed } from '@angular/core/testing';
import { UserService } from './user.service';

describe('UserService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should create an instance', () => {
    expect(new UserService()).toBeTruthy();
  });
});
