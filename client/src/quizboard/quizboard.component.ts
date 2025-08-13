import {ChangeDetectionStrategy, Component, inject, OnInit, signal} from '@angular/core';
import {QuizService} from '../core/services/quiz-service';
import {QuizBoard} from '../types/quizboard';
import {Gamecard} from './gamecard/gamecard';
import {
  MatDialog
} from '@angular/material/dialog';

import {DataForCardEdit} from '../types/dataForCardEdit';
import {ToastService} from '../core/services/toast-service';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-quizboard',
  imports: [FormsModule],
  templateUrl: './quizboard.component.html',
  styleUrl: './quizboard.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class QuizboardComponent implements OnInit {
  protected quizService =  inject(QuizService);
  protected quizboard = signal<QuizBoard | null>(null);
  toastService = inject(ToastService);

  readonly dialog = inject(MatDialog);

  ngOnInit(): void {
    this.quizService.getFirstQuizboard().subscribe({
      next: result => {
        this.quizboard.set(result);
      },
      error: () => console.error('Quizboard konnte nicht geladen werden'),
    })
  }

  openCardEditor(categoryId: number, valueId: number, categoryName: string, valuePoints: number){
    const dialogRef = this.dialog.open<Gamecard, DataForCardEdit, string>
    (Gamecard,
      {width: '70vw',
        maxWidth: '720px',
        data: {
          quizboardId: this.quizboard()!.id,
          categoryId: categoryId,
          valueId: valueId,
          categoryTitle: categoryName,
          valueTitle: valuePoints
        },
        disableClose: true,
      });

  }

  saveCategory(newCategory: string, quizboardId: string, categoryId: number) {
    this.quizService.upsertCategory(quizboardId, categoryId, newCategory).subscribe({
      next: () => {this.toastService.success("Gespeichert!")},
      error: () => {this.toastService.error("Fehler beim Speichern!")}
    });
  }
}
