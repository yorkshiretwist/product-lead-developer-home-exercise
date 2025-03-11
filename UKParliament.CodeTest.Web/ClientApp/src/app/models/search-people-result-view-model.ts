import { PersonViewModel } from "./person-view-model";

export interface SearchPeopleResultViewModel {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: PersonViewModel[];
}
