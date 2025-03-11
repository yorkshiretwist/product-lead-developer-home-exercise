export interface SearchPeopleParamsViewModel {
  query?: string;
  page: number;
  pageSize: number;
  departmentId?: number;
  onlyActive?: boolean;
}
