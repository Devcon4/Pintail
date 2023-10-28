import { Component, HostListener, Inject, InjectionToken } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Card, CardState } from '../../../states/card.state';
import { EditingModel, ToolState, ToolType } from '../../../states/tool.state';
import { Subscription, filter, map, startWith, tap } from 'rxjs';
import CustomOperators from '../../../utils/custom-operators';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CdkDrag, CdkDragDrop, CdkDragRelease } from '@angular/cdk/drag-drop';

@Component({
  selector: 'pin-card-tool',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    CdkDrag,
  ],
  templateUrl: './card-tool.component.html',
  styleUrls: ['./card-tool.component.scss'],
  hostDirectives: [CdkDrag],
})
export class CardToolComponent {
  constructor(
    private toolState: ToolState,
    private cardState: CardState,
    @Inject(CARD_TOOL_DATA) public card: CardTool
  ) {
    this.toolState.saveEditing
      .pipe(
        takeUntilDestroyed(),
        IsCurrentTool('card', this.card.id),
        filter(
          (l) => l === 'Edit' && this.bodyControl.value !== this.card.body
        ),
        tap(() => this.save())
      )
      .subscribe();
  }

  public bodyControl = new FormControl(this.card.body, { nonNullable: true });

  public isEditing = this.toolState.currentEditing.pipe(
    IsCurrentTool('card', this.card.id),
    startWith('Display')
  );

  @HostListener('click')
  public edit = () => {
    this.toolState.setCurrentEditing({
      type: 'card',
      id: this.card.id,
    });
  };

  @HostListener('cdkDragDropped')
  public drop(event: CdkDragDrop<unknown>) {
    console.log('dropped');
    this.cardState.updateCard({
      ...this.card,
      x: event.dropPoint.x,
      y: event.dropPoint.y,
    });
  }

  public save = () => {
    console.log('saving', this.card.id);
    this.cardState.updateCard({
      ...this.card,
      body: this.bodyControl.value,
    });
  };

  public deleteCard = () => {
    this.toolState.clearCurrentEditing();
    this.cardState.deleteCard(this.card);
  };
}

export const CARD_TOOL_DATA = new InjectionToken<CardTool>('CARD_TOOL_DATA');

export const IsCurrentTool = (type: ToolType, id?: string) =>
  map<EditingModel | undefined, 'Edit' | 'Display'>((l) =>
    l?.type === type && l?.id === id ? 'Edit' : 'Display'
  );

export type CardTool = {
  id: string;
  boardId: string;
  x: number;
  y: number;
  body: string;
};

export const CardToolDataFromCard = (card: Card): CardTool => ({
  id: card.id,
  boardId: card.boardId,
  x: card.x,
  y: card.y,
  body: card.body,
});
