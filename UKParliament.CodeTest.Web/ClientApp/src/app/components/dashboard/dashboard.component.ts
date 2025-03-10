import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { PersonService } from '../../services/person.service';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  departments$: DepartmentViewModel[] = [];

  constructor(private personService: PersonService, private departmentService: DepartmentService) {
    this.getAllDepartments();
    this.getPersonById(1);
  }

  getAllDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (result) => {
        this.departments$ = result;
        console.log(result);
      },
      error: (e) => console.error(`Error: ${e}`)
    });
  }

  getPersonById(id: number): void {
    this.personService.getById(id).subscribe({
      next: (result) => console.info(`User returned: ${JSON.stringify(result)}`),
      error: (e) => console.error(`Error: ${e}`)
    });
  }
}
