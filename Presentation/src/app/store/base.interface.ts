import { HttpErrorResponse } from '@angular/common/http';

export interface IBase {
    isActive: boolean;
    success: boolean;
    error: HttpErrorResponse | undefined;
  }
