import { Injectable } from '@angular/core';
import {AddBlogPost} from "../models/add-blog-post.model";
import {Observable} from "rxjs";
import {BlogPost} from "../models/blog-post.model";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {

  constructor(private http: HttpClient) { }

  createBlogPost(data: AddBlogPost): Observable<BlogPost> {
    return this.http.post<BlogPost>(`${environment.apiBaseUrl}${environment.blogPostApi}`, data)
  }

  getAllBlogPosts(): Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${environment.apiBaseUrl}${environment.blogPostApi}`)
  }
}
