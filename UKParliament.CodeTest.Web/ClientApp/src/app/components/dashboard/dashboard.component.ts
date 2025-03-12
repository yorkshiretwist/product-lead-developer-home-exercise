import { Component, Input } from '@angular/core';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonViewModel } from '../../models/person-view-model';
import { PersonService } from '../../services/person.service';
import { DefaultPerson } from '../../models/default-person';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  departments: DepartmentViewModel[] = [];
  newPerson: PersonViewModel;

  @Input() personFormVisible: boolean = false;
  @Input() peopleListVisible: boolean = true;

  constructor(private personService: PersonService, private departmentService: DepartmentService) {
    this.newPerson = new DefaultPerson();
    this.personFormVisible = false;
    this.peopleListVisible = true;
  }

  ngOnInit() {
    this.personService.personId$.subscribe(personId => {
      this.personFormVisible = true;
      this.peopleListVisible = false;
    });

    this.personService.searchPeopleParams$.subscribe(personId => {
      this.personFormVisible = false;
      this.peopleListVisible = true;
    });

    this.getAllDepartments();
  }

  getAllDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (result) => {
        this.departments = result;
      },
      error: (e) => console.error(`Error: ${e}`)
    });
  }

  createNewPerson() {
    this.personService.setPersonId(0);
    this.personFormVisible = true;
    this.peopleListVisible = false;
  }
}
