import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EnumToArrayPipe } from './enum-to-array.pipe';

@NgModule({
    declarations: [EnumToArrayPipe],
    imports: [CommonModule],
    exports: [EnumToArrayPipe]
  })
  export class PipesModule {}
