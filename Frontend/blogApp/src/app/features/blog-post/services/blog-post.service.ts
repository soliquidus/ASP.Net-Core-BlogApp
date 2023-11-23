import { Injectable } from '@angular/core';
import {AddBlogPost} from "../models/add-blog-post.model";
import {Observable} from "rxjs";
import {BlogPost} from "../models/blog-post.model";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../../environments/environment";
import {UpdateBlogPost} from "../models/update-blog-post.model";

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

  getBlogPostById(id: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${environment.apiBaseUrl}${environment.blogPostApi}/${id}`);
  }

  updateBlogPost(id: string, updatedBlogPost: UpdateBlogPost): Observable<BlogPost> {
    return this.http.put<BlogPost>(`${environment.apiBaseUrl}${environment.blogPostApi}/${id}`, updatedBlogPost);
  }
}
