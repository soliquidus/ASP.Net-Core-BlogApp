import {Component, OnDestroy, OnInit} from '@angular/core';
import {AddBlogPost} from "../models/add-blog-post.model";
import {BlogPostService} from "../services/blog-post.service";
import {Router} from "@angular/router";
import {CategoryService} from "../../category/services/category.service";
import {Observable, Subscription} from "rxjs";
import {Category} from "../../category/models/category.model";
import {ImageService} from "../../../shared/components/image-selector/image.service";

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.scss']
})
export class AddBlogpostComponent implements OnInit, OnDestroy {

  model: AddBlogPost;
  categories$?: Observable<Category[]>;
  isVisibleImageSelector: boolean = false;

  imageSelectionSubscription?: Subscription;

  constructor(private blogPostService: BlogPostService,
              private router: Router,
              private categoryService: CategoryService,
              private imageService: ImageService) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisible: true,
      publishedDate: new Date(),
      categories: []
    };
  }

  ngOnInit(): void {
    this.categories$ = this.categoryService.getAllCategories();

    this.imageSelectionSubscription = this.imageService.onSelectImage()
      .subscribe({
        next: selectedImage => {
          this.model.featuredImageUrl = selectedImage.url;
          this.isVisibleImageSelector = false;
        }
      });
  }

  onFormSubmit() {
    this.blogPostService.createBlogPost(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/admin/blogposts');
      }
    });
  }

  ngOnDestroy() {
    this.imageSelectionSubscription?.unsubscribe();
  }
}
