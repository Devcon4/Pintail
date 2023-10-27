import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoardState } from '../../../states/board.state';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import CustomOperators from '../../../utils/custom-operators';
import { CardState } from '../../../states/card.state';

@Component({
  selector: 'pin-bench',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './bench.component.html',
  styleUrls: ['./bench.component.css'],
})
export default class BenchComponent {
  constructor(
    private boardState: BoardState,
    private cardState: CardState,
    route: ActivatedRoute
  ) {
    route.params.pipe(takeUntilDestroyed()).subscribe((params) => {
      const boardId = params['key'];
      this.boardState.getCurrentBoard(boardId);
    });
  }

  public currentBoardId = this.boardState.currentBoard.pipe(
    CustomOperators.IsDefinedSingle(),
    map((b) => b.id)
  );

  public cards = this.cardState.cards.pipe(CustomOperators.IsDefinedSingle());
}
