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

  public createCard(card: CreateCardCommand) {
    this.http
      .post(`/api/boards/${card.boardId}/cards`, card)
      .subscribe(() => this.getCards(card.boardId));
  }

  public deleteCard(card: Card) {
    this.http
      .delete(`/api/boards/${card.boardId}/cards/${card.id}`)
      .subscribe(() => this.getCards(card.boardId));
  }

  public updateCard(card: Card) {
    this.http
      .put(`/api/boards/${card.boardId}/cards/${card.id}`, card)
      .subscribe(() => this.getCards(card.boardId));
  }
}

export type CreateCardCommand = {
  boardId: string;
  x: number;
  y: number;
  body: string;
};

export type Card = {
  id: string;
  boardId: string;
  x: number;
  y: number;
  body: string;
};
