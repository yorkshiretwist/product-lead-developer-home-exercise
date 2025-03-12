import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';
import { SearchPeopleParamsViewModel } from '../models/search-people-params-view-model';
import { SearchPeopleResultViewModel } from '../models/search-people-result-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private personIdSource = new Subject<number>();
  personId$ = this.personIdSource.asObservable();

  private searchPeopleParamsSource = new Subject<SearchPeopleParamsViewModel>();
  searchPeopleParams$ = this.searchPeopleParamsSource.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  setPersonId(id: number): void {
    this.personIdSource.next(id);
  }

  setPeopleSearchParams(params: SearchPeopleParamsViewModel): void {
    this.searchPeopleParamsSource.next(params);
  }

  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/people/${id}`)
  }

  search(searchParams: SearchPeopleParamsViewModel): Observable<SearchPeopleResultViewModel> {
    return this.http.post<SearchPeopleResultViewModel>(this.baseUrl + `api/people/search`, searchParams)
  }

  updatePerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.put<PersonViewModel>(this.baseUrl + `api/people/${person.id}`, person);
  }

  createPerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<PersonViewModel>(this.baseUrl + `api/people`, person);
  }
}
