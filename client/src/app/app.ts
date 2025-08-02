import {Component, inject, OnInit, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Nav} from '../layout/nav/nav';
import {AccountService} from '../core/services/account-service';
import {Router, RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private http = inject(HttpClient);
  protected router = inject(Router);
  protected readonly title = signal('client');

  ngOnInit(): void {
    this.setCurrectUser();
  }

  setCurrectUser() {
    const userString = localStorage.getItem('user');
    if (!userString) {
      return;
    }
    const user = JSON.parse(userString);
    this.accountService.currectUser.set(user)
  }
}
