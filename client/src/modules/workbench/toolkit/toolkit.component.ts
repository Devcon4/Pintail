import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToolState, ToolType } from '../../../states/tool.state';
import { Observable, map } from 'rxjs';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'pin-toolkit',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule, MatTooltipModule],
  templateUrl: './toolkit.component.html',
  styleUrls: ['./toolkit.component.scss'],
})
export default class ToolkitComponent implements AfterViewInit {
  constructor(
    private ToolState: ToolState,
    private cd: ChangeDetectorRef
  ) {}

  @ViewChild('card') public cardTemplateRef: TemplateRef<unknown> | undefined =
    undefined;
  @ViewChild('label') public labelTemplateRef:
    | TemplateRef<unknown>
    | undefined = undefined;

  public toolkitItems: ToolkitItem[] = [];

  public IsCurrentTool = (tool: string) =>
    this.ToolState.currentTool.pipe(map((l) => l === tool));

  public createToolItem = (
    toolType: ToolType,
    tooltip: string,
    template: TemplateRef<unknown>
  ) => ({
    template,
    action: () => this.ToolState.setCurrentTool(toolType),
    isCurrent: this.IsCurrentTool(toolType),
    tooltip,
  });

  ngAfterViewInit() {
    this.toolkitItems = [
      this.createToolItem('card', 'Create Card', this.cardTemplateRef!),
      this.createToolItem('label', 'Create Label', this.labelTemplateRef!),
    ];

    this.cd.detectChanges();
  }
}

type ToolkitItem = {
  template: TemplateRef<unknown>;
  action: () => void;
  isCurrent: Observable<boolean>;
  tooltip: string;
};
