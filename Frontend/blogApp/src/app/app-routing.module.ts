import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {CategoryListComponent} from "./features/category/category-list/category-list.component";
import {AddCategoryComponent} from "./features/category/add-category/add-category.component";
import {EditCategoryComponent} from "./features/category/edit-category/edit-category.component";
import {BlogpostListComponent} from "./features/blog-post/blogpost-list/blogpost-list.component";
import {AddBlogpostComponent} from "./features/blog-post/add-blogpost/add-blogpost.component";
import {EditBlogpostComponent} from "./features/blog-post/edit-blogpost/edit-blogpost.component";
import {HomeComponent} from "./features/public/home/home.component";
import {LoginComponent} from "./features/auth/login/login.component";
import {BlogDetailsComponent} from "./features/public/blog-details/blog-details.component";
import {authGuard} from "./features/auth/guards/auth.guard";


const publicRoutes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'login', component: LoginComponent },
  { path: 'blog/:url', component: BlogDetailsComponent },
];

const adminRoutes: Routes = [
  { path: 'categories', component: CategoryListComponent },
  { path: 'categories/add', component: AddCategoryComponent },
  { path: 'categories/:id', component: EditCategoryComponent },
  { path: 'blogposts', component: BlogpostListComponent },
  { path: 'blogposts/add', component: AddBlogpostComponent },
  { path: 'blogposts/:id', component: EditBlogpostComponent },
];

const routes: Routes = [
  {
    path: '',
    children: publicRoutes
  },
  {
    path: 'admin',
    children: adminRoutes,
    canActivate: [authGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
