import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-store-dialog',
  templateUrl: './store-dialog.component.html',
  styleUrls: ['./store-dialog.component.less']
})
export class StoreDialogComponent implements OnInit {

  public title: string;
  
  constructor(private dialogRef: MatDialogRef<StoreDialogComponent>) { }

  ngOnInit() {
  }

}
