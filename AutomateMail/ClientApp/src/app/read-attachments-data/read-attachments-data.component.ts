import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpClientMailService } from '../services/http-client-mail.service';
import * as XLSX from 'xlsx';
import { Observable, Subscription, timer, interval } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-read-attachment-data',
  templateUrl: './read-attachments-data.component.html'
})
export class ReadAttachmentDataComponent implements OnInit {

  file: any;
  arrayBuffer: any;
  filelist: any;
  attachmentList: any;
  subscription: Subscription;
  statusText: string;
  observableRef: any;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private httpClientMailService: HttpClientMailService) {

  }

  ngOnInit() {
    interval(5 * 60 * 1000)
      .subscribe(() => {
        this.httpClientMailService.getDownloadAttachment().subscribe(data => {
          this.attachmentList = data;
          this.saveAttachmentData();
        });
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  
  saveAttachmentData() {
    this.httpClientMailService.saveAttachmentContent(this.attachmentList).subscribe(result => {
      alert(result);
    });
  }
}


