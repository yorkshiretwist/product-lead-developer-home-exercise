import { Component } from '@angular/core';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  departments: DepartmentViewModel[] = [];
  newPerson: PersonViewModel;

  constructor(private departmentService: DepartmentService) {
    this.newPerson = {
      id: 0,
      firstName: '',
      lastName: '',
      departmentId: 0,
      emailAddress: '',
      isActive: false
    };
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
}
