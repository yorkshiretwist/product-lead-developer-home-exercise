import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';

@Component({
  selector: 'person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.scss']
})
export class PersonFormComponent {
  @Input() person!: PersonViewModel;
  @Output() personChange = new EventEmitter<PersonViewModel>();
  departments: DepartmentViewModel[] = [];

  constructor(private personService: PersonService, private departmentService: DepartmentService) {
    this.getAllDepartments();
  }

  ngOnInit() {
    this.personService.personId$.subscribe(personId => {
      this.getPerson(personId);
    });
  }

  getAllDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (result) => {
        this.departments = result;
      },
      error: (e) => console.error(`Error: ${e}`)
    });
  }

  getPerson(personId: number): void {
    this.personService.getById(personId).subscribe({
      next: (result) => {
        this.person = result;
      },
      error: (e) => console.error(`Error: ${e}`)
    });
  }
}
