import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoardState } from '../../../states/board.state';
import { ActivatedRoute, RouterModule } from '@angular/router';
import CustomOperators from '../../../utils/custom-operators';
import { map } from 'rxjs';
import { ThemeState, ThemeType } from '../../../services/theme.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'pin-boards',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, MatIconModule],
  templateUrl: './boards.component.html',
  styleUrls: ['./boards.component.scss'],
})
export default class BoardsComponent {
  constructor(
    private boardState: BoardState,
    private themeState: ThemeState,
    route: ActivatedRoute
  ) {
    route.params.pipe(takeUntilDestroyed()).subscribe(() => {
      this.boardState.getBoards();
    });
  }

  public boards = this.boardState.boards.pipe(
    CustomOperators.IsDefinedSingle(),
    map((l) =>
      l.map((b) => ({
        path: `/workbench/${b.key}`,
        label: b.key,
      }))
    )
  );

  isDark = this.themeState.theme.pipe(
    map<ThemeType, boolean>((l) => l === 'dark')
  );
  isLight = this.themeState.theme.pipe(
    map<ThemeType, boolean>((l) => l === 'light')
  );

  toggleTheme = () => this.themeState.toggleTheme();
}
