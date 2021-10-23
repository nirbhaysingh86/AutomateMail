import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MailService } from './mail.service';
const cudOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
@Injectable({
  providedIn: 'root'
})

export class HttpClientMailService extends MailService {
   
  constructor(private http: HttpClient) {
    super();
  }

  getDownloadAttachment(): Observable<any[]> {
    return this.http.get<any[]>(this.airlinesUrl).pipe(
      catchError(this.handleError)
    );
  }

  saveAttachmentContent(quote: any): Observable<any> {
    return this.http.post<any>(this.airlinesUrl +'/saveattachmentdata', quote, cudOptions).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    console.error(error);
    return throwError(error);
  }

}
