import { Component, HostListener, Inject, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoardState } from '../../../states/board.state';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { combineLatest, map, tap } from 'rxjs';
import CustomOperators from '../../../utils/custom-operators';
import { Card, CardState } from '../../../states/card.state';
import ToolkitComponent from '../toolkit/toolkit.component';
import { OverlayLookup, ToolState, ToolType } from '../../../states/tool.state';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import {
  CARD_TOOL_DATA,
  CardTool,
  CardToolComponent,
  CardToolDataFromCard,
} from '../../tools/card-tool/card-tool.component';
import { ComponentPortal } from '@angular/cdk/portal';
import { CdkDragDrop, CdkDropList } from '@angular/cdk/drag-drop';
@Component({
  selector: 'pin-bench',
  standalone: true,
  imports: [ToolkitComponent, CommonModule, RouterModule, CdkDropList],
  templateUrl: './bench.component.html',
  styleUrls: ['./bench.component.scss'],
})
export default class BenchComponent {
  constructor(
    private boardState: BoardState,
    private cardState: CardState,
    private toolState: ToolState,
    private overlay: Overlay,
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

  public cards = this.cardState.cards.pipe(
    CustomOperators.IsDefinedSingle(),
    tap((l) =>
      l.map((c) =>
        this.updateOrCreateOverlay(this.toolState.overlays.getValue(), c)
      )
    ),
    tap((l) => this.cleanupOverlays(this.toolState.overlays.getValue(), l))
  );

  @HostListener('cdkDragDropped')
  public drop(event: CdkDragDrop<unknown>) {
    console.log('dropped');
  }

  @HostListener('document:keydown.escape')
  public clearEditing() {
    this.toolState.clearCurrentEditing();
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

  public cleanupOverlays(overlayLookup: OverlayLookup, cards: Card[]) {
    // get all overlays that are not in the cards list
    const overlaysToRemove = Object.keys(overlayLookup).filter(
      (l) => !cards.some((c) => c.id === l)
    );

    overlaysToRemove.forEach((o) => this.toolState.removeOverlay(o));
  }

  public updateOrCreateOverlay(overlayLookup: OverlayLookup, card: Card) {
    const existing = overlayLookup[card.id];
    if (existing) {
      this.toolState.updateOverlayPosition(
        card.id,
        this.overlay.position().global().left(`${card.x}px`).top(`${card.y}px`)
      );
      existing.detach();
      existing.attach(this.createCardToolComponent(card));
      return;
    }
    this.toolState.setOverlay(card.id, this.createOverlay(card));
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

  public createOverlay(card: Card) {
    const overlayRef = this.overlay.create({
      positionStrategy: this.overlay
        .position()
        .global()
        .left(`${card.x}px`)
        .top(`${card.y}px`),
    });
    overlayRef.attach(this.createCardToolComponent(card));
    return overlayRef;
  }
}
