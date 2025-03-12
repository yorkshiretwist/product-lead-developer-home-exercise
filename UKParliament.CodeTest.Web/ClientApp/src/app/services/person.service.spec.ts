import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonViewModel } from '../models/person-view-model';
import { SearchPeopleParamsViewModel } from '../models/search-people-params-view-model';
import { SearchPeopleResultViewModel } from '../models/search-people-result-view-model';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost:5000/';

  const searchParams: SearchPeopleParamsViewModel = {
    query: 'python',
    page: 1,
    pageSize: 10,
    departmentId: 1,
    onlyActive: false
  };
  const mockPerson: PersonViewModel = {
    id: 1,
    firstName: 'John',
    lastName: 'Cleese',
    emailAddress: 'john.cleese@python.org',
    departmentId: 1,
    departmentName: 'Funny Walks',
    isActive: true
  };
  const searchResult: SearchPeopleResultViewModel = {
    totalCount: 1,
    page: 1,
    pageSize: 10,
    totalPages: 1,
    items: [mockPerson]
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PersonService,
        { provide: 'BASE_URL', useValue: baseUrl }
      ]
    });
    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the service', () => {
    expect(service).toBeTruthy();
  });

  it('should set personId and emit the value', (done) => {
    service.personId$.subscribe(id => {
      expect(id).toBe(1);
      done();
    });
    service.setPersonId(1);
  });

  it('should set searchPeopleParams and emit the value', (done) => {
    service.searchPeopleParams$.subscribe(emittedParams => {
      expect(emittedParams).toEqual(searchParams);
      done();
    });
    service.setPeopleSearchParams(searchParams);
  });

  it('should fetch a person by id', () => {
    service.getById(1).subscribe(person => {
      expect(person).toEqual(mockPerson);
    });
    const req = httpMock.expectOne(`${baseUrl}api/people/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockPerson);
  });

  it('should search people', () => {
    service.search(searchParams).subscribe(result => {
      expect(result).toEqual(searchResult);
    });
    const req = httpMock.expectOne(`${baseUrl}api/people/search`);
    expect(req.request.method).toBe('POST');
    req.flush(searchResult);
  });

  it('should update a person', () => {
    service.updatePerson(mockPerson).subscribe(person => {
      expect(person).toEqual(mockPerson);
    });
    const req = httpMock.expectOne(`${baseUrl}api/people/1`);
    expect(req.request.method).toBe('PUT');
    req.flush(mockPerson);
  });

  it('should create a person', () => {
    service.createPerson(mockPerson).subscribe(person => {
      expect(person).toEqual(mockPerson);
    });
    const req = httpMock.expectOne(`${baseUrl}api/people`);
    expect(req.request.method).toBe('POST');
    req.flush(mockPerson);
  });
});
