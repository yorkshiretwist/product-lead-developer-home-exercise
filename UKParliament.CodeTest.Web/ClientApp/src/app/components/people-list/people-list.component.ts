import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { SearchPeopleParamsViewModel } from '../../models/search-people-params-view-model';
import { SearchPeopleResultViewModel } from '../../models/search-people-result-view-model';

@Component({
  selector: 'people-list',
  templateUrl: './people-list.component.html',
  styleUrls: ['./people-list.component.scss']
})
export class PeopleListComponent {
  @Input() visible: boolean = false;

  searchPeopleResult!: SearchPeopleResultViewModel;
  searchPeopleParams: SearchPeopleParamsViewModel;

  constructor(private personService: PersonService) {
    this.searchPeopleParams = {
      pageSize: 25,
      page: 1
    };

    this.searchPeople();
  }

  ngOnInit() {
    this.personService.searchPeopleParams$.subscribe(params => {
      this.searchPeopleParams = params;
      this.searchPeople();
    });
  }

  setPersonId(id: number) {
    this.personService.setPersonId(id);
  }

  searchPeople(): void {
    this.personService.search(this.searchPeopleParams).subscribe({
      next: (result) => {
        this.searchPeopleResult = result;
      },
      error: (e) => console.error(`Error: ${e}`)
    });
  }
}
