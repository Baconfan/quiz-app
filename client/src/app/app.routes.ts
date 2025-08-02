import { Routes } from '@angular/router';
import {Home} from '../home/home';
import {QuizboardComponent} from '../quizboard/quizboard.component';
import {authGuard} from '../core/guards/auth-guard';

export const routes: Routes = [
  {path: '', component: Home},
  {path: 'editor', component: QuizboardComponent, canActivate: [authGuard]}
];
