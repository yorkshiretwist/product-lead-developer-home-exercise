import { Component } from '@angular/core';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor(private personService: PersonService) {
    this.getPersonById(1);
  }

  getPersonById(id: number): void {
    this.personService.getById(id).subscribe(
      result => console.info(`User returned: ${JSON.stringify(result)}`),
      error => console.error(`Error: ${error}`));
  }
}
