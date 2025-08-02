import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {QuizBoard} from '../../types/quizboard';
import {Gamecard} from '../../types/quizboard';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  private http = inject(HttpClient);
  baseUrl = 'http://localhost:5146/api'

  // Get Quizboard
  getAllQuizboards(): any {
    return this.http.get<QuizBoard>(`${this.baseUrl}/quizboard/all`);
  }

  getQuizboardById(quizboardId: string) {
    const testId = "688dede0fb95d8750ac852d6";
    return this.http.get<QuizBoard>(`${this.baseUrl}/quizboard/${testId}`);
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
