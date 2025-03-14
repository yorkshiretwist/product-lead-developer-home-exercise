import { PersonViewModel } from "./person-view-model";

export class DefaultPerson implements PersonViewModel {
    id: number;
    firstName: string;
    lastName: string;
    emailAddress: string;
    isActive: boolean;
    departmentId: number;
    departmentName: string;
  constructor() {
    this.id = 0;
    this.firstName = '';
    this.lastName = '';
    this.departmentId = 0;
    this.departmentName = '';
    this.emailAddress = '';
    this.isActive = true;
  }
}
