import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EnumToArrayPipe } from './enum-to-array.pipe';
import { OrderByPipe } from './order-by.pipe';

@NgModule({
    declarations: [EnumToArrayPipe, OrderByPipe],
    imports: [CommonModule],
    exports: [EnumToArrayPipe, OrderByPipe]
  })
  export class PipesModule {}
