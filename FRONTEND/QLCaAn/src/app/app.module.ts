import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PhongBanComponent } from './phong-ban/phong-ban.component';
import { DanhsachphongbanComponent } from './phong-ban/danhsachphongban/danhsachphongban.component';
import { FormsModule ,ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SharedService } from './shared.service';

@NgModule({
  declarations: [
    AppComponent,
    PhongBanComponent,
    DanhsachphongbanComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
