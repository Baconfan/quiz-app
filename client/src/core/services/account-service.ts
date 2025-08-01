import {inject, Injectable, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {User} from '../../types/users';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  currectUser = signal<User | null>(null);

  baseUrl = 'http://localhost:5146/api'

  login(creds: any){
    return this.http.post<User>(`${this.baseUrl}/account/login`, creds).pipe(
      tap(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currectUser.set(user)
        }
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.currectUser.set(null);
  }
}
