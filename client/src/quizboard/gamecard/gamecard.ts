import {ChangeDetectionStrategy, Component, inject, OnInit, output, ViewChild} from '@angular/core';
import {FormsModule, NgForm} from '@angular/forms';
import {GamecardType, QuizBoard} from '../../types/quizboard';
import {MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {QuizService} from '../../core/services/quiz-service';
import {DataForCardEdit} from '../../types/dataForCardEdit';
import {GameModeType} from '../../types/gamemode';
import {filter, map, Observable, startWith, tap} from 'rxjs';
import {AsyncPipe, NgOptimizedImage} from '@angular/common';
import {ToastService} from '../../core/services/toast-service';
import {ImageUpload} from '../../shared/image-upload/image-upload';
import {ImageAssignment, ImageUploadMetaData} from '../../types/imageUploadToQuizcardDto';

@Component({
  selector: 'app-gamecard',
  imports: [FormsModule, MatDialogModule, MatButtonModule, AsyncPipe, ImageUpload, NgOptimizedImage],
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

  @ViewChild(ImageUpload) uploadComponent!: ImageUpload;

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

  saveEdit(form: NgForm){
    console.log('Card was updated!');
    this.quizservice.upsertGamecard(this.gcard).subscribe({
      next: () => {
        this.toastService.success('Änderungen gespeichert');
        this.dialogRef.close();
      },
      error: (e) => {this.toastService.error('Fehler beim Speichern!'); console.log(e.message)},
    })
  }

  onImageUpload(file: File) {
    let metaData: ImageUploadMetaData = {
      quizcardId: {
        quizboardId: this.gcard.quizboardId,
        categoryId: this.gcard.categoryId!,
        valueId: this.gcard.valueId!
      },
      imageAssignment: ImageAssignment.QuestionImage,
      arrayPosition: null
    };

    console.log("Metadata set!");
    this.quizservice.uploadImage(file, metaData).subscribe({
      next: () => {
        this.toastService.success('Bild hochgeladen');
        this.uploadComponent.clearPreview();
      },
      error: (e) => {this.toastService.error('Fehler beim Hochladen!'); console.log(e.message)}
    });
  }

  deleteImage(position: number){
    console.log("Position: "+position);
  }

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
