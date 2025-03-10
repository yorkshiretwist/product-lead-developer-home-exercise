import { DepartmentViewModel } from "./department-view-model";

export interface PersonViewModel {
  id: number;
  firstName: string;
  lastName: string;
  emailAddress: string;
  isActive: boolean;
  department: DepartmentViewModel;
}
