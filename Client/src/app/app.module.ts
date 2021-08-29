// modules (keep alpahabetical)
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';

// components (keep alphabetical)
import { AppComponent } from './app.component';
import { LoginComponent } from './modules/login/login.component';
import { BaseComponent } from './modules/base/base.component';
import { HttpService } from './services/http/http.service';
import { DashboardComponent } from './modules/dashboard/dashboard.component';

@NgModule({
  declarations: [AppComponent, LoginComponent, BaseComponent, DashboardComponent],
  imports: [BrowserModule, FormsModule, FormsModule, HttpClientModule, AppRoutingModule],
  providers: [HttpService],
  bootstrap: [AppComponent],
})
export class AppModule { }
