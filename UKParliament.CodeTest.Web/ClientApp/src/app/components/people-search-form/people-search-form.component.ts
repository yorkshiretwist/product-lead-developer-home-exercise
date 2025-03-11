import { Component } from '@angular/core';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'people-search-form',
  templateUrl: './people-search-form.component.html',
  styleUrls: ['./people-search-form.component.scss']
})
export class PeopleSearchFormComponent {
  departments: DepartmentViewModel[] = [];
  searchText: string | undefined;
  selectedDepartmentId: number | undefined;
  onlyActive: boolean = false;

  constructor(private personService: PersonService, private departmentService: DepartmentService) {
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

  onSearch(): void {
    this.personService.setPeopleSearchParams({
      page: 1,
      pageSize: 25,
      query: this.searchText,
      departmentId: this.selectedDepartmentId,
      onlyActive: this.onlyActive
    });
  }

  onClear(): void {
    this.searchText = undefined;
    this.selectedDepartmentId = undefined;
    this.onlyActive = false;
    this.personService.setPeopleSearchParams({
      page: 1,
      pageSize: 25,
      query: undefined,
      departmentId: undefined,
      onlyActive: undefined
    });
  }
}
