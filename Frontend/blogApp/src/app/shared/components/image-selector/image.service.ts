import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {BlogImage} from "../../Models/blog-image.model";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) { }

  getAllImages(): Observable<BlogImage[]> {
    return this.http.get<BlogImage[]>(`${environment.apiBaseUrl}${environment.imageApi}`);
  }

  uploadImage(file: File, fileName: string, title: string): Observable<BlogImage> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('fileName', fileName);
    formData.append('title', title);

   return this.http.post<BlogImage>(`${environment.apiBaseUrl}${environment.imageApi}`, formData);
  }
}
