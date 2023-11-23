export interface UpdateBlogPost {
  title: string;
  shortDescription: string;
  content: string;
  featuredImageUrl: string;
  urlHandle: string;
  author: string;
  publicationDate: Date;
  isVisible: boolean;
  categories: string[];
}
