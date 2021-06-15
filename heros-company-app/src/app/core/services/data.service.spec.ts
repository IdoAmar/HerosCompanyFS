import { TestBed } from '@angular/core/testing';

import { HerosDataService } from './heros-data.service';

describe('DataService', () => {
  let service: HerosDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HerosDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
