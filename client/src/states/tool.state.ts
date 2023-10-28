import { OverlayRef, PositionStrategy } from '@angular/cdk/overlay';
import { ComponentRef, Injectable } from '@angular/core';
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

  public tools = new BehaviorSubject<ToolLookup>({});

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

  public clearTools() {
    Object.entries(this.tools.getValue()).forEach(([key, [o, c]]) => {
      c.destroy();
      o.dispose();
    });
    this.tools.next({});
  }

  public setTool(id: string, tool: ToolRef) {
    this.tools.next({
      ...this.tools.getValue(),
      [id]: tool,
    });
  }

  public removeTool(id: string) {
    const overlays = this.tools.getValue();
    const [o, c] = overlays[id];
    c.destroy();
    o?.dispose();
    delete overlays[id];
    this.tools.next(overlays);
  }

  public updateToolPosition(id: string, strategy: PositionStrategy) {
    const overlays = this.tools.getValue();
    const [o, c] = overlays[id];
    o?.updatePositionStrategy(strategy);
    this.tools.next(overlays);
  }
}
export type ToolRef = [OverlayRef, ComponentRef<unknown>];
export type ToolLookup = Record<string, ToolRef>;
export type ToolType = 'card' | 'label';

export type EditingModel = {
  id: string;
  type: ToolType;
};
