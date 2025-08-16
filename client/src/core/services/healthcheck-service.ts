import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HealthcheckService {
  private http = inject(HttpClient);
  baseUrl = '/api'

  checkHealth(){
    return this.http.get(`${this.baseUrl}/hc`, {responseType: 'text'})
  }
}


