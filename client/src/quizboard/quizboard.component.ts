import {Component, inject, OnInit} from '@angular/core';
import {QuizService} from '../core/services/quiz-service';
import {QuizBoard} from '../types/quizboard';

@Component({
  selector: 'app-quizboard',
  imports: [],
  templateUrl: './quizboard.component.html',
  styleUrl: './quizboard.component.css'
})
export class QuizboardComponent implements OnInit {

  protected quizService =  inject(QuizService);
  board?: QuizBoard;
  columns: string[] = [];
  rows: number[] = [];

  ngOnInit(): void {
    this.quizService.getQuizboardById("testId").subscribe({
      next: (b:QuizBoard) => {
        this.board = b;
        this.columns = b.categories;
        this.rows = b.valuesAscending;
      },
      error: () => console.error('Quizboard konnte nicht geladen werden'),
    })
  }

  loadOneQuizboard(): void {


  }
}
