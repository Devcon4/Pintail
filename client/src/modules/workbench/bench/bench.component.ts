import { CommonModule } from '@angular/common';
import { Component, HostListener, Injector } from '@angular/core';
import {
  ActivatedRoute,
  NavigationStart,
  Router,
  RouterModule,
} from '@angular/router';
import { BoardState } from '../../../states/board.state';

import { CdkDrag, CdkDropList } from '@angular/cdk/drag-drop';
import { Overlay } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { filter, map, tap } from 'rxjs';
import { Card, CardState } from '../../../states/card.state';
import { ToolLookup, ToolRef, ToolState } from '../../../states/tool.state';
import CustomOperators from '../../../utils/custom-operators';
import {
  CARD_TOOL_DATA,
  CardToolComponent,
  CardToolDataFromCard,
} from '../../tools/card-tool/card-tool.component';
import ToolkitComponent from '../toolkit/toolkit.component';
@Component({
  selector: 'pin-bench',
  standalone: true,
  imports: [
    ToolkitComponent,
    CommonModule,
    RouterModule,
    CdkDropList,
    MatProgressSpinnerModule,
    CdkDrag,
  ],
  templateUrl: './bench.component.html',
  styleUrls: ['./bench.component.scss'],
})
export default class BenchComponent {
  constructor(
    private boardState: BoardState,
    private cardState: CardState,
    private toolState: ToolState,
    private overlay: Overlay,
    route: ActivatedRoute,
    router: Router
  ) {
    route.params.pipe(takeUntilDestroyed()).subscribe((params) => {
      const boardId = params['key'];
      this.boardState.getCurrentBoard(boardId);
    });

    // when we navigate away from the board, clear the current board
    router.events
      .pipe(
        takeUntilDestroyed(),
        filter(
          (e) => e instanceof NavigationStart && !e.url.includes('workbench')
        )
      )
      .subscribe((event) => this.clearBoard());
  }

  public currentBoardId = this.boardState.currentBoard.pipe(
    CustomOperators.IsDefinedSingle(),
    map((b) => b.id)
  );

  public cards = this.cardState.cards.pipe(
    CustomOperators.IsDefinedSingle(),
    tap((l) => l.map((c) => this.updateOrCreateTool(c))),
    tap((l) => this.cleanupTools(this.toolState.tools.getValue(), l))
  );

  public drop(event: any) {
    console.log('dropped');
  }

  @HostListener('document:keydown.escape')
  public clearEditing() {
    this.toolState.clearCurrentEditing();
  }

  public clearBoard() {
    this.toolState.clearCurrentEditing();
    this.toolState.clearTools();
    this.cardState.clearCards();
  }

  public useTool(event: MouseEvent) {
    if (this.toolState.currentEditing.getValue() !== undefined) {
      this.toolState.clearCurrentEditing();
      return;
    }
    const currentBoardId = this.boardState.currentBoard.getValue()?.id;
    this.cardState.createCard({
      boardId: currentBoardId!,
      x: event.clientX,
      y: event.clientY,
      body: '',
    });
  }

  public cleanupTools(toolLookup: ToolLookup, cards: Card[]) {
    // get all tools that are not in the cards list
    const toolsToRemove = Object.keys(toolLookup).filter(
      (l) => !cards.some((c) => c.id === l)
    );

    toolsToRemove.forEach((o) => this.toolState.removeTool(o));
  }

  public updateOrCreateTool(card: Card) {
    debugger;
    const overlayLookup = this.toolState.tools.getValue();
    const existing = overlayLookup[card.id];
    if (existing) {
      const [o, c] = existing;
      // update the CARD_TOOL_DATA to reflect the new card

      if (c.instance instanceof CardToolComponent) {
        c.instance.card = CardToolDataFromCard(card);
      }

      return;
    }
    this.toolState.setTool(card.id, this.createTool(card));
  }

  public createCardToolComponent(card: Card) {
    return new ComponentPortal(
      CardToolComponent,
      null,
      Injector.create({
        providers: [
          { provide: CARD_TOOL_DATA, useValue: CardToolDataFromCard(card) },
        ],
      })
    );
  }

  public createTool(card: Card) {
    const overlayRef = this.overlay.create({
      positionStrategy: this.overlay
        .position()
        .global()
        .left(`${card.x}px`)
        .top(`${card.y}px`),
    });
    const compRef = overlayRef.attach(this.createCardToolComponent(card));
    return [overlayRef, compRef] as ToolRef;
  }
}
