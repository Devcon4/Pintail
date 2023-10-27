import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CardState {
  constructor(private http: HttpClient) {}

  public cards = new BehaviorSubject<Card[] | undefined>(undefined);

  public getCards(boardId: string) {
    this.cards.next(undefined);
    this.http
      .get<Card[]>(`/api/boards/${boardId}/cards`)
      .subscribe((cards) => this.cards.next(cards));
  }
}

export type Card = {
  id: string;
  boardId: string;
  x: number;
  y: number;
  body: string;
};
