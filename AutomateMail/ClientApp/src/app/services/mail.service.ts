import {Injectable } from '@angular/core';

import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})

export abstract class MailService {
  airlinesUrl ='https://localhost:44375/mail';
  
  abstract getDownloadAttachment(): Observable<any[]>;
  abstract saveAttachmentContent(quote : any): Observable<any>;

}
