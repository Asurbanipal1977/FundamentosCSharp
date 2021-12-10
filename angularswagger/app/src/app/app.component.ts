import { Component } from '@angular/core';
import { MinimalApiService } from './api/services';
import { Cerveza, Data } from './api/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  public cerveza?: Cerveza;
  public cervezas?: Cerveza[];

  public constructor (private api: MinimalApiService)
  {
  }

  ngOnInit(): void {
    this.api.cervezaIdGet({id:1}).subscribe(res=> {
      console.log(res);
    });
    
    this.api.cervezasGet().subscribe(res=> {
      this.cervezas=res;
    });
  }




}
