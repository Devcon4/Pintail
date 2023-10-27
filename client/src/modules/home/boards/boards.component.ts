import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoardState } from '../../../states/board.state';
import { ActivatedRoute, RouterModule } from '@angular/router';
import CustomOperators from '../../../utils/custom-operators';
import { map } from 'rxjs';

@Component({
  selector: 'pin-boards',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './boards.component.html',
  styleUrls: ['./boards.component.css'],
})
export default class BoardsComponent implements OnInit {
  constructor(
    private boardState: BoardState,
    private route: ActivatedRoute
  ) {}

  public boards = this.boardState.boards.pipe(
    CustomOperators.IsDefinedSingle(),
    map((l) =>
      l.map((b) => ({
        path: `/workbench/${b.key}`,
        label: b.key,
      }))
    )
  );

  ngOnInit() {
    this.route.params.subscribe(() => {
      this.boardState.getBoards();
    });
  }
}
