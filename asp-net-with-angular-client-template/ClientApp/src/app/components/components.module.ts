import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MaterialModule } from "../modules/shared/material.module";
import { SharedModule } from "../modules/shared/shared.module";
import { SpinnerComponent } from './spinner/spinner.component';


const components = [
    SpinnerComponent
];

@NgModule({
  declarations: [
    ...components
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    SharedModule,
    FormsModule,
  ],
  exports: [
    ...components
  ]
})
export class ComponentsModule { }
