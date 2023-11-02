import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { XWriteModel } from 'src/app/models/xWrite.Model';
import { ConnectionService } from 'src/app/services/connection.service';
import { RouteActivateService } from 'src/app/services/route-activate.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{

  public xInputForm!: FormGroup;
  public dInputForm!: FormGroup;

  constructor(
    private fb:FormBuilder,
    private router: Router, 
    private routeActiveService:RouteActivateService,
    private toast:NgToastService,
    public hubConnectionService:ConnectionService
    ) {}

  
  ngOnInit(): void {

    this.hubConnectionService.createSignalRConnection();

    this.xInputForm = this.fb.group({
      x0: [0,[Validators.required]],
      x1:[0,[Validators.required] ],
      x2:[0,[Validators.required] ],
      x3:[0,[Validators.required] ],
      x4:[0,[Validators.required] ],
      x5:[0,[Validators.required] ]
    });

    this.dInputForm = this.fb.group({
      d100: [0,[Validators.required]],
      d101:[0,[Validators.required] ],
      d102:[0,[Validators.required] ],
      d103:[0,[Validators.required] ],
      d104:[0,[Validators.required] ],
      d105:[0,[Validators.required] ]
    });

    this.routeActiveService.plcTaskStart().subscribe({
      next:res =>{
        this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
      },
      error:err =>{
        this.toast.error({detail:"ERROR", summary:err.message, duration:5000});
      }
    });
  }

  onDisconnect(){
    this.routeActiveService.disconnect().subscribe({
      next:(res)=> {
        this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
        this.routeActiveService.isDashboardActivated = false;
        this.hubConnectionService.stopConnection();
        this.router.navigate(['']);
      },
      error:(err) => {
        this.toast.error({detail:"Error", summary:err.message, duration:5000});
      }
    });
  }

  OnXWrite(){
    const booleanArray = [
      this.xInputForm.value.x0 ? true:false ,
      this.xInputForm.value.x1 ? true:false ,
      this.xInputForm.value.x2 ? true:false ,
      this.xInputForm.value.x3 ? true:false ,
      this.xInputForm.value.x4 ? true:false ,
      this.xInputForm.value.x5 ? true:false
    ];
    const xdata = { booleanData: booleanArray};
    //console.log(this.x.x);
    this.routeActiveService.xWrite(xdata).subscribe({
      next:res =>{
        this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
      },
      error:err =>{
        this.toast.error({detail:"Error", summary:err.message, duration:5000});
      }
    })
  }

  OnDWrite(){
    const dDataArray = [
      this.dInputForm.value.d100,
      this.dInputForm.value.d101,
      this.dInputForm.value.d102,
      this.dInputForm.value.d103,
      this.dInputForm.value.d104,
      this.dInputForm.value.d105
    ];
    //console.log(this.dInputForm.value);

     const ddata = {ushortData:dDataArray};
    
    this.routeActiveService.dWrite(ddata).subscribe({
      next:res =>{
        this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
      },
      error:err =>{
        this.toast.error({detail:"Error", summary:err.message, duration:5000});
      }
    })
  }

  onRestCounter(){
    this.routeActiveService.counterReset().subscribe({
      next:res =>{
        this.toast.success({detail:"SUCCESS", summary:res.message, duration:5000});
      },
      error: err =>{
        this.toast.error({detail:"Error", summary:err.message, duration:5000});
      }
    });
  }

}
