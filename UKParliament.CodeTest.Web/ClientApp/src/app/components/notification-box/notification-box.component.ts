import { Component, Input } from '@angular/core';
import { NotificationMode } from '../../models/notification-mode';

@Component({
  selector: 'notification-box',
  templateUrl: './notification-box.component.html',
  styleUrls: ['./notification-box.component.scss']
})
export class NotificationBoxComponent {
  @Input() visible: boolean = false;
  @Input() message!: string;
  @Input() mode: NotificationMode = NotificationMode.Warning;

  constructor() {
    
  }
}
