import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { DashboardComponent } from './dashboard.component';
import { DepartmentService } from '../../services/department.service';
import { PersonService } from '../../services/person.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonViewModel } from '../../models/person-view-model';
import { DefaultPerson } from '../../models/default-person';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;
  let mockPersonService: jasmine.SpyObj<PersonService>;

  beforeEach(async () => {
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getAll']);
    mockPersonService = jasmine.createSpyObj('PersonService', ['setPersonId', 'personId$', 'searchPeopleParams$']);

    await TestBed.configureTestingModule({
      declarations: [DashboardComponent],
      providers: [
        { provide: DepartmentService, useValue: mockDepartmentService },
        { provide: PersonService, useValue: mockPersonService }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;

    mockDepartmentService.getAll.and.returnValue(of([{
      id: 1,
      name: 'Department 1'
    }]));
    mockPersonService.personId$ = of(1);
    mockPersonService.searchPeopleParams$ = of({
      page: 1,
      pageSize: 25
    });

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the component with default values', () => {
    expect(component.newPerson).toEqual(new DefaultPerson());
    expect(component.personFormVisible).toBeFalse();
    expect(component.peopleListVisible).toBeTrue();
  });

  it('should call getAllDepartments on init', () => {
    expect(mockDepartmentService.getAll).toHaveBeenCalled();
  });

  it('should set personFormVisible to false and peopleListVisible to true when searchPeopleParams$ is emitted', () => {
    mockPersonService.searchPeopleParams$.subscribe(() => {
      expect(component.personFormVisible).toBeFalse();
      expect(component.peopleListVisible).toBeTrue();
    });
  });

  it('should set personFormVisible to true and peopleListVisible to false when createNewPerson is called', () => {
    component.createNewPerson();
    expect(mockPersonService.setPersonId).toHaveBeenCalledWith(0);
    expect(component.personFormVisible).toBeTrue();
    expect(component.peopleListVisible).toBeFalse();
  });
});
