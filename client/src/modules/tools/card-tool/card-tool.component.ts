import { CdkDrag, CdkDragRelease } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  Inject,
  InjectionToken,
  ViewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { debounceTime, filter, map, startWith, tap } from 'rxjs';
import { Card, CardState } from '../../../states/card.state';
import { EditingModel, ToolState, ToolType } from '../../../states/tool.state';

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
    this.bodyControl.valueChanges
      .pipe(
        takeUntilDestroyed(),
        debounceTime(120),
        filter((l) => l !== this.card.body),
        tap(() => this.save())
      )
      .subscribe();
    // this.toolState.saveEditing
    //   .pipe(
    //     takeUntilDestroyed(),
    //     IsCurrentTool('card', this.card.id),
    //     filter(
    //       (l) => l === 'Edit' && this.bodyControl.value !== this.card.body
    //     ),
    //     tap(() => this.save())
    //   )
    //   .subscribe();
  }

  @ViewChild('text') textareaRef: ElementRef = null!;

  public bodyControl = new FormControl(this.card.body, { nonNullable: true });

  public isEditing = this.toolState.currentEditing.pipe(
    IsCurrentTool('card', this.card.id),
    startWith('Display'),
    tap((l) => setTimeout(() => this.textareaRef?.nativeElement?.focus()))
  );

  @HostListener('click')
  public edit = () => {
    this.toolState.setCurrentEditing({
      type: 'card',
      id: this.card.id,
    });
  };

  public drop = (event: CdkDragRelease<any>) => {
    var rect = event.source.element.nativeElement.getBoundingClientRect();
    var pos = { x: rect.x, y: rect.y };
    this.cardState.updateCard({
      ...this.card,
      ...pos,
    });
  };

  public save = () => {
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
