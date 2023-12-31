import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {Observable, Subscription} from "rxjs";
import {BlogPostService} from "../services/blog-post.service";
import {BlogPost} from "../models/blog-post.model";
import {CategoryService} from "../../category/services/category.service";
import {Category} from "../../category/models/category.model";
import {UpdateBlogPost} from "../models/update-blog-post.model";
import {ImageService} from "../../../shared/components/image-selector/image.service";

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.scss']
})
export class EditBlogpostComponent implements OnInit, OnDestroy {
  id: string | null = null;
  model?: BlogPost;
  categories$?: Observable<Category[]>;
  selectedCategories?: string[];
  isVisibleImageSelector: boolean = false;

  routeSubscription?: Subscription;
  getBlogPostSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  deleteBlogPostSubscription?: Subscription;
  imageSelectSubscription?: Subscription;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private blogPostService: BlogPostService,
              private categoryService: CategoryService,
              private imageService: ImageService) {
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.routeSubscription = this.route.paramMap.subscribe({
      next: params => {
        this.id = params.get('id');

        if (this.id != null) {
          this.getBlogPostSubscription = this.blogPostService.getBlogPostById(this.id)
            .subscribe({
              next: response => {
                this.model = response;
                this.selectedCategories = response.categories
                  .map(category => category.id)
              }
            });
        }
        this.imageSelectSubscription = this.imageService.onSelectImage()
          .subscribe({
            next: response => {
             if (this.model) {
               this.model.featuredImageUrl = response.url;
               this.isVisibleImageSelector = false;
             }
            }
        });
      }
    });
  }

  onFormSubmit() {
    if (this.model && this.id) {
      let updateBlogPost: UpdateBlogPost = {
        author: this.model.author,
        content: this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl: this.model.featuredImageUrl,
        isVisible: this.model.isVisible,
        publicationDate: this.model.publicationDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories ?? [],
      }

      this.updateBlogPostSubscription = this.blogPostService.updateBlogPost(this.id, updateBlogPost)
        .subscribe({
          next: () => {
            this.router.navigateByUrl('/admin/blogposts');
          }
        })

    }
  }

  onDelete() {
    if (this.id) {
      this.deleteBlogPostSubscription = this.blogPostService.deleteBlogPost(this.id)
        .subscribe({
          next: () => {
            this.router.navigateByUrl('/admin/blogposts');
          }
        });
    }
  }

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.getBlogPostSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.deleteBlogPostSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }
}
