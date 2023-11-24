import {Component, OnInit, ViewChild} from '@angular/core';
import {ImageService} from "./image.service";
import {BlogImage} from "../../Models/blog-image.model";
import {Observable} from "rxjs";
import {NgForm} from "@angular/forms";

@Component({
  selector: 'app-image-selector',
  templateUrl: './image-selector.component.html',
  styleUrls: ['./image-selector.component.scss']
})
export class ImageSelectorComponent implements OnInit {
  private file?: File;
  fileName: string = '';
  title: string = '';
  images$?: Observable<BlogImage[]>;

  // To reset form when upload is done
  @ViewChild('form', {static: false}) imageUploadForm?: NgForm;

  constructor(private imageService: ImageService) {
  }

  ngOnInit() {
    this.getImages();
  }

  onFileUploadChange(event: Event) {
    const element = event.currentTarget as HTMLInputElement;
    this.file = element.files?.[0];
  }

  uploadImage() {
    if (this.file && this.fileName !== '' && this.title !== '') {
      this.imageService.uploadImage(this.file, this.fileName, this.title)
        .subscribe({
          next: () => {
            this.imageUploadForm?.resetForm();
            this.getImages();
          }
        });
    }
  }

  private getImages(){
    this.images$ = this.imageService.getAllImages();
  }

  selectImage(image: BlogImage) {
    this.imageService.selectImage(image);
  }
}
