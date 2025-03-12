import { Component, EventEmitter, Input, Output, computed, viewChild } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { DefaultPerson } from '../../models/default-person';
import { NotificationBoxComponent } from '../notification-box/notification-box.component';
import { NotificationMode } from '../../models/notification-mode';

@Component({
  selector: 'person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.scss']
})
export class PersonFormComponent {
  @Input() visible: boolean = false;
  @Input() set id(personId: number) {
    this.getPerson(personId);
  }
  @Input() person!: PersonViewModel;
  personCopy!: PersonViewModel;
  @Output() personChange = new EventEmitter<PersonViewModel>();
  departments: DepartmentViewModel[] = [];

  notificationVisible: boolean = false;
  notificationMessage: string = '';
  notificationMode: NotificationMode = NotificationMode.Warning;

  constructor(private personService: PersonService, private departmentService: DepartmentService) {
    this.hideNotification();
    this.setupPerson(new DefaultPerson());
    this.getAllDepartments();
  }

  ngOnInit() {
    this.hideNotification();
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
    this.hideNotification();
    if (personId == 0) {
      this.setupPerson(new DefaultPerson());
      return;
    }
    this.personService.getById(personId).subscribe({
      next: (result) => {
        this.setupPerson(result);
      },
      error: (e) => {
        this.showErrorNotification(`Sorry, the person you are looking for was not found`);
      }
    });
  }

  private setupPerson(person: PersonViewModel) {
    this.person = person;
    this.personCopy = Object.assign({}, person);
  }

  updatePerson(person: PersonViewModel) {
    if (!this.formIsValid()) {
      this.showErrorNotification(`Please fix the erros below`);
      return;
    }
    this.hideNotification();
    this.personService.updatePerson(person).subscribe({
      next: (result) => {
        this.showSuccessNotification(`Person updated successfully`);
        this.setupPerson(result);
        // TODO: refresh the list of people
      },
      error: (e) => {
        this.showErrorNotification(`There was a problem updating this person`);
      }
    });
  }

  createPerson(person: PersonViewModel) {
    if (!this.formIsValid()) {
      this.showErrorNotification(`Please fix the errors below`);
      return;
    }
    this.hideNotification();
    this.personService.createPerson(person).subscribe({
      next: (result) => {
        this.showSuccessNotification(`New person created successfully`);
        this.setupPerson(result);
        // TODO: refresh the list of people
      },
      error: (e) => {
        this.showErrorNotification(`There was a problem creating this new person`);
      }
    });
  }

  formIsValid() {
    // we could validate the model, but I'm taking advantage of HTML form
    // validation, so use that to determine if the form is valid
    const form = document.getElementById('personForm');
    if (form == null) return false;
    const invalidFields = form.querySelectorAll(':invalid');
    if (invalidFields.length == 0) return true;
    return false;
  }

  cancelChanges() {
    this.setupPerson(this.personCopy);
  }

  clearForm() {
    this.hideNotification();
    this.setupPerson(new DefaultPerson());
  }

  // it would be better for these methods to be in the NotificationBoxComponent
  // but lack of time compelled me to put them here
  hideNotification() {
    this.notificationVisible = false;
  }

  showErrorNotification(message: string) {
    this.notificationVisible = true;
    this.notificationMessage = message;
    this.notificationMode = NotificationMode.Error;
  }

  showSuccessNotification(message: string) {
    this.notificationVisible = true;
    this.notificationMessage = message;
    this.notificationMode = NotificationMode.Success;
  }
}
