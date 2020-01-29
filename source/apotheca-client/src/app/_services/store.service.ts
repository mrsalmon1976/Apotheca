import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoreDialogComponent } from '../_components/shared/store-dialog/store-dialog.component';

import { MatDialogRef, MatDialog } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  dialogRef: MatDialogRef<StoreDialogComponent>;

  constructor(public dialog: MatDialog) { }

  public openAddStoreDialog() {

    this.dialogRef = this.dialog.open(StoreDialogComponent);
    this.dialogRef.componentInstance.title = 'Add store';
    //dialogRef.componentInstance.message = message;

    return this.dialogRef.afterClosed();
  }

}

