import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {GamecardType, QuizBoard} from '../../types/quizboard';
import {catchError, map, Observable, of} from 'rxjs';
import {DataForUpsertCategory} from '../../types/dataForUpsertCategory';
import {ImageUploadMetaData} from '../../types/imageUploadToQuizcardDto';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  private http = inject(HttpClient);
  baseUrl = '/api'

  /*
  getAllQuizboards(): any {
    return this.http.get<QuizBoard>(`${this.baseUrl}/quizboard/all`);
  }
  */

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

  // Update category (column = category)
  upsertCategory(quizboardId: string, categoryId: number, newCategory: string) {
    var dataForUpsert : DataForUpsertCategory = {quizboardId: quizboardId, categoryId: categoryId, newCategoryName: newCategory};
    return this.http.put(`${this.baseUrl}/quizboard/category/upsert`, dataForUpsert);
  }

  // TODO Update pointValues (row values)

  upsertGamecard(gamecard: GamecardType) {
    return this.http.put(`${this.baseUrl}/quizcard/upsert`, gamecard)
  }

  uploadImage(file: File, metaData: ImageUploadMetaData){
    const formData = new FormData();
    formData.append('file', file);
    formData.append('metaData', JSON.stringify(metaData));

    return this.http.post(`${this.baseUrl}/quizcard/add-image`, formData);
  }
}
