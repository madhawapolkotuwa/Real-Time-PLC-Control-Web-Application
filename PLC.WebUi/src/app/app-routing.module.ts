import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ConnectComponent } from './components/connect/connect.component';
import { RouteActivateService } from './services/route-activate.service';

const routes: Routes = [
  {path:'',component:ConnectComponent},
  {path:'dashboard',component:DashboardComponent,canActivate:mapToCanActivate([RouteActivateService])} // ,canActivate:mapToCanActivate([RouteActivateService])
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
