import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { environment } from 'src/environments/environment';
import { XWriteModel } from '../models/xWrite.Model';

@Injectable({
  providedIn: 'root'
})
export class RouteActivateService {

  public isDashboardActivated: boolean = false;

  constructor(private http:HttpClient,private toast: NgToastService,private router: Router) { }

  connect(ipObj:any){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/connect`,ipObj)
  }

  disconnect(){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/disconnect`,{})
  }

  plcTaskStart(){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/plcTaskStart`,{})
  }

  counterReset(){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/counterReset`,{})
  }

  xWrite(xobj:any){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/xwrite`,xobj)
    //console.log(xobj);
  }

  dWrite(dobj:any){
    return this.http.post<any>(`${environment.apiUrl}api/Plc/dwrite`,dobj)
  }

  canActivate(): boolean {
    return this.isConnected();
  }

  isConnected() : boolean{
    if(this.isDashboardActivated)
       return true;
    else
      this.toast.error({detail:"ERROR", summary:"Please connect First!"});
      this.router.navigate(['']);
      return false;
   // return !!localStorage.getItem('token'); // !! string to boolean if there is a token return true, if not return falus
  }

}
