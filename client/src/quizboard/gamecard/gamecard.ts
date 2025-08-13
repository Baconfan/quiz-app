import {ChangeDetectionStrategy, Component, inject, OnInit, output} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {GamecardType, QuizBoard} from '../../types/quizboard';
import {MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {QuizService} from '../../core/services/quiz-service';
import {DataForCardEdit} from '../../types/dataForCardEdit';
import {GameModeType} from '../../types/gamemode';
import {filter, map, Observable, startWith, tap} from 'rxjs';
import {AsyncPipe} from '@angular/common';
import {ToastService} from '../../core/services/toast-service';
import {ImageUpload} from '../../shared/image-upload/image-upload';

@Component({
  selector: 'app-gamecard',
  imports: [FormsModule, MatDialogModule, MatButtonModule, AsyncPipe, ImageUpload],
  templateUrl: './gamecard.html',
  styleUrl: './gamecard.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Gamecard implements OnInit {
  protected gcard: GamecardType = {
    isMultipleChoice: false,
    questionImages: [],
    quizboardId: '',
    categoryId: null,
    valueId: null,
    clues: [],
    eyecatcherTitle: '',
    gameMode: '',
    optionalClue: '',
    possibleAnswers: {
      correctAnswers: [
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        }
      ],
      wrongAnswers: [
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        },
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        },{
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        }
      ],
      areClickable: false
    },
    questionText: ''
  };

  gamecard$!: Observable<GamecardType>;

  readonly dialogRef = inject(MatDialogRef<Gamecard>);
  readonly possibleGameModes: GameModeType[] = [
    { key: 'guess1WordWithOnlyQuestion', label: 'Begriff erraten (Frage)' },
    { key: 'guess1WordWithClues', label: 'Begriff erraten (Anhand von Hinweisen)' },
    { key: 'guess1WordWithImages', label: 'Begriff erraten (anhand von Bildern)' },
    { key: 'guess1WordWithSounds', label: 'Begriff erraten (anhand von Soundtrack)' },
    { key: 'multipleChoices1Word', label: 'Mehrere Antworten, eine gesucht' },
    { key: 'guessMultipleWords', label: 'Mehrere passende Begriffe nennen' }
  ]

  dataFromBoardComponent: DataForCardEdit = inject(MAT_DIALOG_DATA);
  quizservice = inject(QuizService);
  toastService = inject(ToastService);

  ngOnInit(): void {
    /*
    this.quizservice
      .getFirstQuizboard()
      .subscribe(data => {
        let x  = data?.gamecards
          .find(c => c.categoryId == this.dataFromBoardComponent.categoryId && c.valueId == this.dataFromBoardComponent.valueId)
        if(x){
          this.gcard = x
        }
      })
    */
    this.loadQuizcard();


  }

  loadQuizcard(){
    this.gcard.quizboardId = this.dataFromBoardComponent.quizboardId;
    this.gcard.valueId = this.dataFromBoardComponent.valueId;
    this.gcard.categoryId = this.dataFromBoardComponent.categoryId;

    this.gamecard$ = this.quizservice.getFirstQuizboard()
      .pipe(
        filter((qb): qb is QuizBoard => !!qb),
        map(qb =>
          qb.gamecards?.find(c =>
            c.categoryId == this.dataFromBoardComponent.categoryId && c.valueId == this.dataFromBoardComponent.valueId) || this.gcard),
        tap(card => this.gcard = card));
  }

  saveEdit(){
    console.log('Card was created!');
    this.quizservice.upsertGamecard(this.gcard).subscribe({
      next: (res) => this.toastService.success('Änderungen gespeichert'),
      error: (e) => {this.toastService.error('Fehler beim Speichern!'); console.log(e.message)},
      complete: () => {this.dialogRef.close()}
    })

    this.dialogRef.close();
  }

  /*
  addCorrectAnswerInput(){
    this.gcard.possibleAnswers.correctAnswers.push(
      {
      textAnswer: '',
      imageLink: "",
      soundLink: "",
      explanation: ""
      });
  }
  */

  onGameModeChange(selected: string) {
    console.log('Game mode changed', selected);
    if(selected == 'guess1WordWithOnlyQuestion'){
      this.gcard.clues = [];
      this.gcard.possibleAnswers.wrongAnswers = [];
    }

    if(selected == 'guess1WordWithClues'){
      this.gcard.clues = [
        '','',''
      ];

      this.gcard.possibleAnswers.wrongAnswers = [];
    }

    if(selected == 'multipleChoices1Word'){
      this.gcard.clues = [];
      this.gcard.possibleAnswers.wrongAnswers = [{
        textAnswer: '',
        imageUrl: "",
        soundUrl: "",
        explanation: ""
      },
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        },{
        textAnswer: '',
        imageUrl: "",
        soundUrl: "",
        explanation: ""
      }]
    }

    if(selected == 'guessMultipleWords'){
      this.gcard.clues = [];
      this.gcard.possibleAnswers.wrongAnswers = [];
      this.gcard.possibleAnswers.correctAnswers = [
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        },
        {
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        },{
          textAnswer: '',
          imageUrl: "",
          soundUrl: "",
          explanation: ""
        }
      ];
    }
  }
}
