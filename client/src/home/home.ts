import {Component, inject, OnInit} from '@angular/core';
import {QuizboardComponent} from '../quizboard/quizboard.component';
import {HealthcheckService} from '../core/services/healthcheck-service';
import {
  catchError,
  map,
  Observable,
  of,
  scan,
  startWith,
  switchMap, takeWhile,
  timer
} from 'rxjs';
import {AsyncPipe} from '@angular/common';

@Component({
  selector: 'app-home',
  imports: [
    QuizboardComponent,
    AsyncPipe
  ],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  healthCheckService = inject(HealthcheckService);

  healthStat$!: Observable<string>;

  ngOnInit() {
    const MAX_CONSECUTIVE_FAILS = 20;

    this.healthStat$ = timer(0, 5000).pipe(
      switchMap(() =>
        this.healthCheckService.checkHealth().pipe(
          map(() => ({ error: false as const, msg: 'Healthy' })),
          catchError(() => of({ error: true as const, msg: 'Unhealthy or unreachable' }))
        )
      ),
      scan(
        (state, curr) => {
          const fails = curr.error ? state.fails + 1 : 0;
          return { fails, msg: curr.msg, stop: fails >= MAX_CONSECUTIVE_FAILS };
        },
        { fails: 0, msg: 'Checking...', stop: false }
      ),
      takeWhile(s => !s.stop, true), // include the final emission
      map(s => s.msg),
      startWith('Checking...')
    );
  }
}
