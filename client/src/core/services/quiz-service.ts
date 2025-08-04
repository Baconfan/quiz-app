import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {QuizBoard} from '../../types/quizboard';
import {Gamecard} from '../../types/quizboard';
import {catchError, map, Observable, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  private http = inject(HttpClient);
  baseUrl = 'http://localhost:5146/api'

  getAllQuizboards(): any {
    return this.http.get<QuizBoard>(`${this.baseUrl}/quizboard/all`);
  }

  // temp api call
  getFirstQuizboard(): Observable<QuizBoard | null> {
    return this.http.get<QuizBoard[]>(`${this.baseUrl}/quizboard/all`).pipe(
      map(([first]) => first ?? null),
      catchError(err => {
        console.error('Error loading quizboards', err );
        return of(null)
      }));
  }

  getQuizboardById(quizboardId: string) {
    return this.http.get<QuizBoard>(`${this.baseUrl}/quizboard/${quizboardId}`);
  }

  // Update category (column topic)

  // Update pointValues (row values)


  // Create new gamecard / fill gamecard
  createNewGamecard(quizboardId: string, gamecard: Gamecard) {
    // return this.http.post(`${this.baseUrl}/gamecard/new`, {gamecard})
  }

  // Update gamecard by QuizboardId + (categoryId, valueId)
  updateGamecard(quizboardId: string, gamecard: Gamecard) {
    // return this.http.put(`${this.baseUrl}/gamecard/${quizboardId}`, {gamecard})
  }

  // Delete gamecard

}
