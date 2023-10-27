import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CardState } from './card.state';

@Injectable({
  providedIn: 'root',
})
export class BoardState {
  constructor(
    public http: HttpClient,
    private cardState: CardState
  ) {}

  public currentBoard = new BehaviorSubject<Board | undefined>(undefined);
  public boards = new BehaviorSubject<Board[] | undefined>(undefined);

  public getCurrentBoard(boardId: string) {
    this.currentBoard.next(undefined);
    this.http.get<Board>(`/api/boards/${boardId}`).subscribe((board) => {
      this.currentBoard.next(board);
      this.cardState.getCards(board.id);
    });
  }

  public getBoards() {
    this.boards.next(undefined);
    this.http.get<Board[]>('/api/boards').subscribe((boards) => {
      this.boards.next(boards);
    });
  }
}

export type Board = {
  id: string;
  key: string;
};
