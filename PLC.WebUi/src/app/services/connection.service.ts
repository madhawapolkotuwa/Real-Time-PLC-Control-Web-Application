import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {

  private signalrConnection?: HubConnection;
  
  public counter:number = 0;

  constructor() { }

  createSignalRConnection() {

    this.signalrConnection = new HubConnectionBuilder().withUrl(`${environment.apiUrl}hubs/connection`).withAutomaticReconnect().build();

    this.signalrConnection.start().catch(err => {
      console.log(err);
    });

    this.signalrConnection.on('HubConnected', () => {
      console.log('the server call here');
    })

    this.signalrConnection.on('CounterValue',(counter:number) => {
      this.counter = counter;
      //console.log(counter);
    })

  }

  stopConnection() {
    this.signalrConnection?.stop().catch(error => console.log(error));
  }



}
