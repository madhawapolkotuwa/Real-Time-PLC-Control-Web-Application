import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { RouteActivateService } from 'src/app/services/route-activate.service';

@Component({
  selector: 'app-connect',
  templateUrl: './connect.component.html',
  styleUrls: ['./connect.component.css']
})
export class ConnectComponent implements OnInit{

  public addressForm!: FormGroup;

  constructor(
    private fb:FormBuilder,
    private router: Router, 
    private routeActiveService:RouteActivateService,
    private toast:NgToastService){}

  ngOnInit(): void {
    if( this.routeActiveService.isDashboardActivated){
      this.routeActiveService.disconnect().subscribe({
        next:(res) => {
          this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
        },
        error:(err) => {
          this.toast.error({detail:"ERROR", summary:err.message,duration:5000});
        }
      });
      this.routeActiveService.isDashboardActivated = false;
    }
      this.addressForm = this.fb.group({
        ipaddress: ['',[Validators.required, customValidatorIp()]],
        port:['',[Validators.required,customValidatorPort()] ]
      });
  }

  get ipaddress(): FormControl {
    return this.addressForm.get("ipaddress") as FormControl;
  }

  onConnect(){
    if (this.addressForm.valid){
        this.routeActiveService.connect(this.addressForm.value).subscribe({
          next:(res) => {
            this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
            this.routeActiveService.isDashboardActivated = true;
            this.router.navigate(['dashboard']);
          },
          error:(err) => {
            this.toast.error({detail:"ERROR", summary:err.message, duration:5000});
          }
        });
    }
  }

}

export function customValidatorIp(): ValidatorFn {
  return (control: AbstractControl) => {
    const regex = /^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    if (regex.test(control.value)) {
      return null;
    }
    return { ipError: true };
  };
}

export function customValidatorPort(): ValidatorFn {
  return (control: AbstractControl) => {
    const regex = /^([0-9]){4}$/;
    if (regex.test(control.value)) {
      return null;
    }
    return { ipError: true };
  };
}
