import { OverlayRef, PositionStrategy } from '@angular/cdk/overlay';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToolState {
  public currentTool = new BehaviorSubject<ToolType>('card');
  public currentEditing = new BehaviorSubject<EditingModel | undefined>(
    undefined
  );

  public saveEditing = new BehaviorSubject<EditingModel | undefined>(undefined);

  public overlays = new BehaviorSubject<OverlayLookup>({});

  public setCurrentTool(tool: ToolType) {
    this.currentTool.next(tool);
  }

  public setCurrentEditing(editing: EditingModel) {
    this.currentEditing.next(editing);
  }

  public clearCurrentEditing() {
    this.saveEditing.next(this.currentEditing.getValue());
    this.currentEditing.next(undefined);
  }

  public setOverlay(id: string, overlay: OverlayRef) {
    this.overlays.next({
      ...this.overlays.getValue(),
      [id]: overlay,
    });
  }

  public removeOverlay(id: string) {
    const overlays = this.overlays.getValue();
    overlays[id]?.dispose();
    delete overlays[id];
    this.overlays.next(overlays);
  }

  public updateOverlayPosition(id: string, strategy: PositionStrategy) {
    const overlays = this.overlays.getValue();
    overlays[id]?.updatePositionStrategy(strategy);
    this.overlays.next(overlays);
  }
}
export type OverlayLookup = Record<string, OverlayRef>;
export type ToolType = 'card' | 'label';

export type EditingModel = {
  id: string;
  type: ToolType;
};
